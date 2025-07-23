using Microsoft.AspNetCore.Identity;

namespace MovieWebsite.Models
{
    public class AppUser : IdentityUser
    {
        // Thêm các thuộc tính tùy chỉnh của bạn ở đây nếu cần
        // Ví dụ:
        // public string FirstName { get; set; }
        public string FullName { get; set; }
        // THÊM DÒNG NÀY:
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // Ghi lại thời gian đăng ký
        public ICollection<UserFavoriteMovie> FavoriteMovies { get; set; } = new List<UserFavoriteMovie>();
    }
}