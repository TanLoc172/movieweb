
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using MovieWebsite.Data;
// using MovieWebsite.Models;
// using System;
// using System.Threading.Tasks;

// namespace MovieWebsite.Controllers
// {
//     public class WatchPartyController : Controller
//     {
//         private readonly AppDbContext _context;

//         public WatchPartyController(AppDbContext context)
//         {
//             _context = context;
//         }

//         // Action được gọi từ nút "Xem Cùng Nhau" trên trang chi tiết phim
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create(int movieId)
//         {
//             //Dùng FindAsync để tìm và lấy toàn bộ đối tượng Movie
//             var movie = await _context.Movies.FindAsync(movieId);

//             // Kiểm tra xem phim có được tìm thấy không. Nếu không, movie sẽ là null.
//             if (movie == null)
//             {
//                 // Nếu không tìm thấy, trả về lỗi NotFound
//                 return NotFound("Không tìm thấy phim.");
//             }

//             // Nếu code chạy đến đây, có nghĩa là đã tìm thấy phim và biến 'movie' đã có dữ liệu.
//             var newRoom = new WatchPartyRoom
//             {

//                 Name = $"Phòng xem chung: {movie.Title}",
//                 MovieId = movieId,
//                 InviteCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()
//             };

//             _context.WatchPartyRooms.Add(newRoom);
//             await _context.SaveChangesAsync();

//             return RedirectToAction("Room", new { inviteCode = newRoom.InviteCode });
//         }

//         // Action hiển thị phòng xem phim
//         // URL sẽ có dạng: /watch/ABC123XYZ
//         [HttpGet("watch/{inviteCode}")]
//         public async Task<IActionResult> Room(string inviteCode)
//         {
//             var room = await _context.WatchPartyRooms
//                                      .Include(r => r.Movie)
//                                       .ThenInclude(m => m.Episodes)  // Lấy kèm thông tin phim
//                                      .FirstOrDefaultAsync(r => r.InviteCode == inviteCode && r.IsActive);

//             if (room == null)
//             {
//                 // Có thể tạo một View riêng để thông báo lỗi này
//                 TempData["ErrorMessage"] = "Phòng xem phim không tồn tại hoặc đã kết thúc.";
//                 return RedirectToAction("Index", "Home");
//             }

//             // === GÁN ID PHIM VÀO VIEWBAG ===
//             ViewBag.CurrentMovieId = room.MovieId;
//             // Truyền cả đối tượng room (đã có movie bên trong) cho View
//             return View(room);
//         }
//         // Xử lý khi người dùng submit form tham gia phòng
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Join(string inviteCode)
//         {
//             // Kiểm tra xem inviteCode có được nhập không
//             if (string.IsNullOrWhiteSpace(inviteCode))
//             {
//                 TempData["ErrorMessage"] = "Vui lòng nhập mã mời.";
//                 return RedirectToAction("Index", "Home");
//             }

//             // Chuyển mã mời người dùng nhập về chữ IN HOA để so sánh
//             var normalizedInviteCode = inviteCode.Trim().ToUpper();

//             // Tìm phòng có mã mời tương ứng và còn hoạt động
//             //    Chúng ta cũng tìm bằng mã đã được chuẩn hóa
//             var room = await _context.WatchPartyRooms
//                                      .FirstOrDefaultAsync(r => r.InviteCode == normalizedInviteCode && r.IsActive);

//             if (room != null)
//             {
//                 //  Nếu phòng tồn tại, chuyển hướng người dùng đến phòng đó
//                 //    Sử dụng mã mời đã được chuẩn hóa để đảm bảo URL nhất quán
//                 return RedirectToAction("Room", new { inviteCode = room.InviteCode });
//             }
//             else
//             {
//                 // Nếu phòng không tồn tại, báo lỗi và quay về trang chủ
//                 TempData["ErrorMessage"] = "Mã mời không hợp lệ hoặc phòng đã kết thúc.";
//                 return RedirectToAction("Index", "Home");
//             }
//         }
//     }
// }


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System;
using System.Threading.Tasks;
using System.Linq; // Cần cho .Contains(), .ToUpper()

namespace MovieWebsite.Controllers
{
    public class WatchPartyController : Controller
    {
        private readonly AppDbContext _context;

        public WatchPartyController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Action được gọi từ nút "Xem Cùng Nhau" trên trang chi tiết phim.
        /// Tạo một phòng xem chung mới cho phim được chỉ định.
        /// </summary>
        /// <param name="movieId">ID của phim để tạo phòng.</param>
        /// <returns>Redirect đến trang phòng xem chung hoặc thông báo lỗi.</returns>
        [HttpGet]
        public async Task<IActionResult> CreateWatchParty(int movieId)
        {
            var movie = await _context.Movies
                                      .Include(m => m.Episodes) // Cần để lấy danh sách tập
                                      .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                // Nếu không tìm thấy phim, hiển thị thông báo lỗi hoặc redirect
                TempData["ErrorMessage"] = "Không tìm thấy phim để tạo phòng xem chung.";
                return RedirectToAction("Index", "Home"); // Hoặc về trang chi tiết phim nếu có
            }

            // Tạo ViewModel tùy chỉnh hoặc sử dụng ViewBag/ViewData
            // ViewModel sẽ là cách tốt hơn để truyền nhiều dữ liệu
            var viewModel = new CreateWatchPartyViewModel
            {
                Movie = movie,
                // Giá trị mặc định cho các trường
                RoomName = $"Cùng xem {movie.Title} nhé",
                SelectedPosterUrl = movie.PosterPath, // Giả sử có PosterUrl trong model Movie
                AutoStart = false,
                PrivateRoom = false // Mặc định là công khai
            };

            // Nếu Movie.PosterUrl không có, có thể gán mặc định hoặc chọn poster khác
            if (string.IsNullOrEmpty(viewModel.SelectedPosterUrl))
            {
                // Gán URL poster mặc định hoặc poster đầu tiên nếu có nhiều
                viewModel.AvailablePosters = new[] { "/images/default_poster.jpg" }; // Thay bằng đường dẫn thực
            }
            else
            {
                // Giả sử bạn có thể chọn giữa poster chính và một poster khác (ví dụ: thumbnail tập 1)
                viewModel.AvailablePosters = new[] { viewModel.SelectedPosterUrl, movie.Episodes?.FirstOrDefault()?.Title ?? viewModel.SelectedPosterUrl };
            }


            return View("~/Views/WatchParty/CreateWatchParty.cshtml", viewModel);
        }

        /// <summary>
        /// Action xử lý việc tạo phòng xem chung.
        /// </summary>
        /// <param name="viewModel">Dữ liệu từ form tạo phòng.</param>
        /// <returns>Redirect đến trang phòng xem phim hoặc hiển thị lỗi.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWatchPartyViewModel viewModel)
        {
            // Kiểm tra lại movie để đảm bảo nó vẫn tồn tại và hợp lệ
            var movie = await _context.Movies.FindAsync(viewModel.MovieId);
            if (movie == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phim để tạo phòng xem chung.";
                return RedirectToAction(nameof(CreateWatchParty), new { movieId = viewModel.MovieId });
            }

            // Tạo mã mời duy nhất
            var inviteCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var newRoom = new WatchPartyRoom
            {
                Name = viewModel.RoomName,
                MovieId = viewModel.MovieId,
                InviteCode = inviteCode,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                AutoStart = viewModel.AutoStart,
                Private = viewModel.PrivateRoom,
                SelectedPosterUrl = viewModel.SelectedPosterUrl // Lưu lại poster đã chọn
                // HostUserId = ... (nếu bạn có hệ thống user và muốn gán host)
            };

            try
            {
                _context.WatchPartyRooms.Add(newRoom);
                await _context.SaveChangesAsync();

                // Chuyển hướng đến trang phòng xem phim
                return RedirectToAction(nameof(Room), new { inviteCode = newRoom.InviteCode });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error creating watch party room: {ex.Message}");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo phòng xem chung. Vui lòng thử lại sau.";
                // Quay lại trang tạo phòng, có thể giữ lại các giá trị đã nhập nếu có thể
                return RedirectToAction(nameof(CreateWatchParty), new { movieId = viewModel.MovieId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error creating watch party room: {ex.Message}");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";
                return RedirectToAction(nameof(CreateWatchParty), new { movieId = viewModel.MovieId });
            }
        }

        /// <summary>
        /// Action hiển thị trang phòng xem phim.
        /// Định tuyến tùy chỉnh để URL có dạng: /watch/INVITEC0DE
        /// </summary>
        /// <param name="inviteCode">Mã mời của phòng.</param>
        /// <returns>View hiển thị chi tiết phòng xem phim hoặc thông báo lỗi.</returns>
        [HttpGet("watch/{inviteCode}")]
        public async Task<IActionResult> Room(string inviteCode)
        {
            // Kiểm tra xem inviteCode có hợp lệ không trước khi query CSDL
            if (string.IsNullOrWhiteSpace(inviteCode))
            {
                TempData["ErrorMessage"] = "Mã phòng xem phim không được để trống.";
                return RedirectToAction(nameof(EnterInviteCode)); // Chuyển về trang nhập mã
            }

            // Chuẩn hóa inviteCode để so sánh (viết hoa, bỏ khoảng trắng)
            var normalizedInviteCode = inviteCode.Trim().ToUpper();

            // Tìm phòng xem phim dựa trên mã mời và kiểm tra trạng thái hoạt động.
            // Sử dụng Include để tải thông tin phim kèm theo, bao gồm cả Episodes.
            var room = await _context.WatchPartyRooms
                                     .Include(r => r.Movie)
                                         .ThenInclude(m => m.Episodes) // Bao gồm Episodes của phim
                                     .FirstOrDefaultAsync(r => r.InviteCode == normalizedInviteCode && r.IsActive);

            // Kiểm tra xem phòng có được tìm thấy không.
            if (room == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy phòng xem phim với mã '{inviteCode}' hoặc phòng đã kết thúc.";
                return RedirectToAction(nameof(EnterInviteCode)); // Quay về trang nhập mã
            }

            // Gán ID phim vào ViewBag để có thể sử dụng trong View (ví dụ: cho các nút liên quan đến phim).
            ViewBag.CurrentMovieId = room.MovieId;

            // Truyền đối tượng room (đã bao gồm thông tin phim và tập) cho View.
            return View(room);
        }

        /// <summary>
        /// Action để hiển thị trang nhập mã mời tham gia phòng.
        /// </summary>
        /// <returns>View chứa form nhập mã mời.</returns>
        [HttpGet]
        public IActionResult EnterInviteCode()
        {
            // Trả về view EnterInviteCode.cshtml
            // Bạn có thể thêm logic để hiển thị thông báo lỗi nếu có từ TempData
            // Ví dụ: if (TempData["ErrorMessage"] != null) ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(); // Mặc định sẽ tìm View trong thư mục cùng tên với Controller
        }

        /// <summary>
        /// Xử lý khi người dùng submit form nhập mã mời trên trang EnterInviteCode.
        /// Tìm phòng và chuyển hướng đến trang phòng xem phim.
        /// </summary>
        /// <param name="inviteCode">Mã mời được nhập bởi người dùng.</param>
        /// <returns>Redirect đến trang phòng xem phim hoặc quay về trang nhập mã với lỗi.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(string inviteCode)
        {
            // Kiểm tra xem inviteCode có được nhập không.
            if (string.IsNullOrWhiteSpace(inviteCode))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập mã mời.";
                return RedirectToAction(nameof(EnterInviteCode));
            }

            // Chuẩn hóa mã mời người dùng nhập về chữ IN HOA để so sánh.
            var normalizedInviteCode = inviteCode.Trim().ToUpper();

            // Tìm phòng có mã mời tương ứng và còn hoạt động.
            var room = await _context.WatchPartyRooms
                                     .FirstOrDefaultAsync(r => r.InviteCode == normalizedInviteCode && r.IsActive);

            if (room != null)
            {
                // Nếu phòng tồn tại, chuyển hướng người dùng đến trang phòng đó.
                // Sử dụng mã mời đã chuẩn hóa để đảm bảo URL nhất quán.
                return RedirectToAction(nameof(Room), new { inviteCode = room.InviteCode });
            }
            else
            {
                // Nếu phòng không tồn tại, báo lỗi và quay về trang nhập mã.
                TempData["ErrorMessage"] = $"Mã mời '{inviteCode.Trim()}' không hợp lệ hoặc phòng đã kết thúc.";
                return RedirectToAction(nameof(EnterInviteCode));
            }
        }

        // --- Các Action khác của WatchParty (nếu có) ---
        // Ví dụ: Xử lý tin nhắn chat, cập nhật trạng thái phát, mời bạn bè, v.v.
        // [HttpPost]
        // public async Task<IActionResult> SendMessage(int roomId, string message) { ... }
    }
}