
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;

using System.Linq;
using System.Threading.Tasks;

public class ScheduleController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ScheduleController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // ===================================================================
    // PHẦN DÀNH CHO NGƯỜI DÙNG (PUBLIC)
    // ===================================================================

    // GET: /Schedule hoặc /Schedule/Index
    // [AllowAnonymous] // Mọi người đều có thể xem
    public async Task<IActionResult> Index()
    {
        var now = DateTime.UtcNow;
        var endDate = now.AddDays(7); // Lấy lịch trong 7 ngày tới

        // Lấy lịch chiếu sắp tới, không kiểm tra IsPublished
        var schedules = await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Episode)
            .Where(s => s.ScheduledTime >= now && s.ScheduledTime < endDate)
            .OrderBy(s => s.ScheduledTime)
            .ToListAsync();

        // Nhóm lịch chiếu theo ngày
        var groupedSchedules = schedules
            .GroupBy(s => s.ScheduledTime.ToLocalTime().Date)
            .ToDictionary(
                group => group.Key,
                group => group.Select(s => new ScheduleItemViewModel
                {
                    ScheduleId = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    PosterDoc = s.Movie.PosterDoc,
                    EpisodeNumber = s.Episode?.EpisodeNumber,
                    ScheduledTime = s.ScheduledTime,
                    Description = s.Description,
                    EntryType = s.EntryType,
                    ItemTitle = s.EpisodeId.HasValue && s.Episode != null
                        ? $"Tập {s.Episode.EpisodeNumber}: {s.Episode.Title}"
                        : s.Movie.Title
                }).ToList()
            );

        var viewModel = new ScheduleViewModel
        {
            ScheduledGroups = groupedSchedules
        };

        return View("Index", viewModel); // Chỉ định view rõ ràng để không bị nhầm lẫn
    }

    // ===================================================================
    // PHẦN DÀNH CHO QUẢN TRỊ VIÊN (ADMIN)
    // Các action này phải được bảo vệ
    // ===================================================================

    // // GET: /Schedule/Manage
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Manage()
    {
        var scheduleList = await _context.Schedules
            .Include(s => s.Movie)
            .Include(s => s.Episode)
            .OrderByDescending(s => s.ScheduledTime)
            .Select(s => new ScheduleAdminItemViewModel
            {
                Id = s.Id,
                ScheduledTime = s.ScheduledTime,
                MovieTitle = s.Movie.Title,
                EpisodeInfo = s.Episode != null ? $"Tập {s.Episode.EpisodeNumber} - {s.Episode.Title}" : "Phim lẻ / Sự kiện",
                Description = s.Description,
                EntryType = s.EntryType
            })
            .ToListAsync();

        return View("AdminIndex", scheduleList); // Chỉ định view rõ ràng
    }

    // GET: /Schedule/Create
    // [Authorize(Roles = "Admin")]
    [HttpGet("Schedule/Create")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new ScheduleViewModelForAdmin
        {
            Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync()
        };
        return View(viewModel); // View này mặc định là của Admin
    }

    // POST: /Schedule/Create
    [HttpPost("Schedule/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduleViewModelForAdmin viewModel)
    {
        // Validation=
        if (viewModel.EntryType == ScheduleType.EpisodeRelease)
        {
            if (!viewModel.NewEpisodeNumber.HasValue)
                ModelState.AddModelError(nameof(viewModel.NewEpisodeNumber), "Số tập là bắt buộc.");

            if (string.IsNullOrWhiteSpace(viewModel.NewEpisodeTitle))
                ModelState.AddModelError(nameof(viewModel.NewEpisodeTitle), "Tên tập là bắt buộc.");

            if (viewModel.NewEpisodeVideoFile == null || viewModel.NewEpisodeVideoFile.Length == 0)
                ModelState.AddModelError(nameof(viewModel.NewEpisodeVideoFile), "Vui lòng chọn một file video để upload.");
        }

        if (ModelState.IsValid)
        {
            // Bắt đầu transaction
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // === BẮT ĐẦU ĐIỀN LOGIC VÀO ĐÂY ===

                    int? episodeIdForSchedule = null;

                    // Chỉ thực hiện logic này nếu người dùng muốn tạo lịch cho tập phim mới
                    if (viewModel.EntryType == ScheduleType.EpisodeRelease)
                    {
                        string videoPath = null;
                        if (viewModel.NewEpisodeVideoFile != null)
                        {
                            // 1. Tạo đường dẫn thư mục lưu trữ
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "videos", "episodes");
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            // 2. Tạo tên file duy nhất để tránh trùng lặp
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.NewEpisodeVideoFile.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            // 3. Lưu file vào server
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await viewModel.NewEpisodeVideoFile.CopyToAsync(fileStream);
                            }

                            // 4. Lấy đường dẫn tương đối để lưu vào DB
                            videoPath = "/videos/episodes/" + uniqueFileName;
                        }

                        // 5. Tạo tập phim mới trong DB
                        var newEpisode = new Episode
                        {
                            MovieId = viewModel.MovieId,
                            EpisodeNumber = viewModel.NewEpisodeNumber.Value,
                            Title = viewModel.NewEpisodeTitle,
                            Description = viewModel.Description,
                            ReleaseDate = viewModel.ScheduledTime.ToUniversalTime(),
                            VideoPath = videoPath, // Lưu đường dẫn video
                            Duration = 0, // Cần cải tiến sau
                            IsPublished = false // Mặc định là chưa publish
                        };
                        _context.Episodes.Add(newEpisode);
                        await _context.SaveChangesAsync(); // Lưu để lấy được ID của tập vừa tạo

                        // 6. Gán ID cho lịch chiếu
                        episodeIdForSchedule = newEpisode.Id;
                    }

                    // 7. Tạo lịch chiếu
                    var schedule = new Schedule
                    {
                        ScheduledTime = viewModel.ScheduledTime.ToUniversalTime(),
                        MovieId = viewModel.MovieId,
                        EpisodeId = episodeIdForSchedule, // Sẽ là null nếu không phải tạo tập mới
                        Description = viewModel.Description,
                        EntryType = viewModel.EntryType,
                    };
                    _context.Schedules.Add(schedule);
                    await _context.SaveChangesAsync();

                    // === KẾT THÚC LOGIC ===

                    // Nếu tất cả thành công, commit transaction
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Đã tạo lịch chiếu thành công!";
                    return RedirectToAction(nameof(Manage));
                }
                catch (Exception ex)
                {
                    // Nếu có bất kỳ lỗi nào xảy ra, hủy bỏ tất cả thay đổi
                    await transaction.RollbackAsync();
                    // Ghi log lỗi để debug
                    // logger.LogError(ex, "Lỗi khi tạo lịch chiếu mới."); 
                    ModelState.AddModelError("", "Đã có lỗi không mong muốn xảy ra. Chi tiết: " + ex.Message);
                }
            }
        }

        // Nếu model không hợp lệ, tải lại danh sách phim và trả về view với các lỗi
        viewModel.Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
        return View(viewModel);
    }
    // GET: /Schedule/Edit/5
    // [Authorize(Roles = "Admin")]
        [HttpGet("Schedule/Edit/{id?}")]

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null) return NotFound();

        var viewModel = new ScheduleViewModelForAdmin
        {
            Id = schedule.Id,
            ScheduledTime = schedule.ScheduledTime.ToLocalTime(),
            MovieId = schedule.MovieId,
            EpisodeId = schedule.EpisodeId,
            Description = schedule.Description,
            EntryType = schedule.EntryType,
            Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync()
        };

        if (viewModel.MovieId > 0)
        {
            viewModel.Episodes = await _context.Episodes
                .Where(e => e.MovieId == viewModel.MovieId)
                .OrderBy(e => e.EpisodeNumber).ToListAsync();
        }
        return View(viewModel);
    }

    // POST: /Schedule/Edit/5
    [HttpPost("Schedule/Edit/{id?}")]
    [ValidateAntiForgeryToken]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, ScheduleViewModelForAdmin viewModel)
    {
        // Logic Edit POST của bạn đã tốt, giữ nguyên.
        if (id != viewModel.Id) return NotFound();

        ModelState.Remove(nameof(viewModel.Movies));
        ModelState.Remove(nameof(viewModel.Episodes));
        // ... xóa các model state khác nếu cần

        if (ModelState.IsValid)
        {
            var scheduleToUpdate = await _context.Schedules.FindAsync(id);
            if (scheduleToUpdate == null) return NotFound();

            scheduleToUpdate.ScheduledTime = viewModel.ScheduledTime.ToUniversalTime();
            // ... cập nhật các trường khác
            scheduleToUpdate.EpisodeId = (viewModel.EntryType == ScheduleType.EpisodeRelease) ? viewModel.EpisodeId : null;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        viewModel.Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
        // ... tải lại dữ liệu khác nếu cần
        return View(viewModel);
    }

    // Các action Delete và API GetEpisodesForMovie
    // Đảm bảo tất cả đều có [Authorize(Roles = "Admin")] hoặc được gọi từ các action đã được authorize

    // GET: /Schedule/Delete/5
    // [Authorize(Roles = "Admin")]
    [HttpGet("Schedule/Delete/{id?}")]
    public async Task<IActionResult> Delete(int? id)
    {
        // 1. Kiểm tra ID đầu vào
        if (id == null)
        {
            return NotFound();
        }

        // 2. Tìm lịch chiếu trong DB, bao gồm cả thông tin phim để hiển thị cho rõ
        var schedule = await _context.Schedules
            .Include(s => s.Movie) // Lấy thông tin phim liên quan
            .Include(s => s.Episode) // Lấy thông tin tập phim liên quan
            .FirstOrDefaultAsync(m => m.Id == id);

        // 3. Nếu không tìm thấy, trả về Not Found
        if (schedule == null)
        {
            return NotFound();
        }

        // 4. Trả về View với đối tượng schedule để hiển thị thông tin xác nhận
        return View(schedule);
    }

    // POST: /Schedule/Delete/5
    // [HttpPost, ActionName("Delete")]
    [HttpPost("Schedule/Delete/{id?}")]
    [ValidateAntiForgeryToken]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // 1. Tìm lịch chiếu cần xóa
        var schedule = await _context.Schedules.FindAsync(id);

        // 2. Nếu tìm thấy thì xóa nó
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        // 3. Chuyển hướng về trang quản lý sau khi xóa xong (hoặc dù không tìm thấy)
        return RedirectToAction(nameof(Manage));
    }

    // GET: /Schedule/GetEpisodesForMovie?movieId=5
    // [Authorize(Roles = "Admin")] // Bảo vệ API endpoint
    public async Task<IActionResult> GetEpisodesForMovie(int movieId)
    {
        // Logic của bạn đã tốt
        var episodes = await _context.Episodes
            .Where(e => e.MovieId == movieId)
            .OrderBy(e => e.EpisodeNumber)
            .Select(e => new { id = e.Id, text = $"Tập {e.EpisodeNumber}: {e.Title}" })
            .ToListAsync();
        return Json(episodes);
    }
}