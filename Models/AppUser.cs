using Microsoft.AspNetCore.Identity;

namespace MovieWebsite.Models
{
    public class AppUser : IdentityUser
    {
        // Thêm các thuộc tính tùy chỉnh của bạn ở đây nếu cần
        // Ví dụ:
        // public string FirstName { get; set; }
        public string FullName { get; set; }
        public ICollection<UserFavoriteMovie> FavoriteMovies { get; set; } = new List<UserFavoriteMovie>();
    }
}