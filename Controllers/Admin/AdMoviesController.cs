using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWebsite.Areas.Admin.Controllers
{
    // [Area("Admin")]
    public class AdMoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AdMoviesController> _logger;

        // Tái sử dụng các phương thức xử lý file upload

        
        private async Task<string> ProcessFileUpload1(IFormFile file, string fileType)
        {
            if (file == null || file.Length == 0) return null;

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{fileType}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}"; // Trả về đường dẫn tương đối
        }

        private void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            // Chuyển đổi đường dẫn tương đối thành đường dẫn vật lý
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }


        public AdMoviesController(AppDbContext context, IWebHostEnvironment environment, ILogger<AdMoviesController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        // GET: Admin/Movies
        public async Task<IActionResult> ListMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Country)
                //.Include(m => m.Genre) 
                .Include(m => m.Episodes)
                .AsSplitQuery()
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Index(MovieSearchViewModel searchVM)
        {
            try
            {
                var moviesQuery = _context.Movies
                    .Include(m => m.Country)
                    .Include(m => m.Genre)
                    .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(searchVM.SearchTerm))
                {
                    moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchVM.SearchTerm) ||
                                                       m.EnglishTitle.Contains(searchVM.SearchTerm));
                }

                if (searchVM.CountryId.HasValue)
                {
                    moviesQuery = moviesQuery.Where(m => m.CountryId == searchVM.CountryId);
                }

                if (searchVM.GenreId.HasValue)
                {
                    moviesQuery = moviesQuery.Where(m => m.MovieGenres.Any(mg => mg.GenreId == searchVM.GenreId));
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
                searchVM.TotalPages = (int)Math.Ceiling((double)searchVM.TotalCount / searchVM.PageSize);
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
                Episodes = new List<EpisodeViewModel>()
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
                _logger.LogInformation("Nhận được MovieViewModel: Title={Title}, Episodes={EpisodeCount}, SelectedGenres={GenreCount}, PosterFile={PosterFile}, TrailerFile={TrailerFile}",
                    movieVM.Title, movieVM.Episodes?.Count ?? 0, movieVM.SelectedGenreIds?.Count ?? 0,
                    movieVM.PosterFile?.FileName ?? "null", movieVM.TrailerFile?.FileName ?? "null");

                // Xóa lỗi MovieTitle khỏi ModelState
                foreach (var key in ModelState.Keys.Where(k => k.Contains("MovieTitle")))
                {
                    ModelState.Remove(key);
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("Xác thực ModelState thất bại: {Errors}", string.Join(", ", errors));
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
                    Episodes = new List<Episode>()
                };

                try
                {
                    movie.PosterPath = await ProcessFileUpload(movieVM.PosterFile, uploadsFolder, "poster");
                    movie.TrailerPath = await ProcessFileUpload(movieVM.TrailerFile, uploadsFolder, "trailer");

                    if (movieVM.Episodes != null && movieVM.Episodes.Any())
                    {
                        foreach (var episodeVM in movieVM.Episodes)
                        {
                            _logger.LogInformation("Xử lý tập phim: EpisodeNumber={EpisodeNumber}, VideoFile={VideoFile}",
                                episodeVM.EpisodeNumber, episodeVM.VideoFile?.FileName ?? "null");
                            var videoPath = await ProcessFileUpload(episodeVM.VideoFile, uploadsFolder, $"episode_{episodeVM.EpisodeNumber}");
                            movie.Episodes.Add(new Episode
                            {
                                EpisodeNumber = episodeVM.EpisodeNumber,
                                Title = episodeVM.Title,
                                Description = episodeVM.Description,
                                Duration = episodeVM.Duration,
                                ReleaseDate = episodeVM.ReleaseDate,
                                VideoPath = videoPath,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            });
                        }
                    }

                    if (movieVM.SelectedGenreIds != null)
                    {
                        movie.MovieGenres = movieVM.SelectedGenreIds
                            .Select(id => new MovieGenre { GenreId = id })
                            .ToList();
                    }

                    _context.Add(movie);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Đã thêm phim: {movie.Title} (ID: {movie.Id})");
                    TempData["SuccessMessage"] = $"Đã thêm phim '{movie.Title}' thành công!";
                    // return RedirectToAction(nameof(Index));
                    return RedirectToAction(nameof(ListMovies));

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
                _logger.LogError(ex, "Lỗi khi thêm phim mới: {Message}, InnerException: {InnerException}", ex.Message, ex.InnerException?.Message);
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
                var allowedImageTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif" };
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
                        errorMessage = $"Chỉ chấp nhận file video (MP4, MOV, AVI) cho tập {episode.EpisodeNumber}";
                        return false;
                    }

                    if (episode.VideoFile.Length > 100 * 1024 * 1024) // 100MB
                    {
                        errorMessage = $"Video tập {episode.EpisodeNumber} không được vượt quá 100MB";
                        return false;
                    }

                    if (string.IsNullOrEmpty(episode.Title))
                    {
                        errorMessage = $"Vui lòng nhập tên cho tập {episode.EpisodeNumber}";
                        return false;
                    }

                    if (episode.Duration < 1 || episode.Duration > 600)
                    {
                        errorMessage = $"Thời lượng tập {episode.EpisodeNumber} phải từ 1 đến 600 phút";
                        return false;
                    }

                    if (episode.ReleaseDate == default)
                    {
                        errorMessage = $"Vui lòng chọn ngày phát hành cho tập {episode.EpisodeNumber}";
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

        private async Task<string> ProcessFileUpload(IFormFile file, string uploadsFolder, string fileType)
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
                _logger.LogError(ex, "Lỗi khi tải file {FileName} lên {FilePath}: {Message}", file.FileName, filePath, ex.Message);
                throw;
            }
        }

        private void CleanupUploadedFiles(Movie movie)
        {
            try
            {
                if (!string.IsNullOrEmpty(movie.PosterPath))
                {
                    var posterPath = Path.Combine(_environment.WebRootPath, movie.PosterPath.TrimStart('/'));
                    if (System.IO.File.Exists(posterPath))
                        System.IO.File.Delete(posterPath);
                }

                if (!string.IsNullOrEmpty(movie.TrailerPath))
                {
                    var trailerPath = Path.Combine(_environment.WebRootPath, movie.TrailerPath.TrimStart('/'));
                    if (System.IO.File.Exists(trailerPath))
                        System.IO.File.Delete(trailerPath);
                }

                if (movie.Episodes != null)
                {
                    foreach (var episode in movie.Episodes)
                    {
                        if (!string.IsNullOrEmpty(episode.VideoPath))
                        {
                            var videoPath = Path.Combine(_environment.WebRootPath, episode.VideoPath.TrimStart('/'));
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

        // GET: Admin/Movies/Edit/5
        // GET: Movie/Edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Country)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.Episodes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        var viewModel = new MovieViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            EnglishTitle = movie.EnglishTitle,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            CountryId = movie.CountryId,
            Director = movie.Director,
            Cast = movie.Cast,
            TotalEpisodes = movie.TotalEpisodes,
            IsCompleted = movie.IsCompleted,
            PosterPath = movie.PosterPath,
            TrailerPath = movie.TrailerPath,
            SelectedGenreIds = movie.MovieGenres.Select(mg => mg.GenreId).ToList(),
            Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync(),
            Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync(),
            ExistingEpisodes = movie.Episodes.Select(e => new EpisodeViewModel
            {
                Id = e.Id,
                MovieId = e.MovieId,
                EpisodeNumber = e.EpisodeNumber,
                Title = e.Title,
                Description = e.Description,
                Duration = e.Duration,
                ReleaseDate = e.ReleaseDate,
                VideoPath = e.VideoPath // Giữ đường dẫn video cũ
            }).OrderBy(e => e.EpisodeNumber).ToList(),
            NewEpisodesToAdd = new List<EpisodeViewModel>() // Khởi tạo danh sách tập phim mới trống
        };

        return View(viewModel);
    }

    // POST: Movie/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(524_288_000)] // Giới hạn kích thước request (ví dụ: 500MB)
    public async Task<IActionResult> Edit(int id, MovieViewModel movieVM)
    {
        if (id != movieVM.Id)
        {
            return NotFound();
        }

        // Lấy bộ phim gốc từ DB để so sánh và cập nhật
        var existingMovie = await _context.Movies
            .Include(m => m.MovieGenres)
            .Include(m => m.Episodes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        // Xóa các lỗi Model State liên quan đến các tập phim (ExistingEpisodes, NewEpisodesToAdd)
        // để ModelState.IsValid chỉ còn kiểm tra các trường của phim chính.
        // Chúng ta sẽ validate các tập phim một cách thủ công sau.
        var keysToRemove = ModelState.Keys.Where(k => k.StartsWith("ExistingEpisodes[") || k.StartsWith("NewEpisodesToAdd[")).ToList();
        foreach (var key in keysToRemove)
        {
            ModelState.Remove(key);
        }

        // Validate file tải lên riêng lẻ trước khi kiểm tra ModelState chính
        if (!ValidateUploadedFiles(movieVM, out string fileErrorMessage))
        {
            _logger.LogWarning("Xác thực file thất bại khi chỉnh sửa phim ID: {MovieId}. Lỗi: {ErrorMessage}", id, fileErrorMessage);
            ModelState.AddModelError("", fileErrorMessage);
            // Tải lại danh sách Countries và Genres để hiển thị trong dropdown
            movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
            movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            // Tải lại ExistingEpisodes để form hiển thị đúng
            movieVM.ExistingEpisodes = existingMovie.Episodes.Select(e => new EpisodeViewModel
            {
                Id = e.Id, MovieId = e.MovieId, EpisodeNumber = e.EpisodeNumber, Title = e.Title, Description = e.Description, Duration = e.Duration, ReleaseDate = e.ReleaseDate, VideoPath = e.VideoPath
            }).OrderBy(e => e.EpisodeNumber).ToList();
            return View(movieVM);
        }

        // Kiểm tra hợp lệ cho các tập phim mới đã được người dùng nhập liệu (chỉ khi có file video)
        // Chúng ta thực hiện validation thủ công ở đây.
        if (movieVM.NewEpisodesToAdd != null)
        {
            for (int i = 0; i < movieVM.NewEpisodesToAdd.Count; i++)
            {
                var episodeVM = movieVM.NewEpisodesToAdd[i];
                // Chỉ validate nếu người dùng đã chọn file video cho tập phim mới này
                if (episodeVM.VideoFile != null)
                {
                    var episodePrefix = $"NewEpisodesToAdd[{i}]";
                    if (string.IsNullOrEmpty(episodeVM.Title))
                    {
                        ModelState.AddModelError($"{episodePrefix}.Title", "Tên tập phim mới là bắt buộc.");
                    }
                    if (episodeVM.EpisodeNumber < 1)
                    {
                        ModelState.AddModelError($"{episodePrefix}.EpisodeNumber", "Số tập phim mới phải lớn hơn 0.");
                    }
                    // // Thêm các validation khác cho tập phim mới nếu cần (ví dụ: duration, releaseDate)
                    // if (episodeVM.Duration.HasValue && (episodeVM.Duration.Value < 1 || episodeVM.Duration.Value > 600))
                    // {
                    //     ModelState.AddModelError($"{episodePrefix}.Duration", "Thời lượng phải từ 1-600 phút.");
                    // }
                    // if (episodeVM.ReleaseDate.HasValue && episodeVM.ReleaseDate.Value.Year < 1900) // Ví dụ validation date
                    // {
                    //     ModelState.AddModelError($"{episodePrefix}.ReleaseDate", "Ngày phát hành không hợp lệ.");
                    // }
                }
            }
        }

        // Kiểm tra trùng lặp số tập phim trước khi thực sự kiểm tra ModelState chính
        // Điều này để tránh hiển thị lỗi validation chung chung từModelState
        var allExistingEpisodeNumbers = existingMovie.Episodes.Select(e => e.EpisodeNumber).ToList();
        var newEpisodeNumbers = movieVM.NewEpisodesToAdd?.Where(evm => evm.VideoFile != null).Select(evm => evm.EpisodeNumber).ToList() ?? new List<int>();

        // Kiểm tra trùng lặp trong các tập phim mới
        if (newEpisodeNumbers.Count != newEpisodeNumbers.Distinct().Count())
        {
            ModelState.AddModelError("", "Có số tập phim mới bị trùng lặp trong danh sách các tập phim mới bạn nhập.");
        }

        // Kiểm tra trùng lặp giữa tập phim mới và tập phim hiện có
        var mergedEpisodeNumbers = allExistingEpisodeNumbers.Concat(newEpisodeNumbers).ToList();
        if (mergedEpisodeNumbers.Count != mergedEpisodeNumbers.Distinct().Count())
        {
            ModelState.AddModelError("", "Có số tập phim mới trùng với số tập phim hiện có.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Xác thực ModelState thất bại khi chỉnh sửa phim ID: {MovieId}", id);
            // Tải lại danh sách Countries và Genres để hiển thị trong dropdown
            movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
            movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            // Tải lại ExistingEpisodes để form hiển thị đúng
            movieVM.ExistingEpisodes = existingMovie.Episodes.Select(e => new EpisodeViewModel
            {
                Id = e.Id, MovieId = e.MovieId, EpisodeNumber = e.EpisodeNumber, Title = e.Title, Description = e.Description, Duration = e.Duration, ReleaseDate = e.ReleaseDate, VideoPath = e.VideoPath
            }).OrderBy(e => e.EpisodeNumber).ToList();
            return View(movieVM);
        }

        // --- Xử lý các file ---
        string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
        Directory.CreateDirectory(uploadsFolder);

            try
            {
                // Xử lý Poster
                if (movieVM.PosterFile != null)
                {
                    if (!string.IsNullOrEmpty(existingMovie.PosterPath) && System.IO.File.Exists(Path.Combine(_environment.WebRootPath, existingMovie.PosterPath)))
                    {
                        System.IO.File.Delete(Path.Combine(_environment.WebRootPath, existingMovie.PosterPath));
                    }
                    existingMovie.PosterPath = await ProcessFileUpload(movieVM.PosterFile, uploadsFolder, $"poster_{id}");
                }

                // Xử lý Trailer
                if (movieVM.TrailerFile != null)
                {
                    if (!string.IsNullOrEmpty(existingMovie.TrailerPath) && System.IO.File.Exists(Path.Combine(_environment.WebRootPath, existingMovie.TrailerPath)))
                    {
                        System.IO.File.Delete(Path.Combine(_environment.WebRootPath, existingMovie.TrailerPath));
                    }
                    existingMovie.TrailerPath = await ProcessFileUpload(movieVM.TrailerFile, uploadsFolder, $"trailer_{id}");
                }

                // Cập nhật thông tin phim chính
                existingMovie.Title = movieVM.Title;
                existingMovie.EnglishTitle = movieVM.EnglishTitle;
                existingMovie.Description = movieVM.Description;
                existingMovie.ReleaseYear = movieVM.ReleaseYear;
                existingMovie.CountryId = movieVM.CountryId;
                existingMovie.Director = movieVM.Director;
                existingMovie.Cast = movieVM.Cast;
                existingMovie.TotalEpisodes = movieVM.TotalEpisodes;
                existingMovie.IsCompleted = movieVM.IsCompleted;
                existingMovie.UpdatedAt = DateTime.Now;

                // Cập nhật Thể loại (MovieGenres)
                _context.MovieGenres.RemoveRange(existingMovie.MovieGenres);
                if (movieVM.SelectedGenreIds != null)
                {
                    foreach (var genreId in movieVM.SelectedGenreIds)
                    {
                        _context.MovieGenres.Add(new MovieGenre { MovieId = id, GenreId = genreId });
                    }
                }

                // --- Xử lý các tập phim ---

                // 1. Xóa các tập phim đã bị xóa khỏi form
                var existingEpisodeIds = existingMovie.Episodes.Select(e => e.Id).ToList();
                var updatedEpisodeIdsInForm = movieVM.ExistingEpisodes.Where(evm => evm.Id > 0).Select(evm => evm.Id).ToList();
                var episodeIdsToDelete = existingEpisodeIds.Except(updatedEpisodeIdsInForm).ToList();

                foreach (var episodeId in episodeIdsToDelete)
                {
                    var episodeToDelete = await _context.Episodes.FindAsync(episodeId);
                    if (episodeToDelete != null)
                    {
                        if (!string.IsNullOrEmpty(episodeToDelete.VideoPath) && System.IO.File.Exists(Path.Combine(_environment.WebRootPath, episodeToDelete.VideoPath)))
                        {
                            System.IO.File.Delete(Path.Combine(_environment.WebRootPath, episodeToDelete.VideoPath));
                        }
                        _context.Episodes.Remove(episodeToDelete);
                    }
                }

                // 2. Cập nhật các tập phim hiện có
                foreach (var episodeVM in movieVM.ExistingEpisodes)
                {
                    if (episodeVM.Id > 0) // Là tập phim hiện có
                    {
                        var episodeToUpdate = await _context.Episodes.FindAsync(episodeVM.Id);
                        if (episodeToUpdate != null)
                        {
                            // Cập nhật file video nếu có file mới
                            if (episodeVM.VideoFile != null)
                            {
                                if (!string.IsNullOrEmpty(episodeToUpdate.VideoPath) && System.IO.File.Exists(Path.Combine(_environment.WebRootPath, episodeToUpdate.VideoPath)))
                                {
                                    System.IO.File.Delete(Path.Combine(_environment.WebRootPath, episodeToUpdate.VideoPath));
                                }
                                episodeToUpdate.VideoPath = await ProcessFileUpload(episodeVM.VideoFile, uploadsFolder, $"episode_{episodeVM.Id}_{episodeVM.EpisodeNumber}");
                            }
                            // Cập nhật các thông tin khác
                            episodeToUpdate.EpisodeNumber = episodeVM.EpisodeNumber;
                            episodeToUpdate.Title = episodeVM.Title;
                            episodeToUpdate.Description = episodeVM.Description;
                            episodeToUpdate.Duration = episodeVM.Duration;
                            episodeToUpdate.ReleaseDate = episodeVM.ReleaseDate;
                            episodeToUpdate.UpdatedAt = DateTime.Now;
                            _context.Episodes.Update(episodeToUpdate);
                        }
                    }
                    // Nếu episodeVM.Id == 0, chúng ta sẽ bỏ qua nó ở đây vì nó sẽ được xử lý trong phần NewEpisodesToAdd
                }

                // 3. Thêm các tập phim mới từ danh sách NewEpisodesToAdd
                if (movieVM.NewEpisodesToAdd != null)
                {
                    // Lọc ra chỉ những tập phim thực sự có file video được tải lên
                    var actualNewEpisodes = movieVM.NewEpisodesToAdd.Where(evm => evm.VideoFile != null).ToList();

                    foreach (var episodeVM in actualNewEpisodes)
                    {
                        // Kiểm tra lại xem số tập này có thực sự được chọn để thêm không
                        if (episodeVM.VideoFile != null)
                        {
                            var videoPath = await ProcessFileUpload(episodeVM.VideoFile, uploadsFolder, $"episode_{id}_{episodeVM.EpisodeNumber}");
                            var newEpisode = new Episode
                            {
                                MovieId = id,
                                EpisodeNumber = episodeVM.EpisodeNumber,
                                Title = episodeVM.Title,
                                Description = episodeVM.Description,
                                Duration = episodeVM.Duration,
                                ReleaseDate = episodeVM.ReleaseDate,
                                VideoPath = videoPath,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };
                            _context.Episodes.Add(newEpisode);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Đã cập nhật phim: {existingMovie.Title} (ID: {id})");
                TempData["SuccessMessage"] = $"Đã cập nhật phim '{existingMovie.Title}' thành công!";
                // return RedirectToAction(nameof(Index)); // Hoặc redirect tới trang chi tiết phim
            return RedirectToAction(nameof(ListMovies));
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chỉnh sửa phim ID: {MovieId}", id);
                // Xóa các file đã upload nếu có lỗi trong quá trình save
                if (!string.IsNullOrEmpty(existingMovie.PosterPath)) System.IO.File.Delete(Path.Combine(_environment.WebRootPath, existingMovie.PosterPath));
                if (!string.IsNullOrEmpty(existingMovie.TrailerPath)) System.IO.File.Delete(Path.Combine(_environment.WebRootPath, existingMovie.TrailerPath));
                foreach (var episodeVM in movieVM.ExistingEpisodes)
                {
                    if (!string.IsNullOrEmpty(episodeVM.VideoPath)) System.IO.File.Delete(Path.Combine(_environment.WebRootPath, episodeVM.VideoPath));
                }
                // Cleanup các file tập phim mới upload trong trường hợp lỗi
                if (movieVM.NewEpisodesToAdd != null)
                {
                    foreach (var episodeVM in movieVM.NewEpisodesToAdd)
                    {
                        if (episodeVM.VideoFile != null) // Nếu có file được chọn
                        {
                            var tempFilePath = Path.Combine(uploadsFolder, Path.GetFileName(episodeVM.VideoFile.FileName)); // Cách tạo tên tạm thời
                            if (System.IO.File.Exists(tempFilePath)) // Cần đảm bảo tên file tạm là chính xác
                            {
                                // System.IO.File.Delete(tempFilePath); // Logic cleanup này cần được cẩn thận
                            }
                        }
                    }
                }


                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi chỉnh sửa phim: {ex.Message}";

                // Tải lại danh sách Countries và Genres để hiển thị trong dropdown
                movieVM.Countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
                movieVM.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                // Tải lại ExistingEpisodes để form hiển thị đúng
                movieVM.ExistingEpisodes = existingMovie.Episodes.Select(e => new EpisodeViewModel
                {
                    Id = e.Id,
                    MovieId = e.MovieId,
                    EpisodeNumber = e.EpisodeNumber,
                    Title = e.Title,
                    Description = e.Description,
                    Duration = e.Duration,
                    ReleaseDate = e.ReleaseDate,
                    VideoPath = e.VideoPath
                }).OrderBy(e => e.EpisodeNumber).ToList();
                return View(movieVM);
            }
    }



        // GET: Admin/Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Country)
                //.Include(m => m.Genre) // Bỏ đi nếu không dùng
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: Admin/Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.Include(m => m.Episodes).FirstOrDefaultAsync(m => m.Id == id);
            if (movie != null)
            {
                // Xóa file poster và trailer
                DeleteFile(movie.PosterPath);
                DeleteFile(movie.TrailerPath);

                // Xóa file video của các tập phim
                foreach (var episode in movie.Episodes)
                {
                    DeleteFile(episode.VideoPath);
                }

                // EF Core sẽ tự động xóa các bản ghi liên quan trong Episodes, MovieGenres, Comments, Ratings...
                // do đã cấu hình quan hệ khóa ngoại.
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa phim {movie.Title} và tất cả dữ liệu liên quan thành công!";
            }

            return RedirectToAction(nameof(ListMovies));
        }
    }
}