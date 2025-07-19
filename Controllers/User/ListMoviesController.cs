using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data; // <-- THAY THẾ bằng namespace của DbContext của bạn
using MovieWebsite.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWebsite.Controllers
{
    // Thay đổi 1: Đổi tên class từ MoviesController thành ListMoviesController
    public class ListMoviesController : Controller
    {
        private readonly AppDbContext _context;

        public ListMoviesController(AppDbContext context)
        {
            _context = context;
        }

        // Action này sẽ được truy cập qua URL: /ListMovies hoặc /ListMovies/Index
        public async Task<IActionResult> List()
        {
            var movies = await _context.Movies
                                       .Include(m => m.Country)
                                       .Include(m => m.Genre)
                                       .OrderByDescending(m => m.UpdatedAt)
                                       .ToListAsync();

            return View(movies);
        }

        // Action này sẽ được truy cập qua URL: /ListMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Genre)
                .Include(m => m.Episodes.OrderBy(e => e.EpisodeNumber))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}