// Tạo file: Services/EpisodeReleaseService.cs

using Microsoft.AspNetCore.SignalR;
using MovieWebsite.Hubs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Hubs;

public class EpisodeReleaseService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<EpisodeReleaseService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EpisodeReleaseService(ILogger<EpisodeReleaseService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dịch vụ kiểm tra lịch chiếu đang khởi động.");
        // Kiểm tra mỗi 30 giây. Bạn có thể điều chỉnh khoảng thời gian này.
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3000000));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        _logger.LogInformation("Dịch vụ đang kiểm tra các tập phim mới.");

        // Chúng ta cần một service scope để resolve các service có đời sống scoped như DbContext
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ScheduleHub>>();

            var now = DateTime.UtcNow;

            // Tìm các lịch chiếu vừa đến hạn và có tập phim chưa được đánh dấu là đã publish
            // (Sử dụng cờ IsPublished ở đây như một biện pháp an toàn để tránh gửi nhiều thông báo)
            var newlyReleasedSchedules = await dbContext.Schedules
                .Include(s => s.Episode)
                .Where(s => s.Episode != null && !s.Episode.IsPublished && s.ScheduledTime <= now)
                .ToListAsync();

            if (!newlyReleasedSchedules.Any()) return;

            foreach (var schedule in newlyReleasedSchedules)
            {
                var episode = schedule.Episode;
                episode.IsPublished = true; // Đánh dấu là đã publish để không gửi thông báo lại

                _logger.LogInformation($"Tập '{episode.Title}' (ID: {episode.Id}) của Phim ID {episode.MovieId} đã ra mắt!");

                // Thông báo cho tất cả các client đang kết nối về tập phim mới
                await hubContext.Clients.All.SendAsync("NewEpisodeReleased", new
                {
                    movieId = episode.MovieId,
                    episodeId = episode.Id,
                    episodeNumber = episode.EpisodeNumber,
                    title = episode.Title
                });
            }

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