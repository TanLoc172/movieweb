// File: ~/ViewComponents/BannerViewComponent.cs

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data; // Thay bằng namespace chứa AppDbContext của bạn
using MovieWebsite.Models; // Thay bằng namespace chứa các ViewModel của bạn

namespace MovieWebsite.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        // 1. Sử dụng Dependency Injection để lấy DbContext, giống như trong Controller
        public BannerViewComponent(AppDbContext context)
        {
            _context = context;
        }

        // 2. Phương thức InvokeAsync sẽ được gọi khi bạn dùng @await Component.InvokeAsync("Banner")
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 3. Sao chép và điều chỉnh logic query từ HomeController vào đây
            //    Sử dụng ToListAsync() để thực hiện query bất đồng bộ
            var moviesForBanners = await _context
                .Movies.AsNoTracking() // Thêm AsNoTracking() để tăng hiệu suất vì đây là dữ liệu chỉ đọc
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                // .Include(m => m.Genre) // Không cần thiết nếu bạn đã lấy từ MovieGenres
                .OrderByDescending(m => m.ReleaseYear)
                .ThenByDescending(m => m.CreatedAt)
                .Take(5)
                .Select(m => new MovieBannerViewModel // Tạo ViewModel trực tiếp từ query
                {
                    MovieId = m.Id,
                    PosterBanner = m.PosterBanner,
                    Title = m.Title,
                    EnglishTitle = m.EnglishTitle, // Đảm bảo bạn có trường này trong model Movie
                    Description = m.Description,
                    ReleaseYear = m.ReleaseYear,
                    TotalEpisodes = m.TotalEpisodes,
                    TrailerPath = m.TrailerPath,
                    // Lấy genre chính (đầu tiên) và các genre khác
                    MainGenreName =
                        m.MovieGenres.Select(mg => mg.Genre.Name).FirstOrDefault() ?? "N/A",
                    GenreNames = m.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                })
                .ToListAsync();

            // 4. Tạo một HomeViewModel và chỉ điền dữ liệu mà banner cần
            var viewModel = new HomeViewModel { BannerMovies = moviesForBanners };

            // 5. Trả về view mặc định của component (Default.cshtml) cùng với dữ liệu đã chuẩn bị
            //    View này vẫn sẽ dùng @model HomeViewModel
            return View(viewModel);
        }
    }
}
