// File: ViewComponents/NotificationViewComponent.cs

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;

namespace MovieWebsite.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public NotificationViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content(""); // Không hiển thị gì nếu user chưa đăng nhập
            }

            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(10) // Lấy 10 thông báo gần nhất
                .ToListAsync();

            var unreadCount = await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);

            var viewModel = new NotificationViewModel
            {
                RecentNotifications = notifications,
                UnreadCount = unreadCount
            };

            return View(viewModel); // Sẽ gọi view Default.cshtml
        }
    }
}