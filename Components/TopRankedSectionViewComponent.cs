using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data; // sửa theo namespace thực tế của bạn
using MovieWebsite.Models;


namespace MovieWebsite.ViewComponents
{
    public class TopRankedSeriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TopRankedSeriesViewComponent> _logger;

        public TopRankedSeriesViewComponent(AppDbContext context, ILogger<TopRankedSeriesViewComponent> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var topMoviesQuery = await _context.Movies
                    .AsNoTracking()
                    .Include(m => m.Episodes)
                    .Where(m => m.TotalEpisodes > 1)
                    .OrderByDescending(m => m.Views)
                    .Take(12)
                    .ToListAsync();

                var rankedMovies = new List<RankedMovieViewModel>();
                int rank = 1;
                foreach (var movie in topMoviesQuery)
                {
                    rankedMovies.Add(new RankedMovieViewModel
                    {
                        Rank = rank++,
                        Movie = movie,
                        AiredEpisodesCount = movie.Episodes.Count(e => e.ReleaseDate <= DateTime.Now && e.IsPublished)
                    });
                }

                var viewModel = new TopRankedSectionViewModel
                {
                    SectionTitle = "Top 10 phim bộ hôm nay",
                    RankedMovies = rankedMovies
                };

                return View(viewModel); // -> /Views/Shared/Components/TopRankedSeries/Default.cshtml
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim xếp hạng.");
                return View(new TopRankedSectionViewModel());
            }
        }
    }
}
