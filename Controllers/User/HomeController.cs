using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;

public class HomeController : Controller
{
    private readonly AppDbContext _context; // Thay thế YourDbContext bằng tên DbContext của bạn

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var moviesForBanners = _context
            .Movies.Include(m => m.Genre)
            .Include(m => m.Country)
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .OrderByDescending(m => m.ReleaseYear)
            .ThenByDescending(m => m.CreatedAt)
            .Take(5)
            .Select(m => new MovieBannerViewModel
            {
                MovieId = m.Id,
                PosterBanner = m.PosterBanner,
                Title = m.Title,
                Description = m.Description,
                ReleaseYear = m.ReleaseYear,
                TotalEpisodes = m.TotalEpisodes,
                TrailerPath = m.TrailerPath, // Controller cung cấp đường dẫn trailer
                MainGenreName = m.Genre != null ? m.Genre.Name : "N/A",
                CountryName = m.Country != null ? m.Country.Name : "N/A",
                GenreNames = m
                    .MovieGenres.Select(mg => mg.Genre != null ? mg.Genre.Name : "N/A")
                    .ToList(),
            })
            .ToList();

        var homeViewModel = new HomeViewModel
        {
            BannerMovies = moviesForBanners,
            PopularGenres = _context
                .Genres.OrderByDescending(g => g.Movies.Count())
                .Take(10)
                .ToList(),
            LatestMovies = _context.Movies.OrderByDescending(m => m.CreatedAt).Take(8).ToList(),
        };

        return View(homeViewModel);
    }

    [HttpGet]
    public IActionResult TopMoviesByViews()
    {
        // Fetch all movies from the context
        // We need to include related data if your view needs it,
        // but for just sorting by views, it's not strictly necessary unless you display them.
        // For now, we assume the view only needs basic Movie properties and Poster/Title.
        var allMovies = _context.Movies.ToList();

        // Create the list of MovieWithViewCount objects
        var moviesWithViews = allMovies
            .Select(movie => new MovieWithViewCount
            {
                Movie = movie,
                ViewCount = movie.Views, // Directly access the Views property from your Movie model
            })
            .ToList();

        // Sort by ViewCount in descending order and take the top 10
        var top10Movies = moviesWithViews.OrderByDescending(vm => vm.ViewCount).Take(5).ToList();

        // Prepare the ViewModel
        var viewModel = new TopMoviesByViewsViewModel { Movies = top10Movies };

        // Return the view with the ViewModel
        return View(viewModel);
    }
}
