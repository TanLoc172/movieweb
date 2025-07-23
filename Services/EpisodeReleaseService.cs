// Tạo file: Services/EpisodeReleaseService.cs

// using Microsoft.AspNetCore.SignalR;
// using MovieWebsite.Hubs;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using MovieWebsite.Data;
// using MovieWebsite.Hubs;

// public class EpisodeReleaseService : IHostedService, IDisposable
// {
//     private Timer _timer;
//     private readonly ILogger<EpisodeReleaseService> _logger;
//     private readonly IServiceProvider _serviceProvider;
    

//     public EpisodeReleaseService(ILogger<EpisodeReleaseService> logger, IServiceProvider serviceProvider)
//     {
//         _logger = logger;
//         _serviceProvider = serviceProvider;
//     }

//     public Task StartAsync(CancellationToken cancellationToken)
//     {
//         _logger.LogInformation("Dịch vụ kiểm tra lịch chiếu đang khởi động.");
//         // Kiểm tra mỗi 30 giây. Bạn có thể điều chỉnh khoảng thời gian này.
//         _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3000000));
//         return Task.CompletedTask;
//     }

//     private async void DoWork(object state)
//     {
//         _logger.LogInformation("Dịch vụ đang kiểm tra các tập phim mới.");

//         // Chúng ta cần một service scope để resolve các service có đời sống scoped như DbContext
//         using (var scope = _serviceProvider.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//             var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ScheduleHub>>();

//             var now = DateTime.UtcNow;

//             // Tìm các lịch chiếu vừa đến hạn và có tập phim chưa được đánh dấu là đã publish
//             // (Sử dụng cờ IsPublished ở đây như một biện pháp an toàn để tránh gửi nhiều thông báo)
//             var newlyReleasedSchedules = await dbContext.Schedules
//                 .Include(s => s.Episode)
//                 .Where(s => s.Episode != null && !s.Episode.IsPublished && s.ScheduledTime <= now)
//                 .ToListAsync();

//             if (!newlyReleasedSchedules.Any()) return;

//             foreach (var schedule in newlyReleasedSchedules)
//             {
//                 var episode = schedule.Episode;
//                 episode.IsPublished = true; // Đánh dấu là đã publish để không gửi thông báo lại

//                 _logger.LogInformation($"Tập '{episode.Title}' (ID: {episode.Id}) của Phim ID {episode.MovieId} đã ra mắt!");

//                 // Thông báo cho tất cả các client đang kết nối về tập phim mới
//                 await hubContext.Clients.All.SendAsync("NewEpisodeReleased", new
//                 {
//                     movieId = episode.MovieId,
//                     episodeId = episode.Id,
//                     episodeNumber = episode.EpisodeNumber,
//                     title = episode.Title
//                 });
//             }

//             await dbContext.SaveChangesAsync();
//         }
//     }

//     public Task StopAsync(CancellationToken cancellationToken)
//     {
//         _logger.LogInformation("Dịch vụ kiểm tra lịch chiếu đang dừng.");
//         _timer?.Change(Timeout.Infinite, 0);
//         return Task.CompletedTask;
//     }

//     public void Dispose()
//     {
//         _timer?.Dispose();
//     }
// }
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Hubs;
using MovieWebsite.Interfaces; // Quan trọng: using cho INotificationService

public class EpisodeReleaseService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<EpisodeReleaseService> _logger;
    private readonly IServiceProvider _serviceProvider; // Dùng để tạo scope

    // Constructor CHỈ inject IServiceProvider, KHÔNG inject INotificationService trực tiếp
    public EpisodeReleaseService(ILogger<EpisodeReleaseService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dịch vụ kiểm tra lịch chiếu đang khởi động.");
        // Đặt thời gian kiểm tra. Ví dụ: 1 phút một lần.
        _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    // Phương thức này thực hiện công việc chính
    private async void DoWork(object state)
    {
        _logger.LogInformation("Dịch vụ đang kiểm tra các tập phim đã đến lịch chiếu.");

        // BƯỚC 1: Tạo một scope để lấy các dịch vụ Scoped
        using (var scope = _serviceProvider.CreateScope())
        {
            // BƯỚC 2: Lấy các dịch vụ cần thiết TỪ SCOPE, không phải từ constructor
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ScheduleHub>>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>(); // <-- LẤY DỊCH VỤ THÔNG BÁO Ở ĐÂY

            var now = DateTime.UtcNow;

            // Tìm các lịch chiếu đã đến hạn và tập phim chưa được công bố
            var newlyReleasedSchedules = await dbContext.Schedules
                .Include(s => s.Episode)
                .Where(s => s.Episode != null && !s.Episode.IsPublished && s.ScheduledTime <= now)
                .ToListAsync();

            if (!newlyReleasedSchedules.Any())
            {
                return; // Không có gì để làm
            }

            foreach (var schedule in newlyReleasedSchedules)
            {
                var episode = schedule.Episode;
                if (episode == null) continue;

                // Cập nhật trạng thái tập phim
                episode.IsPublished = true;
                _logger.LogInformation($"Công chiếu tập '{episode.Title}' của Phim ID {episode.MovieId}!");

                // Gửi thông báo real-time qua SignalR
                await hubContext.Clients.All.SendAsync("NewEpisodeReleased", new
                {
                    movieId = episode.MovieId,
                    episodeId = episode.Id,
                    episodeNumber = episode.EpisodeNumber,
                    title = episode.Title
                });

                // BƯỚC 3: THỰC HIỆN THÔNG BÁO bằng cách gọi service đã lấy từ scope
                try
                {
                    // GỌI HÀM TẠO THÔNG BÁO Ở ĐÂY
                    await notificationService.CreateNewEpisodeNotificationAsync(episode);
                    _logger.LogInformation("Đã yêu cầu tạo thông báo trong database cho tập phim ID {EpisodeId}.", episode.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi notificationService để tạo thông báo cho tập phim ID {EpisodeId}.", episode.Id);
                }
            }

            // Lưu tất cả các thay đổi vào DB
            await dbContext.SaveChangesAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dịch vụ kiểm tra lịch chiếu đang dừng.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}