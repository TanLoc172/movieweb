// File: Services/NotificationService.cs
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Interfaces;
using MovieWebsite.Models;

namespace MovieWebsite.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNewEpisodeNotificationAsync(Episode newEpisode)
        {
            // 1. Lấy thông tin phim của tập vừa thêm, bao gồm cả tiêu đề
            var movie = await _context.Movies
                .AsNoTracking() // Không cần theo dõi thay đổi
                .FirstOrDefaultAsync(m => m.Id == newEpisode.MovieId);

            if (movie == null) return; // Không tìm thấy phim, không làm gì cả

            // 2. Tìm tất cả User ID đã yêu thích bộ phim này
            var userIdsWhoFavorited = await _context.UserFavoriteMovies
                .Where(ufm => ufm.MovieId == movie.Id)
                .Select(ufm => ufm.UserId)
                .ToListAsync();

            if (!userIdsWhoFavorited.Any()) return; // Không có ai yêu thích, không cần gửi

            // 3. Tạo danh sách thông báo mới
            var notifications = new List<Notification>();
            string message = $"'{movie.Title}' vừa có tập mới: Tập {newEpisode.EpisodeNumber} - {newEpisode.Title}.";
            // URL sẽ trỏ đến trang xem phim của tập mới
            string url = $"/Movies/Watch?movieId={movie.Id}&episodeNumber={newEpisode.EpisodeNumber}";

            foreach (var userId in userIdsWhoFavorited)
            {
                notifications.Add(new Notification
                {
                    UserId = userId,
                    MovieId = movie.Id,
                    EpisodeId = newEpisode.Id,
                    Message = message,
                    Url = url,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });
            }

            // 4. Thêm tất cả thông báo vào database và lưu lại
            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();
        }
    }
}