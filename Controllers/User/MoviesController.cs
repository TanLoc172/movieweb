using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;

namespace MovieWebsite.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager; // Sử dụng AppUser thay vì IdentityUser
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            AppDbContext context,
            IWebHostEnvironment environment,
            ILogger<MoviesController> logger,
            UserManager<AppUser> userManager
        )
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Movie/Index
        public async Task<IActionResult> Index(MovieSearchViewModel searchVM)
        {
            try
            {
                var moviesQuery = _context
                    .Movies.Include(m => m.Country)
                    .Include(m => m.Genre)
                    .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(searchVM.SearchTerm))
                {
                    moviesQuery = moviesQuery.Where(m =>
                        m.Title.Contains(searchVM.SearchTerm)
                        || m.EnglishTitle.Contains(searchVM.SearchTerm)
                    );
                }

                if (searchVM.CountryId.HasValue)
                {
                    moviesQuery = moviesQuery.Where(m => m.CountryId == searchVM.CountryId);
                }

                if (searchVM.GenreId.HasValue)
                {
                    moviesQuery = moviesQuery.Where(m =>
                        m.MovieGenres.Any(mg => mg.GenreId == searchVM.GenreId)
                    );
                }

                if (searchVM.Year.HasValue)
                {
                    moviesQuery = moviesQuery.Where(m => m.ReleaseYear == searchVM.Year);
                }

                // Apply sorting
                switch (searchVM.SortBy?.ToLower())
                {
                    case "oldest":
                        moviesQuery = moviesQuery.OrderBy(m => m.ReleaseYear);
                        break;
                    case "rating":
                        moviesQuery = moviesQuery.OrderByDescending(m => m.AverageRating);
                        break;
                    case "views":
                        moviesQuery = moviesQuery.OrderByDescending(m => m.Views);
                        break;
                    default: // newest
                        moviesQuery = moviesQuery.OrderByDescending(m => m.CreatedAt);
                        break;
                }

                // Pagination
                searchVM.TotalCount = await moviesQuery.CountAsync();
                searchVM.TotalPages = (int)
                    Math.Ceiling((double)searchVM.TotalCount / searchVM.PageSize);
                searchVM.Page = Math.Max(1, Math.Min(searchVM.Page, searchVM.TotalPages));

                searchVM.Movies = await moviesQuery
                    .Skip((searchVM.Page - 1) * searchVM.PageSize)
                    .Take(searchVM.PageSize)
                    .ToListAsync();

                searchVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                searchVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();

                return View(searchVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading movie index");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải danh sách phim";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            var viewModel = new MovieViewModel
            {
                Countries = _context.Countries.OrderBy(c => c.Name).ToList(),
                Genres = _context.Genres.OrderBy(g => g.Name).ToList(),
                Episodes = new List<EpisodeViewModel>(),
            };
            return View(viewModel);
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(524_288_000)]
        public async Task<IActionResult> Create(MovieViewModel movieVM)
        {
            try
            {
                _logger.LogInformation(
                    "Nhận được MovieViewModel: Title={Title}, Episodes={EpisodeCount}, SelectedGenres={GenreCount}, PosterFile={PosterFile}, TrailerFile={TrailerFile}",
                    movieVM.Title,
                    movieVM.Episodes?.Count ?? 0,
                    movieVM.SelectedGenreIds?.Count ?? 0,
                    movieVM.PosterFile?.FileName ?? "null",
                    movieVM.TrailerFile?.FileName ?? "null"
                );

                // Xóa lỗi MovieTitle khỏi ModelState
                foreach (var key in ModelState.Keys.Where(k => k.Contains("MovieTitle")))
                {
                    ModelState.Remove(key);
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    _logger.LogWarning(
                        "Xác thực ModelState thất bại: {Errors}",
                        string.Join(", ", errors)
                    );
                    movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                    movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                    return View(movieVM);
                }

                if (!ValidateUploadedFiles(movieVM, out string errorMessage))
                {
                    _logger.LogWarning("Xác thực file thất bại: {ErrorMessage}", errorMessage);
                    ModelState.AddModelError("", errorMessage);
                    movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                    movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                    return View(movieVM);
                }

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                Directory.CreateDirectory(uploadsFolder);

                var movie = new Movie
                {
                    Title = movieVM.Title,
                    EnglishTitle = movieVM.EnglishTitle,
                    Description = movieVM.Description,
                    ReleaseYear = movieVM.ReleaseYear,
                    CountryId = movieVM.CountryId,
                    GenreId = movieVM.GenreId,
                    Director = movieVM.Director,
                    Cast = movieVM.Cast,
                    TotalEpisodes = movieVM.TotalEpisodes,
                    IsCompleted = movieVM.IsCompleted,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Episodes = new List<Episode>(),
                };

                try
                {
                    movie.PosterPath = await ProcessFileUpload(
                        movieVM.PosterFile,
                        uploadsFolder,
                        "poster"
                    );
                    movie.TrailerPath = await ProcessFileUpload(
                        movieVM.TrailerFile,
                        uploadsFolder,
                        "trailer"
                    );

                    if (movieVM.Episodes != null && movieVM.Episodes.Any())
                    {
                        foreach (var episodeVM in movieVM.Episodes)
                        {
                            _logger.LogInformation(
                                "Xử lý tập phim: EpisodeNumber={EpisodeNumber}, VideoFile={VideoFile}",
                                episodeVM.EpisodeNumber,
                                episodeVM.VideoFile?.FileName ?? "null"
                            );
                            var videoPath = await ProcessFileUpload(
                                episodeVM.VideoFile,
                                uploadsFolder,
                                $"episode_{episodeVM.EpisodeNumber}"
                            );
                            movie.Episodes.Add(
                                new Episode
                                {
                                    EpisodeNumber = episodeVM.EpisodeNumber,
                                    Title = episodeVM.Title,
                                    Description = episodeVM.Description,
                                    Duration = episodeVM.Duration,
                                    ReleaseDate = episodeVM.ReleaseDate,
                                    VideoPath = videoPath,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                }
                            );
                        }
                    }

                    if (movieVM.SelectedGenreIds != null)
                    {
                        movie.MovieGenres = movieVM
                            .SelectedGenreIds.Select(id => new MovieGenre { GenreId = id })
                            .ToList();
                    }

                    _context.Add(movie);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Đã thêm phim: {movie.Title} (ID: {movie.Id})");
                    TempData["SuccessMessage"] = $"Đã thêm phim '{movie.Title}' thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xử lý tải file hoặc lưu vào cơ sở dữ liệu");
                    CleanupUploadedFiles(movie);
                    TempData["ErrorMessage"] = $"Lỗi khi lưu phim: {ex.Message}";
                    movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                    movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                    return View(movieVM);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi thêm phim mới: {Message}, InnerException: {InnerException}",
                    ex.Message,
                    ex.InnerException?.Message
                );
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi thêm phim mới: {ex.Message}";
                movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                return View(movieVM);
            }
        }

        private bool ValidateUploadedFiles(MovieViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validate Poster
            if (model.PosterFile == null || model.PosterFile.Length == 0)
            {
                if (model.Id == 0) // New movie requires poster
                {
                    errorMessage = "Vui lòng chọn file poster";
                    return false;
                }
            }
            else
            {
                var allowedImageTypes = new[]
                {
                    "image/jpeg",
                    "image/png",
                    "image/jpg",
                    "image/gif",
                };
                if (!allowedImageTypes.Contains(model.PosterFile.ContentType.ToLower()))
                {
                    errorMessage = "Chỉ chấp nhận file ảnh (JPEG, JPG, PNG, GIF) cho poster";
                    return false;
                }

                if (model.PosterFile.Length > 5 * 1024 * 1024) // 5MB
                {
                    errorMessage = "Poster không được vượt quá 5MB";
                    return false;
                }
            }

            // Validate Trailer (optional)
            if (model.TrailerFile != null && model.TrailerFile.Length > 0)
            {
                var allowedVideoTypes = new[] { "video/mp4", "video/quicktime", "video/x-msvideo" };
                if (!allowedVideoTypes.Contains(model.TrailerFile.ContentType.ToLower()))
                {
                    errorMessage = "Chỉ chấp nhận file video (MP4, MOV, AVI) cho trailer";
                    return false;
                }

                if (model.TrailerFile.Length > 100 * 1024 * 1024) // 100MB
                {
                    errorMessage = "Trailer không được vượt quá 100MB";
                    return false;
                }
            }

            // Validate Episode Videos (only if episodes are provided)
            if (model.Episodes != null && model.Episodes.Any())
            {
                var allowedVideoTypes = new[] { "video/mp4", "video/quicktime", "video/x-msvideo" };
                foreach (var episode in model.Episodes)
                {
                    if (episode.VideoFile == null || episode.VideoFile.Length == 0)
                    {
                        errorMessage = $"Vui lòng chọn file video cho tập {episode.EpisodeNumber}";
                        return false;
                    }

                    if (!allowedVideoTypes.Contains(episode.VideoFile.ContentType.ToLower()))
                    {
                        errorMessage =
                            $"Chỉ chấp nhận file video (MP4, MOV, AVI) cho tập {episode.EpisodeNumber}";
                        return false;
                    }

                    if (episode.VideoFile.Length > 100 * 1024 * 1024) // 100MB
                    {
                        errorMessage =
                            $"Video tập {episode.EpisodeNumber} không được vượt quá 100MB";
                        return false;
                    }

                    if (string.IsNullOrEmpty(episode.Title))
                    {
                        errorMessage = $"Vui lòng nhập tên cho tập {episode.EpisodeNumber}";
                        return false;
                    }

                    if (episode.Duration < 1 || episode.Duration > 600)
                    {
                        errorMessage =
                            $"Thời lượng tập {episode.EpisodeNumber} phải từ 1 đến 600 phút";
                        return false;
                    }

                    if (episode.ReleaseDate == default)
                    {
                        errorMessage =
                            $"Vui lòng chọn ngày phát hành cho tập {episode.EpisodeNumber}";
                        return false;
                    }
                }

                // Check for duplicate episode numbers
                var episodeNumbers = model.Episodes.Select(e => e.EpisodeNumber).ToList();
                if (episodeNumbers.Distinct().Count() != episodeNumbers.Count)
                {
                    errorMessage = "Số tập phim không được trùng lặp";
                    return false;
                }

                if (model.Episodes.Count > model.TotalEpisodes)
                {
                    errorMessage = "Số lượng tập phim vượt quá tổng số tập đã khai báo";
                    return false;
                }
            }

            return true;
        }

        private async Task<string> ProcessFileUpload(
            IFormFile file,
            string uploadsFolder,
            string fileType
        )
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Không có file được tải lên cho {FileType}", fileType);
                return null;
            }

            string fileExt = Path.GetExtension(file.FileName).ToLower();
            string uniqueFileName = $"{fileType}_{Guid.NewGuid()}{fileExt}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            _logger.LogInformation($"Đang tải file {fileType}: {file.FileName} lên {filePath}");

            try
            {
                // Kiểm tra quyền ghi trước khi lưu
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Xác minh file đã được lưu
                if (!System.IO.File.Exists(filePath))
                {
                    throw new IOException($"File không được lưu tại {filePath}");
                }

                return $"/Uploads/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi tải file {FileName} lên {FilePath}: {Message}",
                    file.FileName,
                    filePath,
                    ex.Message
                );
                throw;
            }
        }

        private void CleanupUploadedFiles(Movie movie)
        {
            try
            {
                if (!string.IsNullOrEmpty(movie.PosterPath))
                {
                    var posterPath = Path.Combine(
                        _environment.WebRootPath,
                        movie.PosterPath.TrimStart('/')
                    );
                    if (System.IO.File.Exists(posterPath))
                        System.IO.File.Delete(posterPath);
                }

                if (!string.IsNullOrEmpty(movie.TrailerPath))
                {
                    var trailerPath = Path.Combine(
                        _environment.WebRootPath,
                        movie.TrailerPath.TrimStart('/')
                    );
                    if (System.IO.File.Exists(trailerPath))
                        System.IO.File.Delete(trailerPath);
                }

                if (movie.Episodes != null)
                {
                    foreach (var episode in movie.Episodes)
                    {
                        if (!string.IsNullOrEmpty(episode.VideoPath))
                        {
                            var videoPath = Path.Combine(
                                _environment.WebRootPath,
                                episode.VideoPath.TrimStart('/')
                            );
                            if (System.IO.File.Exists(videoPath))
                                System.IO.File.Delete(videoPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa file đã tải lên: {Message}", ex.Message);
            }
        }



        [HttpPost]
        [Authorize] // Yêu cầu người dùng phải đăng nhập để thực hiện hành động này
        [ValidateAntiForgeryToken] // Kiểm tra token chống tấn công CSRF
        public async Task<IActionResult> ToggleFavorite(int movieId)
        {
            // 1. Lấy ID của người dùng hiện tại
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                // Trường hợp này hiếm khi xảy ra vì đã có [Authorize]
                // nhưng vẫn nên kiểm tra để đảm bảo an toàn.
                return Json(new { success = false, message = "Người dùng không hợp lệ." });
            }

            try
            {
                // 2. Tìm bản ghi yêu thích trong database
                var existingFavorite = await _context.UserFavoriteMovies
                    .FirstOrDefaultAsync(ufm => ufm.UserId == userId && ufm.MovieId == movieId);

                bool isNowFavorited;

                if (existingFavorite != null)
                {
                    // 3a. Nếu đã tồn tại -> Xóa nó đi (Bỏ yêu thích)
                    _context.UserFavoriteMovies.Remove(existingFavorite);
                    isNowFavorited = false; // Trạng thái mới là "chưa yêu thích"
                }
                else
                {
                    // 3b. Nếu chưa tồn tại -> Tạo mới (Thêm yêu thích)
                    var newFavorite = new UserFavoriteMovie
                    {
                        UserId = userId,
                        MovieId = movieId
                    };
                    await _context.UserFavoriteMovies.AddAsync(newFavorite);
                    isNowFavorited = true; // Trạng thái mới là "đã yêu thích"
                }

                // 4. Lưu các thay đổi vào database
                await _context.SaveChangesAsync();

                // 5. Trả về kết quả JSON thành công cho client
                // JavaScript sẽ dùng thuộc tính 'isFavorited' để cập nhật icon
                return Json(new { success = true, isFavorited = isNowFavorited });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thay đổi trạng thái yêu thích cho phim ID {MovieId} của người dùng {UserId}", movieId, userId);
                // Trả về lỗi nếu có vấn đề xảy ra
                return Json(new { success = false, message = "Đã xảy ra lỗi phía máy chủ." });
            }
        }


        // GET: Movies/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     try
        //     {
        //         if (id == null)
        //         {
        //             _logger.LogWarning("Movie ID is null in Details action");
        //             return NotFound();
        //         }

        //         // Fetch movie with related data, including comments and their replies
        //         var movie = await _context
        //             .Movies
        //             .Include(m => m.Country)
        //             .Include(m => m.Genre)
        //             .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
        //             .Include(m => m.Episodes)
        //             .Include(m => m.Ratings)
        //             // Eagerly load comments and their replies
        //             .Include(m => m.Comments)
        //                 .ThenInclude(c => c.Replies) // Include replies for each comment
        //             .FirstOrDefaultAsync(m => m.Id == id);

        //         if (movie == null)
        //         {
        //             _logger.LogWarning("Movie not found with ID: {Id}", id);
        //             return NotFound();
        //         }

        //         // Increment views
        //         movie.Views++; // Ensure this is uncommented and working
        //         await _context.SaveChangesAsync();


        //         // Get related movies (same genre, excluding current movie)
        //         var relatedMovies = await _context
        //             .Movies
        //             .Include(m => m.Genre) // Include Genre for genre name in related movies list
        //             .Where(m =>
        //                 m.Id != movie.Id
        //                 && (
        //                     m.GenreId == movie.GenreId || // Check primary genre
        //                     m.MovieGenres.Any(mg => mg.GenreId == movie.GenreId) // Check other genres
        //                 )
        //             )
        //             .Take(6) // Limit to 6 related movies
        //             .ToListAsync();

        //         // --- Prepare ViewModel ---
        //         var viewModel = new MovieDetailViewModel
        //         {
        //             Movie = movie,
        //             Episodes = movie.Episodes.OrderBy(e => e.EpisodeNumber).ToList(),
        //             Ratings = movie.Ratings.OrderByDescending(r => r.CreatedAt).ToList(),
        //             // Filter comments to only include top-level ones for initial display
        //             Comments = movie.Comments.Where(c => c.ParentCommentId == null).OrderByDescending(c => c.CreatedAt).ToList(),
        //             RelatedMovies = relatedMovies,
        //             NewRating = new RatingViewModel { MovieId = movie.Id }, // Initialize form models
        //             NewComment = new CommentViewModel { MovieId = movie.Id }, // Initialize form models
        //         };

        //         // --- User Specific Data for Favorites and Form Population ---
        //         string currentUserId = null;
        //         bool isUserLoggedIn = User.Identity != null && User.Identity.IsAuthenticated;
        //         bool hasFavorited = false;

        //         if (isUserLoggedIn)
        //         {
        //             var appUser = await _userManager.GetUserAsync(User);
        //             if (appUser != null)
        //             {
        //                 currentUserId = appUser.Id;

        //                 // Check if the current user has favorited this movie
        //                 hasFavorited = await _context.UserFavoriteMovies.AnyAsync(ufm => ufm.UserId == currentUserId && ufm.MovieId == movie.Id);

        //                 // Pre-populate user details for comment/rating forms if logged in
        //                 viewModel.NewComment.UserName = appUser.FullName ?? appUser.UserName ?? appUser.Email;
        //                 viewModel.NewComment.UserEmail = appUser.Email;
        //                 viewModel.NewRating.UserName = appUser.FullName ?? appUser.UserName ?? appUser.Email;
        //                 viewModel.NewRating.UserEmail = appUser.Email;
        //             }
        //             else
        //             {
        //                 // User is authenticated but GetUserAsync returned null (should not happen usually)
        //                 _logger.LogWarning("User is authenticated but UserManager could not retrieve user details for userId: {UserId}", User.FindFirst("sub")?.Value ?? "N/A");
        //             }
        //         }

        //         // Assign user-specific data to the ViewModel
        //         viewModel.UserId = currentUserId;
        //         viewModel.IsUserLoggedIn = isUserLoggedIn;
        //         viewModel.HasFavorited = hasFavorited;

        //         // The ViewBag.CurrentMovieId is not strictly necessary if you use asp-for correctly with ViewModels
        //         // ViewBag.CurrentMovieId = movie.Id;

        //         return View(viewModel);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error loading movie details for ID: {Id}", id);
        //         TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải chi tiết phim";
        //         // Redirect to the Index action of MoviesController
        //         return RedirectToAction(nameof(Index));
        //     }
        // }


        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                // 1. Kiểm tra ID đầu vào
                if (id == null)
                {
                    _logger.LogWarning("Movie ID is null in Details action");
                    return NotFound();
                }

                // 2. Truy vấn phim và các dữ liệu liên quan (gộp từ cả 2 phiên bản)
                // Bao gồm: Country, Genre, MovieGenres, Episodes, Ratings, Comments và Replies
                var movie = await _context
                    .Movies
                    .Include(m => m.Country)
                    .Include(m => m.Genre)
                    .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                    .Include(m => m.Episodes) // Lấy tất cả các tập để xử lý logic bên dưới
                    .Include(m => m.Ratings)
                    .Include(m => m.Comments)
                        .ThenInclude(c => c.Replies) // Lấy cả các comment trả lời
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (movie == null)
                {
                    _logger.LogWarning("Movie not found with ID: {Id}", id);
                    return NotFound();
                }

                // 3. Tăng lượt xem và lưu thay đổi (thực hiện sớm)
                movie.Views++;
                await _context.SaveChangesAsync();

                // 4. Lọc các tập phim đã được phát hành để hiển thị
                var airedEpisodes = movie.Episodes
                                         .Where(e => e.IsPublished)
                                         .OrderBy(e => e.EpisodeNumber)
                                         .ToList();

                // 5. [Logic từ code 2] Tìm lịch chiếu cho tập tiếp theo
                DateTime? nextScheduleTime = null;
                // Chỉ tìm khi phim chưa hoàn thành và số tập đã phát hành nhỏ hơn tổng số tập
                if (!movie.IsCompleted && airedEpisodes.Count < movie.TotalEpisodes)
                {
                    // Tìm số của tập tiếp theo
                    int nextEpisodeNumber = airedEpisodes.Any() ? airedEpisodes.Max(e => e.EpisodeNumber) + 1 : 1;

                    // Truy vấn bảng Schedule để tìm lịch chiếu cho tập đó
                    var nextSchedule = await _context.Schedules
                        .Include(s => s.Episode)
                        .Where(s => s.MovieId == id && s.Episode.EpisodeNumber == nextEpisodeNumber)
                        .OrderBy(s => s.ScheduledTime) // Lấy lịch sớm nhất
                        .FirstOrDefaultAsync();

                    if (nextSchedule != null)
                    {
                        nextScheduleTime = nextSchedule.ScheduledTime;
                    }
                }

                // 6. Lấy danh sách phim liên quan
                var relatedMovies = await _context
                    .Movies
                    .Include(m => m.Genre)
                    .Where(m =>
                        m.Id != movie.Id
                        && (
                            m.GenreId == movie.GenreId ||
                            m.MovieGenres.Any(mg => mg.GenreId == movie.GenreId)
                        )
                    )
                    .Take(6)
                    .ToListAsync();

                // 7. Khởi tạo ViewModel và điền dữ liệu cơ bản
                var viewModel = new MovieDetailViewModel
                {
                    Movie = movie,
                    // Sử dụng danh sách tập đã lọc
                    Episodes = airedEpisodes,
                    // Thêm lịch chiếu tập tiếp theo
                    NextEpisodeScheduleTime = nextScheduleTime,
                    Ratings = movie.Ratings.OrderByDescending(r => r.CreatedAt).ToList(),
                    // Lọc chỉ lấy các comment gốc (không phải reply)
                    Comments = movie.Comments.Where(c => c.ParentCommentId == null).OrderByDescending(c => c.CreatedAt).ToList(),
                    RelatedMovies = relatedMovies,
                    // Khởi tạo các form model
                    NewRating = new RatingViewModel { MovieId = movie.Id },
                    NewComment = new CommentViewModel { MovieId = movie.Id },
                };

                // 8. [Logic từ code 1] Xử lý dữ liệu dành riêng cho người dùng đang đăng nhập
                string currentUserId = null;
                bool isUserLoggedIn = User.Identity != null && User.Identity.IsAuthenticated;
                bool hasFavorited = false;

                if (isUserLoggedIn)
                {
                    var appUser = await _userManager.GetUserAsync(User);
                    if (appUser != null)
                    {
                        currentUserId = appUser.Id;

                        // Kiểm tra xem người dùng đã thêm phim này vào danh sách yêu thích chưa
                        hasFavorited = await _context.UserFavoriteMovies.AnyAsync(ufm => ufm.UserId == currentUserId && ufm.MovieId == movie.Id);

                        // Điền sẵn thông tin người dùng vào các form
                        viewModel.NewComment.UserName = appUser.FullName ?? appUser.UserName ?? appUser.Email;
                        viewModel.NewComment.UserEmail = appUser.Email;
                        viewModel.NewRating.UserName = appUser.FullName ?? appUser.UserName ?? appUser.Email;
                        viewModel.NewRating.UserEmail = appUser.Email;
                    }
                    else
                    {
                        _logger.LogWarning("User is authenticated but UserManager could not retrieve user details.");
                    }
                }

                // Gán dữ liệu người dùng vào ViewModel
                viewModel.UserId = currentUserId;
                viewModel.IsUserLoggedIn = isUserLoggedIn;
                viewModel.HasFavorited = hasFavorited;

                // Trả về View với ViewModel đã được điền đầy đủ thông tin
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading movie details for ID: {Id}", id);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải chi tiết phim.";
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang chủ nếu có lỗi
            }
        }


        // GET: Movies/Watch/{movieId}/{episodeNumber}
        public async Task<IActionResult> Watch(int movieId, int episodeNumber)
        {
            try
            {
                // Fetch movie with related data
                var movie = await _context
                    .Movies.Include(m => m.Episodes)
                    .Include(m => m.Country)
                    .Include(m => m.Genre)
                    .Include(m => m.Ratings)
                    .Include(m => m.Comments)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie == null)
                {
                    _logger.LogWarning("Movie not found with ID: {MovieId}", movieId);
                    return NotFound();
                }

                // Find the specific episode
                var episode = movie.Episodes.FirstOrDefault(e => e.EpisodeNumber == episodeNumber);
                if (episode == null)
                {
                    _logger.LogWarning(
                        "Episode {EpisodeNumber} not found for movie ID: {MovieId}",
                        episodeNumber,
                        movieId
                    );
                    return NotFound();
                }

                // Increment episode views
                episode.Views++;
                await _context.SaveChangesAsync();

                // Check if the episode is scheduled to air in the future
                var currentDateTime = DateTime.Now; // 02:48 PM +07, June 09, 2025
                var nextEpisode = movie
                    .Episodes.Where(e =>
                        e.EpisodeNumber > episodeNumber && e.ReleaseDate > currentDateTime
                    )
                    .OrderBy(e => e.ReleaseDate)
                    .FirstOrDefault();

                var viewModel = new MovieDetailViewModel
                {
                    Movie = movie,
                    Episodes = movie.Episodes.OrderBy(e => e.EpisodeNumber).ToList(),
                    Ratings = movie.Ratings.ToList(),
                    Comments = movie.Comments.Where(c => c.EpisodeId == episode.Id).ToList(),
                    RelatedMovies = await _context
                        .Movies.Where(m => m.GenreId == movie.GenreId && m.Id != movieId)
                        .Take(4)
                        .ToListAsync(),
                    NewRating = new RatingViewModel { MovieId = movie.Id },
                    NewComment = new CommentViewModel
                    {
                        MovieId = movie.Id,
                        EpisodeId = episode.Id,
                    },
                };

                // Set notification for the next episode if it exists and is airing soon
                if (nextEpisode != null)
                {
                    var timeUntilAir = nextEpisode.ReleaseDate - currentDateTime;
                    if (timeUntilAir.TotalHours <= 24) // Notify if within 24 hours
                    {
                        TempData["EpisodeNotification"] =
                            $"Tập {nextEpisode.EpisodeNumber} sẽ phát sóng vào {nextEpisode.ReleaseDate:dd/MM/yyyy HH:mm}.";
                    }
                }

                ViewData["CurrentEpisode"] = episode;
                // Tương tự trong Watch action, ngay trước 'return View(viewModel);'
                // if (User.Identity.IsAuthenticated)
                // {
                //     var appUser = await _userManager.GetUserAsync(User); // Đảm bảo _userManager đã được inject
                //     if (appUser != null)
                //     {
                //         viewModel.NewComment.UserName =
                //             appUser.FullName ?? appUser.UserName ?? appUser.Email;
                //         viewModel.NewComment.UserEmail = appUser.Email;
                //     }
                // }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading watch page for movie ID: {MovieId}, episode: {EpisodeNumber}",
                    movieId,
                    episodeNumber
                );
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải trang xem phim";
                return RedirectToAction(nameof(Details), new { id = movieId });
            }
        }

        // POST: Movies/AddComment
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> AddComment(CommentViewModel commentVM)
        // {
        //     try
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             var comment = new Comment
        //             {
        //                 MovieId = commentVM.MovieId,
        //                 EpisodeId = commentVM.EpisodeId,
        //                 UserName = commentVM.UserName,
        //                 UserEmail = commentVM.UserEmail,
        //                 Content = commentVM.Content,
        //                 ParentCommentId = commentVM.ParentCommentId,
        //                 CreatedAt = DateTime.Now,
        //                 Likes = 0,
        //                 Dislikes = 0,
        //             };

        //             _context.Comments.Add(comment);
        //             await _context.SaveChangesAsync();

        //             TempData["SuccessMessage"] = "Đã thêm bình luận thành công!";
        //             if (commentVM.EpisodeId.HasValue)
        //             {
        //                 // Redirect to Watch page for episode-specific comments
        //                 var episode = await _context.Episodes.FirstOrDefaultAsync(e =>
        //                     e.Id == commentVM.EpisodeId
        //                 );
        //                 if (episode != null)
        //                 {
        //                     return RedirectToAction(
        //                         nameof(Watch),
        //                         new
        //                         {
        //                             movieId = commentVM.MovieId,
        //                             episodeNumber = episode.EpisodeNumber,
        //                         }
        //                     );
        //                 }
        //             }
        //             // Redirect to Details page for movie-wide comments
        //             return RedirectToAction(nameof(Details), new { id = commentVM.MovieId });
        //         }

        //         TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin bình luận";
        //         if (commentVM.EpisodeId.HasValue)
        //         {
        //             var episode = await _context.Episodes.FirstOrDefaultAsync(e =>
        //                 e.Id == commentVM.EpisodeId
        //             );
        //             if (episode != null)
        //             {
        //                 return RedirectToAction(
        //                     nameof(Watch),
        //                     new
        //                     {
        //                         movieId = commentVM.MovieId,
        //                         episodeNumber = episode.EpisodeNumber,
        //                     }
        //                 );
        //             }
        //         }
        //         return RedirectToAction(nameof(Details), new { id = commentVM.MovieId });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(
        //             ex,
        //             "Error adding comment for movie ID: {MovieId}, episode ID: {EpisodeId}",
        //             commentVM.MovieId,
        //             commentVM.EpisodeId
        //         );
        //         TempData["ErrorMessage"] = "Đã xảy ra lỗi khi thêm bình luận";
        //         if (commentVM.EpisodeId.HasValue)
        //         {
        //             var episode = await _context.Episodes.FirstOrDefaultAsync(e =>
        //                 e.Id == commentVM.EpisodeId
        //             );
        //             if (episode != null)
        //             {
        //                 return RedirectToAction(
        //                     nameof(Watch),
        //                     new
        //                     {
        //                         movieId = commentVM.MovieId,
        //                         episodeNumber = episode.EpisodeNumber,
        //                     }
        //                 );
        //             }
        //         }
        //         return RedirectToAction(nameof(Details), new { id = commentVM.MovieId });
        //     }
        // }

        // POST: Movies/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentViewModel commentVM)
        {
            try
            {
                // Kiểm tra ModelState Hợp Lệ
                if (ModelState.IsValid)
                {
                    string currentUserId = null;
                    string userNameToSave = commentVM.UserName; // Mặc định lấy từ ViewModel (cho khách)
                    string userEmailToSave = commentVM.UserEmail; // Mặc định lấy từ ViewModel (cho khách)

                    // 1. Kiểm tra xem người dùng có đang đăng nhập không
                    if (User.Identity.IsAuthenticated)
                    {
                        // Lấy AppUser đang đăng nhập
                        var appUser = await _userManager.GetUserAsync(User); // Đảm bảo _userManager đã được inject
                        if (appUser != null)
                        {
                            currentUserId = appUser.Id;
                            // Ưu tiên FullName, sau đó đến UserName, cuối cùng là Email
                            userNameToSave = appUser.FullName ?? appUser.UserName ?? appUser.Email;
                            userEmailToSave = appUser.Email;
                        }
                    }
                    else
                    {
                        // 2. Nếu là khách, phải đảm bảo họ đã nhập tên
                        if (string.IsNullOrWhiteSpace(commentVM.UserName))
                        {
                            // Lưu thông báo lỗi vào TempData để hiển thị trên trang web
                            TempData["ErrorMessage"] =
                                "Vui lòng nhập tên của bạn khi bình luận với tư cách khách.";
                            // Chuyển hướng về trang trước đó bằng helper method
                            return RedirectToPreviousPage(commentVM.MovieId, commentVM.EpisodeId);
                        }
                        // userEmailToSave sẽ được lấy từ commentVM.UserEmail (có thể là null hoặc do khách nhập)
                    }

                    // 3. Tạo đối tượng Comment mới với dữ liệu đã xử lý
                    var comment = new Comment
                    {
                        MovieId = commentVM.MovieId,
                        EpisodeId = commentVM.EpisodeId,
                        // UserId = currentUserId, // Lưu UserId nếu có (null nếu là khách)
                        UserName = userNameToSave, // Lưu tên người dùng (từ AppUser hoặc khách nhập)
                        UserEmail = userEmailToSave, // Lưu email (từ AppUser hoặc khách nhập)
                        Content = commentVM.Content,
                        ParentCommentId = commentVM.ParentCommentId, // Có thể là null
                        CreatedAt = DateTime.Now,
                        Likes = 0,
                        Dislikes = 0,
                    };

                    _context.Comments.Add(comment);
                    await _context.SaveChangesAsync(); // Lưu vào DB

                    // Thông báo thành công và chuyển hướng
                    TempData["SuccessMessage"] = "Đã thêm bình luận thành công!";
                    return RedirectToPreviousPage(commentVM.MovieId, commentVM.EpisodeId);
                }
                else
                {
                    // Nếu ModelState không hợp lệ (ví dụ: thiếu nội dung, tên...)
                    // Thông báo lỗi đã được ModelState thêm vào (qua TempData["ErrorMessage"])
                    // và chúng ta cần chuyển hướng để hiển thị nó.
                    TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin bình luận.";
                    return RedirectToPreviousPage(commentVM.MovieId, commentVM.EpisodeId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding comment for movie ID: {MovieId}, episode ID: {EpisodeId}. User is authenticated: {IsAuthenticated}",
                    commentVM.MovieId,
                    commentVM.EpisodeId,
                    User.Identity.IsAuthenticated
                );
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi thêm bình luận.";
                return RedirectToPreviousPage(commentVM.MovieId, commentVM.EpisodeId);
            }
        }

        // Helper method để chuyển hướng về trang trước đó (nếu chưa có thì thêm vào class Controller)
        private IActionResult RedirectToPreviousPage(int movieId, int? episodeId)
        {
            if (episodeId.HasValue)
            {
                // Tìm thông tin tập phim để lấy số tập cho việc redirect
                var episode = _context.Episodes.FirstOrDefault(e => e.Id == episodeId.Value);
                if (episode != null)
                {
                    return RedirectToAction(
                        nameof(Watch),
                        new { movieId = movieId, episodeNumber = episode.EpisodeNumber }
                    );
                }
            }
            // Nếu không phải bình luận cho tập phim, hoặc không tìm thấy tập phim
            return RedirectToAction(nameof(Details), new { id = movieId });
        }

        // // GET: Movie/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var movie = await _context
        //         .Movies.Include(m => m.MovieGenres)
        //         .FirstOrDefaultAsync(m => m.Id == id);

        //     if (movie == null)
        //     {
        //         return NotFound();
        //     }

        //     var viewModel = new MovieViewModel
        //     {
        //         Id = movie.Id,
        //         Title = movie.Title,
        //         EnglishTitle = movie.EnglishTitle,
        //         Description = movie.Description,
        //         ReleaseYear = movie.ReleaseYear,
        //         CountryId = movie.CountryId,
        //         GenreId = movie.GenreId,
        //         Director = movie.Director,
        //         Cast = movie.Cast,
        //         TotalEpisodes = movie.TotalEpisodes,
        //         IsCompleted = movie.IsCompleted,
        //         PosterPath = movie.PosterPath,
        //         TrailerPath = movie.TrailerPath,
        //         SelectedGenreIds = movie.MovieGenres.Select(mg => mg.GenreId).ToList(),
        //         Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync(),
        //         Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync(),
        //     };

        //     return View(viewModel);
        // }

        // // POST: Movie/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [RequestSizeLimit(524_288_000)] // 500MB
        // public async Task<IActionResult> Edit(int id, MovieViewModel movieVM)
        // {
        //     if (id != movieVM.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (!ModelState.IsValid)
        //     {
        //         movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        //         movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        //         return View(movieVM);
        //     }

        //     try
        //     {
        //         var movie = await _context
        //             .Movies.Include(m => m.MovieGenres)
        //             .FirstOrDefaultAsync(m => m.Id == id);

        //         if (movie == null)
        //         {
        //             return NotFound();
        //         }

        //         movie.Title = movieVM.Title;
        //         movie.EnglishTitle = movieVM.EnglishTitle;
        //         movie.Description = movieVM.Description;
        //         movie.ReleaseYear = movieVM.ReleaseYear;
        //         movie.CountryId = movieVM.CountryId;
        //         movie.GenreId = movieVM.GenreId;
        //         movie.Director = movieVM.Director;
        //         movie.Cast = movieVM.Cast;
        //         movie.TotalEpisodes = movieVM.TotalEpisodes;
        //         movie.IsCompleted = movieVM.IsCompleted;
        //         movie.UpdatedAt = DateTime.Now;

        //         string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");

        //         // Upload new Poster if provided
        //         if (movieVM.PosterFile != null && movieVM.PosterFile.Length > 0)
        //         {
        //             if (!string.IsNullOrEmpty(movie.PosterPath))
        //             {
        //                 var oldFilePath = Path.Combine(
        //                     _environment.WebRootPath,
        //                     movie.PosterPath.TrimStart('/')
        //                 );
        //                 if (System.IO.File.Exists(oldFilePath))
        //                 {
        //                     System.IO.File.Delete(oldFilePath);
        //                 }
        //             }
        //             movie.PosterPath = await ProcessFileUpload(
        //                 movieVM.PosterFile,
        //                 uploadsFolder,
        //                 "poster"
        //             );
        //         }

        //         // Upload new Trailer if provided
        //         if (movieVM.TrailerFile != null && movieVM.TrailerFile.Length > 0)
        //         {
        //             if (!string.IsNullOrEmpty(movie.TrailerPath))
        //             {
        //                 var oldFilePath = Path.Combine(
        //                     _environment.WebRootPath,
        //                     movie.TrailerPath.TrimStart('/')
        //                 );
        //                 if (System.IO.File.Exists(oldFilePath))
        //                 {
        //                     System.IO.File.Delete(oldFilePath);
        //                 }
        //             }
        //             movie.TrailerPath = await ProcessFileUpload(
        //                 movieVM.TrailerFile,
        //                 uploadsFolder,
        //                 "trailer"
        //             );
        //         }

        //         // Update genres
        //         movie.MovieGenres.Clear();
        //         if (movieVM.SelectedGenreIds != null)
        //         {
        //             movie.MovieGenres = movieVM
        //                 .SelectedGenreIds.Select(id => new MovieGenre
        //                 {
        //                     MovieId = movie.Id,
        //                     GenreId = id,
        //                 })
        //                 .ToList();
        //         }

        //         _context.Update(movie);
        //         await _context.SaveChangesAsync();

        //         _logger.LogInformation($"Updated movie: {movie.Title} (ID: {movie.Id})");
        //         TempData["SuccessMessage"] = $"Đã cập nhật phim '{movie.Title}' thành công!";
        //         return RedirectToAction(nameof(Index));
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!MovieExists(id))
        //         {
        //             return NotFound();
        //         }
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, $"Error updating movie ID: {id}");
        //         TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi cập nhật phim: {ex.Message}";
        //         movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        //         movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        //         return View(movieVM);
        //     }
        // }

        // GET: Movies/Korean
        public async Task<IActionResult> HQ(int countryId = 2) // Giả định ID của Hàn Quốc là 1
        {
            try
            {
                // Tìm quốc gia theo ID (ở đây giả định ID của Hàn Quốc là 1)
                var koreanCountry = await _context.Countries.FindAsync(countryId);

                if (koreanCountry == null)
                {
                    _logger.LogWarning("Quốc gia với ID {CountryId} không tồn tại.", countryId);
                    TempData["ErrorMessage"] = "Quốc gia được yêu cầu không tồn tại.";
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
                }

                // Lọc các bộ phim thuộc quốc gia Hàn Quốc
                // Bao gồm các thông tin cần thiết cho danh sách phim
                var movies = await _context
                    .Movies.Include(m => m.Country) // Bao gồm thông tin quốc gia để hiển thị tên quốc gia
                    .Include(m => m.Genre) // Bao gồm thông tin thể loại chính để hiển thị
                    .Where(m => m.CountryId == countryId)
                    .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo ngày tạo mới nhất, bạn có thể thay đổi
                    .ToListAsync();

                // Tạo ViewModel cho view
                var viewModel = new MoviesByCountryViewModel
                {
                    SelectedCountry = koreanCountry,
                    Movies = movies,
                };

                _logger.LogInformation(
                    "Đã lấy danh sách {MovieCount} phim của quốc gia {CountryName}.",
                    movies.Count,
                    koreanCountry.Name
                );

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi lấy danh sách phim theo quốc gia {CountryId}",
                    countryId
                );
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tải danh sách phim của {countryId}.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> TQ(int countryId = 3) // Giả định ID của Hàn Quốc là 1
        {
            try
            {
                // Tìm quốc gia theo ID (ở đây giả định ID của Hàn Quốc là 1)
                var koreanCountry = await _context.Countries.FindAsync(countryId);

                if (koreanCountry == null)
                {
                    _logger.LogWarning("Quốc gia với ID {CountryId} không tồn tại.", countryId);
                    TempData["ErrorMessage"] = "Quốc gia được yêu cầu không tồn tại.";
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
                }

                // Lọc các bộ phim thuộc quốc gia Hàn Quốc
                // Bao gồm các thông tin cần thiết cho danh sách phim
                var movies = await _context
                    .Movies.Include(m => m.Country) // Bao gồm thông tin quốc gia để hiển thị tên quốc gia
                    .Include(m => m.Genre) // Bao gồm thông tin thể loại chính để hiển thị
                    .Where(m => m.CountryId == countryId)
                    .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo ngày tạo mới nhất, bạn có thể thay đổi
                    .ToListAsync();

                // Tạo ViewModel cho view
                var viewModel = new MoviesByCountryViewModel
                {
                    SelectedCountry = koreanCountry,
                    Movies = movies,
                };

                _logger.LogInformation(
                    "Đã lấy danh sách {MovieCount} phim của quốc gia {CountryName}.",
                    movies.Count,
                    koreanCountry.Name
                );

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi lấy danh sách phim theo quốc gia {CountryId}",
                    countryId
                );
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tải danh sách phim của {countryId}.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> US(int countryId = 5) // Giả định ID của Hàn Quốc là 1
        {
            try
            {
                // Tìm quốc gia theo ID (ở đây giả định ID của Hàn Quốc là 1)
                var koreanCountry = await _context.Countries.FindAsync(countryId);

                if (koreanCountry == null)
                {
                    _logger.LogWarning("Quốc gia với ID {CountryId} không tồn tại.", countryId);
                    TempData["ErrorMessage"] = "Quốc gia được yêu cầu không tồn tại.";
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
                }

                // Lọc các bộ phim thuộc quốc gia Hàn Quốc
                // Bao gồm các thông tin cần thiết cho danh sách phim
                var movies = await _context
                    .Movies.Include(m => m.Country) // Bao gồm thông tin quốc gia để hiển thị tên quốc gia
                    .Include(m => m.Genre) // Bao gồm thông tin thể loại chính để hiển thị
                    .Where(m => m.CountryId == countryId)
                    .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo ngày tạo mới nhất, bạn có thể thay đổi
                    .ToListAsync();

                // Tạo ViewModel cho view
                var viewModel = new MoviesByCountryViewModel
                {
                    SelectedCountry = koreanCountry,
                    Movies = movies,
                };

                _logger.LogInformation(
                    "Đã lấy danh sách {MovieCount} phim của quốc gia {CountryName}.",
                    movies.Count,
                    koreanCountry.Name
                );

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi lấy danh sách phim theo quốc gia {CountryId}",
                    countryId
                );
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tải danh sách phim của {countryId}.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("Movies/[action]/{countryId?}")]
        public async Task<IActionResult> Quocgia(int countryId) // Giả định ID của Hàn Quốc là 1
        {
            try
            {
                // Tìm quốc gia theo ID (ở đây giả định ID của Hàn Quốc là 1)
                var koreanCountry = await _context.Countries.FindAsync(countryId);

                if (koreanCountry == null)
                {
                    _logger.LogWarning("Quốc gia với ID {CountryId} không tồn tại.", countryId);
                    TempData["ErrorMessage"] = "Quốc gia được yêu cầu không tồn tại.";
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
                }

                // Lọc các bộ phim thuộc quốc gia Hàn Quốc
                // Bao gồm các thông tin cần thiết cho danh sách phim
                var movies = await _context
                    .Movies.Include(m => m.Country) // Bao gồm thông tin quốc gia để hiển thị tên quốc gia
                    .Include(m => m.Genre) // Bao gồm thông tin thể loại chính để hiển thị
                    .Where(m => m.CountryId == countryId)
                    .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo ngày tạo mới nhất, bạn có thể thay đổi
                    .ToListAsync();

                // Tạo ViewModel cho view
                var viewModel = new MoviesByCountryViewModel
                {
                    SelectedCountry = koreanCountry,
                    Movies = movies,
                };

                _logger.LogInformation(
                    "Đã lấy danh sách {MovieCount} phim của quốc gia {CountryName}.",
                    movies.Count,
                    koreanCountry.Name
                );

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi lấy danh sách phim theo quốc gia {CountryId}",
                    countryId
                );
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tải danh sách phim của {countryId}.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        // Phương thức mới để lấy danh sách phim theo thể loại
        [Route("Movies/[action]/{genreId?}")]
        public async Task<IActionResult> MoviesByGenre(int genreId)
        {
            try
            {
                // Tìm thể loại theo ID
                var selectedGenre = await _context.Genres.FindAsync(genreId);

                if (selectedGenre == null)
                {
                    _logger.LogWarning("Thể loại với ID {GenreId} không tồn tại.", genreId);
                    TempData["ErrorMessage"] = "Thể loại được yêu cầu không tồn tại.";
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
                }

                // Lọc các bộ phim thuộc thể loại này sử dụng bảng trung gian MovieGenres
                var moviesWithGenre = await _context
                    .Movies.Include(m => m.Country) // Bao gồm thông tin quốc gia để hiển thị tên quốc gia
                                                    // Nếu bạn muốn hiển thị TÊN của thể loại chính của phim (nếu có trường GenreId trong Movie),
                                                    // bạn có thể giữ lại .Include(m => m.Genre).
                                                    // Tuy nhiên, để hiển thị tất cả các thể loại của phim trong danh sách, bạn cần Include MovieGenres
                    .Include(m => m.MovieGenres) // Bao gồm bảng trung gian
                    .ThenInclude(mg => mg.Genre) // Từ bảng trung gian, bao gồm thông tin của Genre
                    .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId)) // Lọc phim có ít nhất 1 liên kết với genreId này
                    .OrderByDescending(m => m.CreatedAt) // Sắp xếp theo ngày tạo mới nhất
                    .ToListAsync();

                // Tạo ViewModel cho view
                var viewModel = new MoviesByGenreViewModel
                {
                    SelectedGenre = selectedGenre,
                    Movies = moviesWithGenre,
                };

                _logger.LogInformation(
                    "Đã lấy danh sách {MovieCount} phim của thể loại {GenreName}.",
                    moviesWithGenre.Count,
                    selectedGenre.Name
                );

                // Trả về view tương ứng, ví dụ: Views/Movie/MoviesByGenre.cshtml
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim theo thể loại {GenreId}", genreId);
                TempData["ErrorMessage"] =
                    $"Đã xảy ra lỗi khi tải danh sách phim của thể loại {genreId}.";
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách phim chính
            }
        }


        /// Lấy danh sách các phim bộ (có nhiều hơn 1 tập).
        /// </summary>
        [Route("Movies/Series")]
        public async Task<IActionResult> SeriesMovies()
        {
            try
            {
                var seriesMovies = await _context
                    .Movies.Include(m => m.Country)
                    .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                    .Where(m => m.TotalEpisodes > 1)
                    .OrderByDescending(m => m.UpdatedAt)
                    .ToListAsync();

                // Tạo và gán dữ liệu cho ViewModel
                var viewModel = new SeriesMoviesViewModel
                {
                    PageTitle = "Phim Bộ Mới Cập Nhật",
                    PageDescription = "Khám phá các bộ phim truyền hình, phim dài tập mới nhất và hấp dẫn nhất.",
                    Movies = seriesMovies
                };

                _logger.LogInformation("Đã lấy thành công {MovieCount} phim bộ.", seriesMovies.Count);

                // Trả về view với ViewModel
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim bộ.");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải danh sách phim bộ.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Lấy danh sách các phim lẻ (chỉ có 1 tập).
        /// </summary>
        [Route("Movies/Feature")]
        public async Task<IActionResult> FeatureMovies()
        {
            try
            {
                var featureMovies = await _context
                    .Movies.Include(m => m.Country)
                    .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                    .Where(m => m.TotalEpisodes == 1)
                    .OrderByDescending(m => m.UpdatedAt)
                    .ToListAsync();

                // Tạo và gán dữ liệu cho ViewModel
                var viewModel = new FeatureMoviesViewModel
                {
                    PageTitle = "Phim Lẻ Đặc Sắc",
                    PageDescription = "Tuyển tập những bộ phim lẻ, phim chiếu rạp đặc sắc từ nhiều quốc gia.",
                    Movies = featureMovies
                };

                _logger.LogInformation("Đã lấy thành công {MovieCount} phim lẻ.", featureMovies.Count);

                // Trả về view với ViewModel
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim lẻ.");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải danh sách phim lẻ.";
                return RedirectToAction(nameof(Index));
            }
        }




        [Route("Movies/TopRankedSeries")]
        public async Task<IActionResult> TopRankedSeries() // Đổi tên để rõ nghĩa hơn
        {
            try
            {
                // Phân tích từ hình ảnh: "Top 10 phim bộ hôm nay"
                // 1. "Top 10": Lấy 10 phim
                // 2. "phim bộ": Lọc những phim có TotalEpisodes > 1
                // 3. "hôm nay": Thuộc tính `Views` của bạn là tổng lượt xem. Để có "lượt xem hôm nay"
                //    cần một cấu trúc DB phức tạp hơn (VD: bảng ViewLogs).
                //    Ở đây, chúng ta sẽ tạm dùng TỔNG LƯỢT XEM để xếp hạng.

                var topMoviesQuery = await _context.Movies
                    .AsNoTracking()
                    .Include(m => m.Episodes) // Cần include Episodes để đếm số tập đã chiếu
                    .Where(m => m.TotalEpisodes > 1) // Chỉ lấy phim bộ
                    .OrderByDescending(m => m.Views) // Sắp xếp theo tổng lượt xem
                    .Take(10) // Lấy 10 phim hàng đầu
                    .ToListAsync();

                var rankedMovies = new List<RankedMovieViewModel>();
                int rank = 1;
                foreach (var movie in topMoviesQuery)
                {
                    rankedMovies.Add(new RankedMovieViewModel
                    {
                        Rank = rank,
                        Movie = movie,
                        // Đếm số tập đã thực sự phát hành (có ngày phát hành trong quá khứ)
                        AiredEpisodesCount = movie.Episodes.Count(e => e.ReleaseDate <= DateTime.Now && e.IsPublished)
                    });
                    rank++;
                }

                var viewModel = new TopRankedSectionViewModel
                {
                    SectionTitle = "Top 10 phim bộ hôm nay",
                    RankedMovies = rankedMovies
                };

                // Nếu bạn muốn dùng nó như một trang riêng:
                // return View("TopRankedSeries", viewModel);

                // Nếu bạn muốn dùng nó như một thành phần (component) trên trang chủ,
                // trả về PartialView sẽ tốt hơn.
                return PartialView("TopRankedSeries", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim xếp hạng.");
                // Trả về một partial view rỗng hoặc thông báo lỗi
                return PartialView("TopRankedSeries", new TopRankedSectionViewModel());
            }
        }


        // // Giả định bạn có trang Index để hiển thị tất cả phim hoặc chuyển hướng
        // public async Task<IActionResult> Index()
        // {
        //     // Ví dụ: Lấy tất cả phim và tất cả thể loại để hiển thị trên trang Index
        //     var allMovies = await _context
        //         .Movies.Include(m => m.Country)
        //         .Include(m => m.MovieGenres)
        //         .ThenInclude(mg => mg.Genre)
        //         .ToListAsync();
        //     var allGenres = await _context.Genres.ToListAsync();

        //     // Bạn có thể cần một ViewModel khác cho trang Index, hoặc chỉ đơn giản là trả về danh sách phim
        //     return View(allMovies); // Giả sử View Index nhận List<Movie>
        // }
    }
}
