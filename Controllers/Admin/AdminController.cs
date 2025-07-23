using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MovieWebsite.Controllers
{
    // [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index2()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Action để tạo vai trò mới (chỉ Admin)
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem vai trò đã tồn tại chưa
                var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);
                if (!roleExist)
                {
                    var newRole = new IdentityRole { Name = model.RoleName };
                    var result = await _roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("CreateRole"); // Chuyển hướng về danh sách user
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vai trò đã tồn tại.");
                }
            }
            return View(model);
        }

        // Action để gán/thay đổi vai trò cho người dùng
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Người dùng không tồn tại
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var roleAssignments = allRoles.Select(role => new RoleAssignmentViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name) // Đặt IsSelected dựa trên vai trò hiện tại của user
            }).ToList();

            var viewModel = new ManageUserRolesViewModel2
            {
                UserId = user.Id,
                UserName = user.UserName,
                RoleAssignments = roleAssignments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel2 model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách các vai trò hiện tại của người dùng
            var currentUserRoles = await _userManager.GetRolesAsync(user);

            // Lấy các vai trò đã được chọn trong form (chỉ những cái có IsSelected = true)
            var selectedRoles = model.RoleAssignments.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();

            // Thêm các vai trò mới được chọn mà user chưa có
            var rolesToAdd = selectedRoles.Except(currentUserRoles).ToList();
            if (rolesToAdd.Any())
            {
                var addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addRolesResult.Succeeded)
                {
                    foreach (var error in addRolesResult.Errors) ModelState.AddModelError("", error.Description);
                    // Rất quan trọng: Nếu có lỗi, cần nạp lại dữ liệu cho view
                    return await LoadRoleAssignmentsForView(model);
                }
            }

            // Xóa các vai trò mà user đã có nhưng không còn được chọn trong form
            var rolesToRemove = currentUserRoles.Except(selectedRoles).ToList();
            if (rolesToRemove.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeRolesResult.Succeeded)
                {
                    foreach (var error in removeRolesResult.Errors) ModelState.AddModelError("", error.Description);
                    // Rất quan trọng: Nếu có lỗi, cần nạp lại dữ liệu cho view
                    return await LoadRoleAssignmentsForView(model);
                }
            }

            // Nếu mọi thứ thành công, chuyển hướng
            return RedirectToAction("Index2"); // Quay về danh sách người dùng
        }

        // Hàm trợ giúp để nạp lại dữ liệu cho view nếu có lỗi
        private async Task<IActionResult> LoadRoleAssignmentsForView(ManageUserRolesViewModel2 model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Rebuild the RoleAssignments list to correctly bind checkboxes
            model.RoleAssignments = allRoles.Select(role => new RoleAssignmentViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name) || model.RoleAssignments.Any(ra => ra.RoleId == role.Id && ra.IsSelected)
            }).ToList();

            return View(model); // Trả về view với dữ liệu đã được cập nhật
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // User không tồn tại
            }

            // Tạo ViewModel cho form edit
            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Gán thêm các thuộc tính tùy chỉnh của bạn nếu có
                FullName = user.FullName,
                // ...
            };

            return View(editUserViewModel);
        }

        // --- Action để xử lý yêu cầu chỉnh sửa thông tin user ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Cập nhật các thuộc tính của user
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                // Cập nhật thêm các thuộc tính tùy chỉnh nếu có
                // user.FullName = model.FullName;
                // ...

                // Cập nhật user trong UserManager
                var result = await _userManager.UpdateAsync(user);

                // Cập nhật email và username (nếu có thay đổi)
                // Nếu email hoặc username thay đổi, bạn có thể cần làm mới các Claim của user
                if (user.Email != model.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    // Có thể cần xử lý setEmailResult.Errors
                }
                if (user.UserName != model.UserName)
                {
                    var setUserNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                    // Có thể cần xử lý setUserNameResult.Errors
                }

                // Xử lý thay đổi mật khẩu riêng
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                        return View(model); // Trả về form với lỗi
                    }
                }


                if (result.Succeeded)
                {
                    // Có thể refresh lại Claims của user nếu Email/UserName thay đổi
                    await _signInManager.RefreshSignInAsync(user); // Nếu bạn muốn người dùng vẫn đăng nhập

                    return RedirectToAction("Index2"); // Quay về danh sách user
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            // Nếu ModelState không hợp lệ, trả về lại form edit
            return View(model);
        }

        // --- Action để xác nhận xóa user ---
        // Đây là một trang hiển thị thông báo trước khi xóa thực sự
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // ViewModel để hiển thị thông tin user cần xóa
            var deleteUserViewModel = new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(deleteUserViewModel);
        }

        // --- Action để xử lý yêu cầu xóa user ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Thực hiện xóa user
            var result = await _userManager.DeleteAsync(user);


            if (result.Succeeded)
            {
                // Nếu user đang đăng nhập là user bị xóa, hãy đăng xuất họ
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == user.Id)
                {
                    await _signInManager.SignOutAsync();
                }
                return RedirectToAction("Index2"); // Quay về danh sách user
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                // Nếu có lỗi, hiển thị lại trang confirm delete với lỗi
                return View(model);
            }
        }

        // public async Task<IActionResult> Index()
        // {
        //     // Tạo ViewModel để chứa dữ liệu thống kê
        //     var viewModel = new DashboardViewModel
        //     {
        //         MovieCount = await _context.Movies.CountAsync(),
        //         GenreCount = await _context.Genres.CountAsync(),
        //         CountryCount = await _context.Countries.CountAsync(),
        //         // Em có thể thêm các thống kê khác ở đây, ví dụ:
        //         // EpisodeCount = await _context.Episodes.CountAsync(),

        //         // UserCount = ...
        //     };

        //     // Truyền ViewModel ra View
        //     return View(viewModel);
        // }
        
        // Trong Controllers/AdminController.cs

public async Task<IActionResult> Index()
{
    // Tạo ViewModel để chứa dữ liệu thống kê
    var viewModel = new DashboardViewModel
    {
        // 1. Lấy dữ liệu cho các thẻ KPI (sử dụng các thuộc tính trong ViewModel mới)
        MovieCount = await _context.Movies.CountAsync(),
        GenreCount = await _context.Genres.CountAsync(),
        CountryCount = await _context.Countries.CountAsync(),
        UserCount = await _userManager.Users.CountAsync(),
        EpisodeCount = await _context.Episodes.CountAsync(), // Tính tổng số tập
        
        // Thêm các truy vấn mới cho KPI
        TotalViews = await _context.Movies.SumAsync(m => (long)m.Views),
        TotalPageViews = (await _context.AnalyticsCounters
                            .FirstOrDefaultAsync(c => c.CounterName == "TotalPageViews"))?.CountValue ?? 0
    };

    // 2. Lấy dữ liệu cho Biểu đồ Người dùng mới (30 ngày gần nhất)
    var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
    var newUsersData = await _context.Users
        .Where(u => u.RegistrationDate >= thirtyDaysAgo)
        .GroupBy(u => u.RegistrationDate.Date)
        .Select(g => new { Date = g.Key, Count = g.Count() })
        .OrderBy(x => x.Date)
        .ToListAsync();
    
    viewModel.NewUsersChart.Labels.AddRange(newUsersData.Select(d => d.Date.ToString("dd/MM")));
    viewModel.NewUsersChart.Data.AddRange(newUsersData.Select(d => (long)d.Count));

    // 3. Lấy dữ liệu cho Biểu đồ Phân bổ Lượt xem theo Thể loại
    var viewsByGenreData = await _context.Movies
        .Where(m => m.Genre != null)
        .GroupBy(m => m.Genre.Name)
        .Select(g => new { GenreName = g.Key, TotalViews = g.Sum(m => (long)m.Views) })
        .OrderByDescending(x => x.TotalViews)
        .Take(7)
        .ToListAsync();

    viewModel.ViewsByGenreChart.Labels.AddRange(viewsByGenreData.Select(d => d.GenreName));
    viewModel.ViewsByGenreChart.Data.AddRange(viewsByGenreData.Select(d => d.TotalViews));

    // 4. Lấy dữ liệu cho Biểu đồ Top 10 Phim
    var topMoviesData = await _context.Movies
        .OrderByDescending(m => m.Views)
        .Take(10)
        .Select(m => new { m.Title, Views = (long)m.Views })
        .ToListAsync();

    viewModel.TopMoviesChart.Labels.AddRange(topMoviesData.Select(m => m.Title));
    viewModel.TopMoviesChart.Data.AddRange(topMoviesData.Select(m => m.Views));

    // Truyền ViewModel đã đầy đủ dữ liệu ra View
    return View(viewModel);
}


    

    // [HttpGet]
        //     public async Task<IActionResult> Analytics()
        //     {
        //         var viewModel = new AnalyticsViewModel();

        //         // 1. Lấy dữ liệu cho các thẻ KPI
        //         viewModel.TotalMovies = await _context.Movies.CountAsync();
        //         viewModel.TotalUsers = await _userManager.Users.CountAsync();
        //         viewModel.TotalViews = await _context.Movies.SumAsync(m => (long)m.Views);
        //         var pageViewCounter = await _context.AnalyticsCounters.FirstOrDefaultAsync(c => c.CounterName == "TotalPageViews");
        //         viewModel.TotalPageViews = pageViewCounter?.CountValue ?? 0;

        //         // 2. Dữ liệu cho biểu đồ Người dùng mới (30 ngày gần nhất)
        //         var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        //         var newUsersData = await _context.Users
        //             .Where(u => u.RegistrationDate >= thirtyDaysAgo)
        //             .GroupBy(u => u.RegistrationDate.Date)
        //             .Select(g => new { Date = g.Key, Count = g.Count() })
        //             .OrderBy(x => x.Date)
        //             .ToListAsync();

        //         viewModel.NewUsersChart.Labels = newUsersData.Select(d => d.Date.ToString("dd/MM")).ToList();
        //         viewModel.NewUsersChart.Data = newUsersData.Select(d => (long)d.Count).ToList();


        //         // 3. Dữ liệu cho biểu đồ Lượt xem theo Thể loại
        //         var viewsByGenreData = await _context.Movies
        //             .Include(m => m.Genre)
        //             .GroupBy(m => m.Genre.Name)
        //             .Select(g => new
        //             {
        //                 GenreName = g.Key,
        //                 TotalViews = g.Sum(m => (long)m.Views)
        //             })
        //             .OrderByDescending(x => x.TotalViews)
        //             .Take(7) // Lấy 7 thể loại phổ biến nhất
        //             .ToListAsync();

        //         viewModel.ViewsByGenreChart.Labels = viewsByGenreData.Select(d => d.GenreName).ToList();
        //         viewModel.ViewsByGenreChart.Data = viewsByGenreData.Select(d => d.TotalViews).ToList();


        //         // 4. Dữ liệu cho biểu đồ Top 10 Phim
        //         var topMoviesData = await _context.Movies
        //             .OrderByDescending(m => m.Views)
        //             .Take(10)
        //             .Select(m => new { m.Title, Views = (long)m.Views })
        //             .ToListAsync();

        //         viewModel.TopMoviesChart.Labels = topMoviesData.Select(m => m.Title).ToList();
        //         viewModel.TopMoviesChart.Data = topMoviesData.Select(m => m.Views).ToList();


        //         return View(viewModel);
        //     }
    }
}


