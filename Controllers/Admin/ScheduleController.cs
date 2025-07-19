using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWebsite.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // Action chính để hiển thị trang lịch chiếu
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Xác định khoảng thời gian cần lấy lịch
            var today = DateTime.Today;
            var endDate = today.AddDays(7); // Lấy lịch cho 7 ngày tới

            // 2. Truy vấn CSDL để lấy các lịch trình trong khoảng thời gian đã định
            // Sử dụng Include để tải sẵn dữ liệu liên quan (Movie, Episode) -> tránh lỗi N+1 query
            var schedules = await _context.Schedules
                .Include(s => s.Movie)
                // .Include(s => s.Episode) // Không cần thiết nếu bạn không dùng thông tin gì khác ngoài EpisodeNumber từ Episode
                .Where(s => s.ScheduledTime >= today && s.ScheduledTime < endDate)
                .OrderBy(s => s.ScheduledTime) // Sắp xếp theo thời gian sớm nhất
                .ToListAsync();

            // 3. Xử lý dữ liệu và gom nhóm theo ngày
            // Dùng Dictionary với Key là ngày (không có giờ), Value là danh sách các mục lịch chiếu của ngày đó
            var groupedSchedules = schedules
                .GroupBy(s => s.ScheduledTime.Date)
                .ToDictionary(
                    group => group.Key, // Key là ngày (ví dụ: 2024-05-24 00:00:00)
                    group => group.Select(s => new ScheduleItemViewModel
                    {
                        // 4. Map dữ liệu từ Model sang ScheduleItemViewModel
                        ScheduleId = s.Id,
                        MovieId = s.Movie.Id,
                        MovieTitle = s.Movie.Title,
                        // Hoặc PosterDoc tùy bạn chọn
                        PosterDoc = s.Movie.PosterDoc,
                        // Lấy thông tin tập phim từ chính lịch chiếu nếu có
                        EpisodeNumber = s.EpisodeId.HasValue ? _context.Episodes.Find(s.EpisodeId.Value)?.EpisodeNumber : null,
                        EpisodeTitle = s.EpisodeId.HasValue ? _context.Episodes.Find(s.EpisodeId.Value)?.Title : null,
                        ScheduledTime = s.ScheduledTime,
                        Description = s.Description,
                        EntryType = s.EntryType,
                        // Tạo một tiêu đề chung để hiển thị
                        ItemTitle = s.EpisodeId.HasValue
                            ? $"{s.Movie.Title} - Tập {_context.Episodes.Find(s.EpisodeId.Value)?.EpisodeNumber}"
                            : s.Movie.Title
                    }).ToList() // Value là danh sách các ScheduleItemViewModel của ngày đó
                );

            // 5. Tạo ViewModel chính cho View
            var viewModel = new ScheduleViewModel
            {
                // Tạo danh sách các ngày để hiển thị tab (Hôm nay, Ngày mai, Thứ 2,...)
                SelectableDates = Enumerable.Range(0, 7).Select(i => today.AddDays(i)).ToList(),
                ScheduledGroups = groupedSchedules,
                ViewType = "week" // Có thể đặt mặc định hoặc dựa trên tham số query
            };

            return View(viewModel);
        }
        // Helper method để lấy danh sách phim cho dropdown, tránh lặp code
        private async Task PopulateMoviesDropdown(ScheduleViewModelForAdmin viewModel)
        {
            viewModel.Movies = await _context.Movies
                                            .OrderBy(m => m.Title)
                                            .ToListAsync();
        }

        // GET: /Schedule/AdminIndex - Trang quản lý lịch chiếu
        // Đổi tên action để không trùng với trang Index công khai
        public async Task<IActionResult> AdminIndex()
        {
            // Tạo ViewModel mới, nhẹ hơn cho trang danh sách
            var scheduleList = await _context.Schedules
                .Include(s => s.Movie) // Lấy thông tin phim
                .Include(s => s.Episode) // Lấy thông tin tập phim
                .OrderByDescending(s => s.ScheduledTime)
                .Select(s => new ScheduleAdminItemViewModel // Map sang ViewModel mới
                {
                    Id = s.Id,
                    ScheduledTime = s.ScheduledTime,
                    MovieTitle = s.Movie.Title,
                    // Sử dụng toán tử ?. để tránh lỗi nếu Episode là null
                    EpisodeInfo = s.Episode != null ? $"Tập {s.Episode.EpisodeNumber} - {s.Episode.Title}" : "Phim lẻ / Sự kiện",
                    Description = s.Description,
                    EntryType = s.EntryType
                })
                .ToListAsync();

            return View(scheduleList);
        }

        // === API Endpoint để lấy danh sách tập phim ===
        // GET: /Schedule/GetEpisodesForMovie?movieId=5
        [HttpGet]
        public async Task<IActionResult> GetEpisodesForMovie(int movieId)
        {
            if (movieId <= 0)
            {
                return Json(new List<object>()); // Trả về danh sách rỗng nếu không có movieId
            }

            var episodes = await _context.Episodes
                .Where(e => e.MovieId == movieId)
                .OrderBy(e => e.EpisodeNumber)
                .Select(e => new { id = e.Id, text = $"Tập {e.EpisodeNumber}: {e.Title}" }) // Định dạng cho select/option
                .ToListAsync();

            return Json(episodes);
        }
        // GET: Schedule/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ScheduleViewModelForAdmin();
            // Lấy danh sách phim để hiển thị trong dropdown
            await PopulateMoviesDropdown(viewModel);
            return View(viewModel);
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleViewModelForAdmin viewModel)
        {
            // Luôn cần xóa các thuộc tính chỉ dùng để hiển thị khỏi ModelState
            ModelState.Remove("Movies");

            // === VALIDATION LOGIC NÂNG CAO ===
            // Chỉ kiểm tra thông tin tập mới khi người dùng chọn loại là "Ra mắt tập phim"
            if (viewModel.EntryType == ScheduleType.EpisodeRelease)
            {
                if (!viewModel.NewEpisodeNumber.HasValue)
                {
                    ModelState.AddModelError(nameof(viewModel.NewEpisodeNumber), "Số tập là bắt buộc khi lên lịch cho tập phim mới.");
                }
                if (string.IsNullOrWhiteSpace(viewModel.NewEpisodeTitle))
                {
                    ModelState.AddModelError(nameof(viewModel.NewEpisodeTitle), "Tên tập là bắt buộc khi lên lịch cho tập phim mới.");
                }

                // Nếu số tập có giá trị, kiểm tra xem nó đã tồn tại cho phim này chưa
                if (viewModel.NewEpisodeNumber.HasValue)
                {
                    bool episodeExists = await _context.Episodes
                        .AnyAsync(e => e.MovieId == viewModel.MovieId && e.EpisodeNumber == viewModel.NewEpisodeNumber.Value);

                    if (episodeExists)
                    {
                        ModelState.AddModelError(nameof(viewModel.NewEpisodeNumber), $"Tập {viewModel.NewEpisodeNumber.Value} đã tồn tại cho phim này. Vui lòng chọn số tập khác.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // Sử dụng transaction để đảm bảo cả 2 thao tác (tạo tập, tạo lịch) đều thành công hoặc thất bại cùng nhau
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        int? episodeIdForSchedule = null;

                        // Chỉ tạo tập phim mới nếu loại hình là ra mắt tập
                        if (viewModel.EntryType == ScheduleType.EpisodeRelease)
                        {
                            // 1. TẠO TẬP PHIM MỚI
                            var newEpisode = new Episode
                            {
                                MovieId = viewModel.MovieId,
                                EpisodeNumber = viewModel.NewEpisodeNumber.Value,
                                Title = viewModel.NewEpisodeTitle,
                                Description = viewModel.Description, // Có thể dùng chung mô tả
                                ReleaseDate = viewModel.ScheduledTime, // Ngày phát hành dự kiến
                                VideoPath = "placeholder.mp4", // Đặt một giá trị tạm, sẽ cập nhật sau
                                Duration = 0,
                                Views = 0,
                            };
                            _context.Episodes.Add(newEpisode);
                            await _context.SaveChangesAsync(); // Lưu để lấy được newEpisode.Id

                            episodeIdForSchedule = newEpisode.Id; // Lấy ID của tập vừa tạo
                        }

                        // 2. TẠO LỊCH CHIẾU
                        var schedule = new Schedule
                        {
                            ScheduledTime = viewModel.ScheduledTime,
                            MovieId = viewModel.MovieId,
                            EpisodeId = episodeIdForSchedule, // Gán ID nếu có, nếu không thì là null (cho phim lẻ)
                            Description = viewModel.Description,
                            EntryType = viewModel.EntryType,
                        };
                        _context.Schedules.Add(schedule);
                        await _context.SaveChangesAsync();

                        // Nếu tất cả thành công, xác nhận transaction
                        await transaction.CommitAsync();

                        // TempData["SuccessMessage"] = "Đã lên lịch chiếu thành công!";
                        return RedirectToAction(nameof(AdminIndex));
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, hủy bỏ tất cả thay đổi
                        await transaction.RollbackAsync();
                        // Ghi lại log lỗi (quan trọng cho gỡ lỗi)
                        // logger.LogError(ex, "Lỗi khi tạo lịch chiếu mới.");
                        ModelState.AddModelError("", "Đã có lỗi không mong muốn xảy ra. Vui lòng thử lại.");
                    }
                }
            }

            // Nếu model không hợp lệ, tải lại danh sách phim và trả về view với các lỗi
            viewModel.Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
            return View(viewModel);
        }
        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            // Map từ Model sang ViewModel để hiển thị trên form
            var viewModel = new ScheduleViewModelForAdmin
            {
                Id = schedule.Id,
                ScheduledTime = schedule.ScheduledTime,
                MovieId = schedule.MovieId,
                EpisodeId = schedule.EpisodeId,
                Description = schedule.Description,
                EntryType = schedule.EntryType
            };

            // Lấy danh sách phim cho dropdown
            await PopulateMoviesDropdown(viewModel);
            // Load danh sách tập phim cho phim hiện tại
            if (viewModel.MovieId > 0)
            {
                viewModel.Episodes = await _context.Episodes
                    .Where(e => e.MovieId == viewModel.MovieId)
                    .OrderBy(e => e.EpisodeNumber)
                    .ToListAsync();
            }
            return View(viewModel);
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ScheduleViewModelForAdmin viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            // Luôn xóa các thuộc tính hiển thị khỏi ModelState
            ModelState.Remove("Movies");
            ModelState.Remove("Episodes");
            ModelState.Remove("NewEpisodeNumber");
            ModelState.Remove("NewEpisodeTitle");

            if (ModelState.IsValid)
            {
                try
                {
                    var scheduleToUpdate = await _context.Schedules.FindAsync(id);
                    if (scheduleToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật các thuộc tính chung
                    scheduleToUpdate.ScheduledTime = viewModel.ScheduledTime;
                    scheduleToUpdate.MovieId = viewModel.MovieId;
                    scheduleToUpdate.Description = viewModel.Description;
                    scheduleToUpdate.EntryType = viewModel.EntryType;

                    // === LOGIC MỚI: XỬ LÝ KHI LOẠI LỊCH CHIẾU THAY ĐỔI ===
                    if (viewModel.EntryType == ScheduleType.EpisodeRelease)
                    {
                        // Nếu là loại ra mắt tập, lấy EpisodeId từ form
                        scheduleToUpdate.EpisodeId = viewModel.EpisodeId;
                    }
                    else
                    {
                        // Nếu là các loại khác (Phim lẻ, sự kiện...), đảm bảo không có EpisodeId
                        scheduleToUpdate.EpisodeId = null;
                    }
                    // ====================================================

                    _context.Update(scheduleToUpdate);
                    await _context.SaveChangesAsync();

                    // TempData["SuccessMessage"] = "Cập nhật lịch chiếu thành công!";
                    return RedirectToAction(nameof(AdminIndex));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Schedules.Any(e => e.Id == viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Nếu model không hợp lệ, tải lại dữ liệu cần thiết cho form
            viewModel.Movies = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
            if (viewModel.MovieId > 0)
            {
                viewModel.Episodes = await _context.Episodes
                    .Where(e => e.MovieId == viewModel.MovieId)
                    .OrderBy(e => e.EpisodeNumber)
                    .ToListAsync();
            }
            return View(viewModel);
        }
        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Dùng Include để hiển thị thêm thông tin tên phim cho dễ xác nhận
            var schedule = await _context.Schedules
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule); // Trả về thẳng model Schedule để hiển thị thông tin xác nhận
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            // TODO: Redirect đến trang danh sách lịch chiếu của Admin
            return RedirectToAction(nameof(Index));
        }

        // Hàm GetSelectableDates không thay đổi
        // private List<DateTime> GetSelectableDates()
        // {
        //     var dates = new List<DateTime>();
        //     for (int i = 0; i < 7; i++)
        //     {
        //         dates.Add(DateTime.Today.AddDays(i));
        //     }
        //     return dates;
        // }


        // Hiển thị danh sách lịch chiếu (Admin)
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> Manage()
        // {
        //     var schedules = await _context.Schedules
        //         .Include(s => s.Movie)
        //         .Include(s => s.Episode)
        //         .OrderBy(s => s.ScheduledTime)
        //         .ToListAsync();

        //     var model = schedules.Select(s => new ScheduleViewModelForAdmin
        //     {
        //         Id = s.Id,
        //         ScheduledTime = s.ScheduledTime,
        //         MovieId = s.MovieId,
        //         EpisodeId = s.EpisodeId,
        //         Description = s.Description,
        //         EntryType = s.EntryType,
        //         Movies = _context.Movies.ToList(),
        //         Episodes = _context.Episodes.Where(e => e.MovieId == s.MovieId).ToList()
        //     }).ToList();

        //     return View(model);
        // }

        // // Hiển thị form tạo lịch chiếu mới (Admin)
        // [HttpGet]
        // public IActionResult Create()
        // {
        //     var model = new ScheduleViewModelForAdmin
        //     {
        //         ScheduledTime = DateTime.Now,
        //         Movies = _context.Movies.ToList(),
        //         Episodes = _context.Episodes.ToList()
        //     };
        //     return View(model);
        // }

        // // Xử lý tạo lịch chiếu mới (Admin)
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(ScheduleViewModelForAdmin model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         int? episodeId = null;
        //         // Nếu người dùng nhập tên tập phim
        //         if (!string.IsNullOrWhiteSpace(model.EpisodeTitle))
        //         {
        //             // Tạo mới Episode
        //             var episode = new Episode
        //             {
        //                 MovieId = model.MovieId,
        //                 Title = model.EpisodeTitle,
        //                 Description = "", // Có thể bổ sung field mô tả nếu muốn
        //                 VideoPath = "", // Có thể bổ sung field nếu muốn
        //                 Duration = 0,
        //                 ReleaseDate = DateTime.Now,
        //                 CreatedAt = DateTime.Now,
        //                 UpdatedAt = DateTime.Now
        //             };
        //             _context.Episodes.Add(episode);
        //             await _context.SaveChangesAsync();
        //             episodeId = episode.Id;
        //         }

        //         var schedule = new Schedule
        //         {
        //             ScheduledTime = model.ScheduledTime,
        //             MovieId = model.MovieId,
        //             EpisodeId = episodeId, // null nếu không nhập tên tập
        //             Description = model.Description,
        //             EntryType = model.EntryType,
        //             CreatedAt = DateTime.Now
        //         };

        //         _context.Schedules.Add(schedule);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Manage));
        //     }

        //     model.Movies = _context.Movies.ToList();
        //     model.Episodes = _context.Episodes.ToList();
        //     return View(model);
        // }

        // // Hiển thị form chỉnh sửa lịch chiếu (Admin)
        // [HttpGet]
        // public async Task<IActionResult> Edit(int id)
        // {
        //     var schedule = await _context.Schedules
        //         .Include(s => s.Movie)
        //         .Include(s => s.Episode)
        //         .FirstOrDefaultAsync(s => s.Id == id);

        //     if (schedule == null)
        //     {
        //         return NotFound();
        //     }

        //     var model = new ScheduleViewModelForAdmin
        //     {
        //         Id = schedule.Id,
        //         ScheduledTime = schedule.ScheduledTime,
        //         MovieId = schedule.MovieId,
        //         EpisodeId = schedule.EpisodeId,
        //         Description = schedule.Description,
        //         EntryType = schedule.EntryType,
        //         Movies = _context.Movies.ToList(),
        //         Episodes = _context.Episodes.Where(e => e.MovieId == schedule.MovieId).ToList()
        //     };

        //     return View(model);
        // }

        // // Xử lý chỉnh sửa lịch chiếu (Admin)
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(ScheduleViewModelForAdmin model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var schedule = await _context.Schedules.FindAsync(model.Id);
        //         if (schedule == null)
        //         {
        //             return NotFound();
        //         }

        //         schedule.ScheduledTime = model.ScheduledTime;
        //         schedule.MovieId = model.MovieId;
        //         schedule.EpisodeId = model.EpisodeId;
        //         schedule.Description = model.Description;
        //         schedule.EntryType = model.EntryType;

        //         _context.Schedules.Update(schedule);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Manage));
        //     }

        //     model.Movies = _context.Movies.ToList();
        //     model.Episodes = _context.Episodes.Where(e => e.MovieId == model.MovieId).ToList();
        //     return View(model);
        // }

        // // Hiển thị form xác nhận xóa lịch chiếu (Admin)
        // [HttpGet]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var schedule = await _context.Schedules
        //         .Include(s => s.Movie)
        //         .Include(s => s.Episode)
        //         .FirstOrDefaultAsync(s => s.Id == id);

        //     if (schedule == null)
        //     {
        //         return NotFound();
        //     }

        //     var model = new ScheduleViewModelForAdmin
        //     {
        //         Id = schedule.Id,
        //         ScheduledTime = schedule.ScheduledTime,
        //         MovieId = schedule.MovieId,
        //         EpisodeId = schedule.EpisodeId,
        //         Description = schedule.Description,
        //         EntryType = schedule.EntryType,
        //         Movies = _context.Movies.ToList(),
        //         Episodes = _context.Episodes.Where(e => e.MovieId == schedule.MovieId).ToList()
        //     };

        //     return View(model);
        // }

        // // Xử lý xóa lịch chiếu (Admin)
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var schedule = await _context.Schedules.FindAsync(id);
        //     if (schedule == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Schedules.Remove(schedule);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Manage));
        // }

        // // Action AJAX để lấy danh sách tập phim theo MovieId
        // [HttpGet]
        // public async Task<IActionResult> GetEpisodesByMovie(int movieId)
        // {
        //     var episodes = await _context.Episodes
        //         .Where(e => e.MovieId == movieId)
        //         .Select(e => new { id = e.Id, title = e.Title, episodeNumber = e.EpisodeNumber })
        //         .ToListAsync();

        //     return Json(episodes);
        // }

        // Hàm lấy danh sách ngày có thể chọn (7 ngày tới)


    }
}