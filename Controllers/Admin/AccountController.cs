// // MovieWebsite/Controllers/AccountController.cs
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Identity; // Quan trọng: Sử dụng Identity
// using MovieWebsite.Models;         // Sử dụng AppUser của bạn


// namespace MovieWebsite.Controllers
// {
//     public class AccountController : Controller
//     {
//         // Tiêm UserManager và SignInManager từ ASP.NET Core Identity
//         private readonly UserManager<AppUser> _userManager;
//         private readonly SignInManager<AppUser> _signInManager;
//         private readonly ILogger<AccountController> _logger; // Để ghi log

//         // Constructor để khởi tạo các dịch vụ đã tiêm
//         public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
//         {
//             _userManager = userManager;
//             _signInManager = signInManager;
//             _logger = logger;
//         }

//         [HttpGet]
//         public IActionResult Register()
//         {
//             // Trả về View đăng ký khi yêu cầu GET
//             // ViewModel sẽ được tạo ra trên server
//             return View();
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken] // Bảo vệ chống tấn công CSRF
//         public async Task<IActionResult> Register(RegisterViewModel model)
//         {
//             // Kiểm tra xem ViewModel có hợp lệ không (dựa trên Data Annotations)
//             if (ModelState.IsValid)
//             {
//                 // Tạo một đối tượng AppUser mới từ ViewModel
//                 var user = new AppUser
//                 {
//                     UserName = model.Email, // Tên người dùng thường là Email
//                     Email = model.Email,
//                     // Bạn có thể gán thêm các thuộc tính khác của AppUser ở đây, ví dụ:
//                     FullName = model.FullName
//                 };

//                 // Tạo người dùng với mật khẩu trong ASP.NET Core Identity
//                 var result = await _userManager.CreateAsync(user, model.Password);

//                 if (result.Succeeded)
//                 {
//                     // Đăng nhập người dùng ngay sau khi đăng ký thành công (tùy chọn)
//                     // isPersistent: false nghĩa là cookie sẽ hết hạn khi trình duyệt đóng
//                     await _signInManager.SignInAsync(user, isPersistent: false);
//                     _logger.LogInformation($"User created a new account with password. UserId: {user.Id}");

//                     // Chuyển hướng đến trang chủ hoặc trang khác sau khi đăng ký và đăng nhập
//                     return RedirectToAction("Index", "Movies"); // Thay đổi controller/action nếu cần
//                 }

//                 // Nếu có lỗi trong quá trình tạo người dùng, thêm các lỗi vào ModelState
//                 foreach (var error in result.Errors)
//                 {
//                     ModelState.AddModelError(string.Empty, error.Description);
//                 }
//             }
//             // Nếu ModelState không hợp lệ, trả về lại View với các lỗi
//             return View(model);
//         }

//         [HttpGet]
//         public IActionResult Login()
//         {
//             // Trả về View đăng nhập khi yêu cầu GET
//             return View();
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Login(LoginViewModel model)
//         {
//             // Kiểm tra xem ViewModel có hợp lệ không
//             if (ModelState.IsValid)
//             {
//                 // Tìm người dùng theo Email hoặc UserName
//                 var user = await _userManager.FindByEmailAsync(model.Email) ?? await _userManager.FindByNameAsync(model.Email);

//                 if (user != null)
//                 {
//                     // Đăng nhập người dùng bằng mật khẩu
//                     // lockoutOnFailure: false nghĩa là không khóa tài khoản ngay lập tức nếu đăng nhập thất bại
//                     var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

//                     if (result.Succeeded)
//                     {
//                         _logger.LogInformation($"User logged in successfully. UserId: {user.Id}");
//                         // Chuyển hướng đến trang chủ hoặc trang trước đó
//                         return RedirectToAction("Index", "Movies"); // Thay đổi controller/action nếu cần
//                     }
//                     if (result.IsLockedOut)
//                     {
//                         // Nếu tài khoản bị khóa
//                         _logger.LogWarning($"User account locked out. UserId: {user.Id}");
//                         ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.");
//                     }
//                     else
//                     {
//                         // Các lỗi đăng nhập khác (sai mật khẩu, v.v.)
//                         ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
//                     }
//                 }
//                 else
//                 {
//                     ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
//                 }
//             }
//             // Nếu ModelState không hợp lệ, trả về lại View với các lỗi
//             return View(model);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Logout()
//         {
//             // Đăng xuất người dùng
//             await _signInManager.SignOutAsync();
//             _logger.LogInformation("User logged out.");

//             // Chuyển hướng về trang chủ hoặc trang đăng nhập
//             return RedirectToAction("Index", "Movies"); // Thay đổi controller/action nếu cần
//         }

//         [HttpGet]
//         public IActionResult AccessDenied()
//         {
//             // View cho trường hợp người dùng không có quyền truy cập
//             return View();
//         }

//         // Bạn có thể thêm các Action khác như ForgotPassword, ResetPassword, ConfirmEmail,...
//     }
// }


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MovieWebsite.Models;
using System.Threading.Tasks;
using System.Collections.Generic; // For List<>
using Microsoft.Extensions.Logging; // For ILogger
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using System.Linq;
using MovieWebsite.Data;
using Microsoft.EntityFrameworkCore; // Required for .Any()

namespace MovieWebsite.Controllers
{
    // Ensure this controller is secured for profile-related actions
    // Bảo vệ toàn bộ controller, yêu cầu đăng nhập
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context; // << THÊM: Inject DbContext

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            AppDbContext context) // << THÊM: Tham số context
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context; // << THÊM: Gán context
        }

        // --- Existing Authentication Actions ---

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation($"User created a new account with password. UserId: {user.Id}");
                    return RedirectToAction("Index", "Movies");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If the user is already logged in, redirect them to their profile or the movies page
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile", "Account"); // Or RedirectToAction("Index", "Movies");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? await _userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"User logged in successfully. UserId: {user.Id}");
                        return RedirectToAction("Profile", "Account"); // Redirect to profile after login
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning($"User account locked out. UserId: {user.Id}");
                        ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Movies");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // --- New Profile Actions ---

        [Authorize] // This page requires the user to be logged in
        // Action Profile cũng cần được cập nhật tương tự nếu bạn muốn hiển thị "Xem tiếp"
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // TODO: Lấy danh sách phim đang xem dở từ database
            var continueWatchingMovies = new List<Movie>(); // Giữ tạm placeholder

            var viewModel = new UserProfileViewModel
            {
                User = user,
                ContinueWatchingMovies = continueWatchingMovies,
                // Lấy cả danh sách yêu thích để hiển thị số lượng hoặc thông tin khác nếu cần
                FavoriteMovies = await _context.UserFavoriteMovies
                                       .Where(ufm => ufm.UserId == user.Id)
                                       .Select(ufm => ufm.Movie)
                                       .ToListAsync()
            };

            return View("Profile", viewModel);
        }

        // =====================================================================
        // === BẠN CẦN CẬP NHẬT ACTION NÀY ===
        // =====================================================================
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // [Authorize] đã kiểm tra, nhưng vẫn nên có để an toàn
                return Challenge(); // Hoặc RedirectToAction("Login")
            }

            // --- BỎ DỮ LIỆU MẪU VÀ LẤY DỮ LIỆU THẬT TỪ DATABASE ---

            // 1. Truy vấn bảng UserFavoriteMovies để lấy các phim mà user này đã thích
            // 2. Dùng Include() để tải thông tin chi tiết của phim (Movie)
            // 3. Dùng ThenInclude() để tải thông tin Thể loại (Genre) từ phim đó
            // 4. Dùng Select() để chỉ lấy ra đối tượng Movie
            var favoriteMovies = await _context.UserFavoriteMovies
                .Where(ufm => ufm.UserId == user.Id)
                .Include(ufm => ufm.Movie)         // Tải thông tin của Movie liên quan
                    .ThenInclude(movie => movie.Genre) // Từ Movie, tải tiếp Genre của nó
                .Select(ufm => ufm.Movie)          // Chỉ chọn ra đối tượng Movie
                .OrderByDescending(m => m.ReleaseYear) // Sắp xếp (tùy chọn)
                .ToListAsync();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                FavoriteMovies = favoriteMovies,
                // Bạn có thể giữ lại việc tải các danh sách khác nếu cần
                ContinueWatchingMovies = new List<Movie>() // Tạm thời để trống
            };

            // Dùng lại view "Profile" để hiển thị
            return View("Profile", viewModel);
        }
        
        // ... các action khác như AccountSettings ...
    

        [Authorize]
        public async Task<IActionResult> AccountSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // You might want a separate ViewModel for settings or reuse UserProfileViewModel
            var viewModel = new UserProfileViewModel
            {
                User = user,
                // Populate other properties if needed for settings page
                ContinueWatchingMovies = await GetPlaceholderContinueWatchingMovies() // Keeping sidebar data
            };

            // This action will render the AccountSettings.cshtml view
            return View(viewModel); // Assuming you will create Views/Account/AccountSettings.cshtml
        }
        
        public async Task<IActionResult> Notifications()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge(); // Hoặc RedirectToAction("Login")
        }

        // 1. Lấy tất cả thông báo của người dùng, sắp xếp mới nhất lên đầu
        var userNotifications = await _context.Notifications
            .Where(n => n.UserId == user.Id)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        // 2. Lấy ra các thông báo chưa đọc để cập nhật trạng thái
        var unreadNotifications = userNotifications.Where(n => !n.IsRead).ToList();
        if (unreadNotifications.Any())
        {
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }
            // Lưu lại thay đổi trạng thái "đã đọc" vào database
            await _context.SaveChangesAsync();
        }

        // 3. Chuẩn bị ViewModel để gửi sang View
        var viewModel = new UserProfileViewModel
        {
            User = user,
            UserNotifications = userNotifications // Gán danh sách thông báo đã lấy
            // Bạn có thể không cần tải các danh sách khác nếu không hiển thị chúng
        };

        // 4. Dùng lại view "Profile.cshtml" để hiển thị
        return View("Profile", viewModel);
    }


        // --- Helper method for placeholder data (optional) ---
        private async Task<List<Movie>> GetPlaceholderContinueWatchingMovies()
        {
            // Simulate async operation
            await Task.Delay(10);
            var movies = new List<Movie>();
            if (movies.Count == 0)
            {
                //  movies.Add(new Movie { Id = 1, Title = "Inception", PosterPath = "/images/inception_poster.jpg", ReleaseYear = 2010, Genre = new Genre { Name = "Sci-Fi" } });
                //  movies.Add(new Movie { Id = 2, Title = "The Dark Knight", PosterPath = "/images/dark_knight_poster.jpg", ReleaseYear = 2008, Genre = new Genre { Name = "Action" } });
            }
            return movies;
        }
    }
}