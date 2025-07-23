using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieWebsite.Models;

public class MovieViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
    public string Title { get; set; }

    [MaxLength(200, ErrorMessage = "Tiêu đề tiếng Anh không được vượt quá 200 ký tự")]
    public string EnglishTitle { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Năm phát hành là bắt buộc")]
    [Range(1900, 2100, ErrorMessage = "Năm phát hành phải từ 1900 đến 2100")]
    public int ReleaseYear { get; set; }

    [Required(ErrorMessage = "Quốc gia là bắt buộc")]
    public int CountryId { get; set; }
    public int GenreId { get; set; }

    // Giả định bạn muốn cho phép nhiều thể loại chính hoặc chỉ là một thể loại chính
    // Nếu chỉ một thể loại chính, giữ nguyên:
    // [Required(ErrorMessage = "Thể loại chính là bắt buộc")]
    // public int GenreId { get; set; }

    [MaxLength(500, ErrorMessage = "Tên đạo diễn không được vượt quá 500 ký tự")]
    public string Director { get; set; }

    [MaxLength(1000, ErrorMessage = "Danh sách diễn viên không được vượt quá 1000 ký tự")]
    public string Cast { get; set; }

    [Required(ErrorMessage = "Tổng số tập là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Tổng số tập phải lớn hơn 0")]
    public int TotalEpisodes { get; set; } = 1;

    public bool IsCompleted { get; set; } = false;

    // Dùng khi chỉnh sửa để hiển thị file hiện tại
    public string? PosterPath { get; set; }
    public string? TrailerPath { get; set; }

    // File mới cho poster và trailer (không bắt buộc khi chỉnh sửa)
    public IFormFile? PosterFile { get; set; }
    public IFormFile? TrailerFile { get; set; }

    [Display(Name = "Poster Dọc")]
    public string? PosterDoc { get; set; } // Để hiển thị đường dẫn ảnh đã có (khi edit)
    public IFormFile? PosterDocFile { get; set; } // Để nhận file mới tải lên từ form

    [Display(Name = "Poster Banner")]
    public string? PosterBanner { get; set; } // Để hiển thị đường dẫn ảnh đã có (khi edit)
    public IFormFile? PosterBannerFile { get; set; } // Để nhận file mới tải lên từ form

    [Display(Name = "Poster Phụ")]
    public string? Poster { get; set; } // Để hiển thị đường dẫn ảnh đã có (khi edit)
    public IFormFile? PosterImageFile { get; set; } // Để nhận file mới tải lên từ form (đặt tên khác để không trùng)

    // Danh sách các thể loại đã chọn của phim
    public List<int> SelectedGenreIds { get; set; } = new List<int>();

    // Danh sách các quốc gia và thể loại để hiển thị dropdown
    public List<Country> Countries { get; set; } = new List<Country>();
    public List<Genre> Genres { get; set; } = new List<Genre>();

    public List<ScheduleViewModelForAdmin> Schedules { get; set; } = new List<ScheduleViewModelForAdmin>();

    // Danh sách các tập phim hiện có để chỉnh sửa
    public List<EpisodeViewModel> ExistingEpisodes { get; set; } = new List<EpisodeViewModel>();

    // ViewModel để thêm tập phim mới trong quá trình chỉnh sửa
    // public EpisodeViewModel NewEpisode { get; set; } = new EpisodeViewModel();
    public List<EpisodeViewModel> NewEpisodesToAdd { get; set; } = new List<EpisodeViewModel>();

    // New property for episode uploads
    public List<EpisodeViewModel> Episodes { get; set; } = new List<EpisodeViewModel>();
}

// ViewModel cho MovieGenre
public class MovieGenreViewModel
{
    public List<Movie> Movies { get; set; }
    public SelectList Genres { get; set; }
    public string MovieGenre { get; set; }
    public string SearchString { get; set; }
}

public class EpisodeViewModel
{
    public int Id { get; set; } // ID của tập phim (quan trọng khi cập nhật)

    [Required(ErrorMessage = "Vui lòng chọn phim")]
    public int MovieId { get; set; } // Quan trọng cho các tập phim mới

    [Required(ErrorMessage = "Vui lòng nhập số tập")]
    [Range(1, int.MaxValue, ErrorMessage = "Số tập phải lớn hơn 0")]
    public int EpisodeNumber { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên tập")]
    [StringLength(200, ErrorMessage = "Tên tập không được vượt quá 200 ký tự")]
    public string Title { get; set; }

    [StringLength(1000, ErrorMessage = "Mô tả tập không được vượt quá 1000 ký tự")]
    public string? Description { get; set; } // Cho phép null

    // File mới cho video tập (không bắt buộc khi chỉnh sửa nếu file đã có)
    public IFormFile? VideoFile { get; set; }

    [Range(1, 600, ErrorMessage = "Thời lượng phải từ 1-600 phút")]
    public int Duration { get; set; } // Cho phép null

    public DateTime ReleaseDate { get; set; } // Cho phép null

    [Display(Name = "Tự động lên lịch chiếu?")]
    public bool CreateSchedule { get; set; } = false; // Checkbox để bật/tắt

    [Display(Name = "Thời gian chiếu dự kiến")]
    public DateTime? ScheduledTime { get; set; } // Thời gian chiếu

    // Dùng khi chỉnh sửa để hiển thị file hiện tại
    public string? VideoPath { get; set; }
    public string MovieTitle { get; set; } // Không cần [Required]
}

// ViewModel cho Rating
public class RatingViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn phim để đánh giá")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
    public string UserEmail { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn điểm đánh giá")]
    [Range(1, 10, ErrorMessage = "Điểm đánh giá phải từ 1-10")]
    public int Score { get; set; }

    [StringLength(500, ErrorMessage = "Nhận xét không được vượt quá 500 ký tự")]
    public string Review { get; set; }

    // For display purposes
    public string MovieTitle { get; set; }
}

// ViewModel cho Comment
public class CommentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phim")]
    public int MovieId { get; set; }

    public int? EpisodeId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string UserName { get; set; }

    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
    public string UserEmail { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận")]
    [StringLength(1000, ErrorMessage = "Bình luận không được vượt quá 1000 ký tự")]
    public string Content { get; set; }

    public int? ParentCommentId { get; set; }

    // For display purposes
    public string MovieTitle { get; set; }
    public string EpisodeTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
}

// ViewModel cho tìm kiếm và lọc
public class MovieSearchViewModel
{
    public string SearchTerm { get; set; }
    public int? CountryId { get; set; }
    public int? GenreId { get; set; }
    public int? Year { get; set; }
    public string SortBy { get; set; } = "newest"; // newest, oldest, rating, views
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    // For display
    public List<Movie> Movies { get; set; } = new List<Movie>();
    public List<Country> Countries { get; set; } = new List<Country>();
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

// ViewModel cho trang chi tiết phim
public class MovieDetailViewModel
{
    public Movie Movie { get; set; }
    public List<Episode> Episodes { get; set; } = new List<Episode>();
    public List<Rating> Ratings { get; set; } = new List<Rating>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Movie> RelatedMovies { get; set; } = new List<Movie>();

    // Form để thêm rating/comment mới
    public RatingViewModel NewRating { get; set; } = new RatingViewModel();
    public CommentViewModel NewComment { get; set; } = new CommentViewModel();
    public DateTime? NextEpisodeScheduleTime { get; set; }
    public int AiredEpisodesCount { get; set; } // This was also added in the previous example, good to keep.


    // --- Properties for Favorite Functionality ---
    public string UserId { get; set; } // The ID of the logged-in user
    public bool IsUserLoggedIn { get; set; } // Flag to check if user is logged in
    public bool HasFavorited { get; set; } // Flag to check if the current user has favorited this movie
    public List<Reel> Reels { get; set; } = new List<Reel>();

    public List<Episode> AiredEpisodes { get; set; } = new List<Episode>();
}

public class HomeViewModel1
{
    public List<Genre> PopularGenres { get; set; }
    public List<Movie> LatestMovies { get; set; }
    public List<Movie> TopRatedMovies { get; set; }

    // Các section khác nếu cần
    public List<Genre> Genres { get; set; } = new List<Genre>();
}


// // ViewModel chính cho trang Analytics
// public class AnalyticsViewModel
// {
//     // Dữ liệu cho các thẻ KPI
//     public int TotalMovies { get; set; }
//     public int TotalUsers { get; set; }
//     public long TotalViews { get; set; }
//     public long TotalPageViews { get; set; }

//     // Dữ liệu cho các biểu đồ
//     public ChartData NewUsersChart { get; set; }
//     public ChartData ViewsByGenreChart { get; set; }
//     public ChartData TopMoviesChart { get; set; }

//     public AnalyticsViewModel()
//     {
//         NewUsersChart = new ChartData();
//         ViewsByGenreChart = new ChartData();
//         TopMoviesChart = new ChartData();
//     }
// }

// // Lớp trợ giúp để lưu trữ dữ liệu cho một biểu đồ
// public class ChartData
// {
//     public List<string> Labels { get; set; } = new List<string>();
//     public List<long> Data { get; set; } = new List<long>();
// }


// // ViewModel dành riêng cho trang Dashboard
// public class DashboardViewModel
// {
//     public int MovieCount { get; set; }
//     public int GenreCount { get; set; }
//     public int CountryCount { get; set; }
//     public int EpisodeCount { get; set; }
//     public int UserCount { get; set; }
// }

// Lớp trợ giúp để lưu trữ dữ liệu cho một biểu đồ
// Đặt lớp này ngay trên DashboardViewModel
public class ChartData
{
    public List<string> Labels { get; set; } = new List<string>();
    public List<long> Data { get; set; } = new List<long>();
}

// ViewModel dành riêng cho trang Dashboard - PHIÊN BẢN NÂNG CẤP
public class DashboardViewModel
{
    // === CÁC THUỘC TÍNH CŨ CỦA BẠN (giữ nguyên) ===
    public int MovieCount { get; set; }
    public int GenreCount { get; set; }
    public int CountryCount { get; set; }
    public int EpisodeCount { get; set; } // Giữ lại nếu bạn có dùng
    public int UserCount { get; set; }

    // === CÁC THUỘC TÍNH MỚI TỪ ANALYTICSVIEWMODEL ===
    
    // Dữ liệu cho các thẻ KPI chi tiết
    public long TotalViews { get; set; }
    public long TotalPageViews { get; set; }

    // Dữ liệu cho các biểu đồ
    public ChartData NewUsersChart { get; set; }
    public ChartData ViewsByGenreChart { get; set; }
    public ChartData TopMoviesChart { get; set; }

    // Constructor để khởi tạo các đối tượng ChartData, tránh lỗi null
    public DashboardViewModel()
    {
        NewUsersChart = new ChartData();
        ViewsByGenreChart = new ChartData();
        TopMoviesChart = new ChartData();
    }
}


public class CreateWatchPartyViewModel
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } // Thông tin phim đầy đủ

    [Display(Name = "Tên phòng")]
    [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
    [StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự.")]
    public string RoomName { get; set; } // Phải khớp với Name trong WatchPartyRoom

    [Display(Name = "Tên hiển thị của bạn")]
    [Required(ErrorMessage = "Vui lòng nhập tên hiển thị của bạn.")]
    [StringLength(50, ErrorMessage = "Tên hiển thị không được quá 50 ký tự.")]
    public string HostUserName { get; set; }

    [Display(Name = "Poster hiển thị")]
    public string SelectedPosterUrl { get; set; } // Không bắt buộc nếu DB cho phép null

    public IEnumerable<string> AvailablePosters { get; set; } // Các tùy chọn poster

    [Display(Name = "Tự động bắt đầu")]
    public bool AutoStart { get; set; } // Phải khớp với AutoStart trong WatchPartyRoom

    [Display(Name = "Chỉ xem với bạn bè")]
    public bool PrivateRoom { get; set; } // Phải khớp với Private trong WatchPartyRoom
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên.")]
    [Display(Name = "Bí danh")] // Tên hiển thị trên form
    public string FullName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    [Display(Name = "Email")] // Tên hiển thị trên form
    public string Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [StringLength(
        100,
        ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và tối đa {1}.",
        MinimumLength = 6
    )]
    [DataType(DataType.Password)] // Hiển thị input password trên form
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Xác nhận mật khẩu")]
    [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")] // So sánh với Password
    public string ConfirmPassword { get; set; }

    // Bạn có thể thêm các trường khác của AppUser ở đây nếu muốn người dùng nhập vào lúc đăng ký
    // Ví dụ:
    // [Required(ErrorMessage = "Vui lòng nhập tên đầy đủ.")]
    // [Display(Name = "Tên đầy đủ")]
    // public string FullName { get; set; }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; }

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool RememberMe { get; set; }
}

public class CreateRoleViewModel
{
    [Required]
    [Display(Name = "Tên vai trò")]
    public string RoleName { get; set; }
}

public class RoleViewModel
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsSelected { get; set; }
}

public class ManageUserRolesViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public IList<string> UserRoles { get; set; } // Các role hiện tại của user
    public List<IdentityRole> AllRoles { get; set; } // Danh sách tất cả các role có sẵn
    public List<RoleViewModel> Roles { get; set; } // List để chọn/bỏ chọn role
}

public class RoleAssignmentViewModel
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsSelected { get; set; } // True nếu user đã có vai trò này
}

public class ManageUserRolesViewModel2
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<RoleAssignmentViewModel> RoleAssignments { get; set; } // Danh sách các vai trò có thể chọn
}

public class EditUserViewModel
{
    public string Id { get; set; } // Id của user

    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    [Display(Name = "Tên đăng nhập")]
    [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 ký tự.")]
    // [Remote(action: "IsUserNameUnique", controller: "Admin", ErrorMessage = "Tên đăng nhập đã tồn tại.")] // Nếu muốn validation remote
    public string UserName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    [Display(Name = "Email")]
    // [Remote(action: "IsEmailUnique", controller: "Admin", ErrorMessage = "Email đã tồn tại.")] // Nếu muốn validation remote
    public string Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string PhoneNumber { get; set; }

    // Thêm các thuộc tính tùy chỉnh của bạn ở đây
    [Display(Name = "Tên đầy đủ")]
    public string FullName { get; set; }

    // --- Phần đổi mật khẩu (Tùy chọn) ---
    public string CurrentPassword { get; set; }

    [StringLength(
        100,
        ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và tối đa {1}.",
        MinimumLength = 6
    )]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Xác nhận mật khẩu mới")]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
    public string ConfirmNewPassword { get; set; }
}

public class DeleteUserViewModel
{
    public string Id { get; set; }

    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    // Thông báo xác nhận xóa
    [Display(Name = "Bạn có chắc chắn muốn xóa người dùng này không?")]
    public bool ConfirmDelete { get; set; } = false;
}

public class MovieBannerViewModel
{
    public string PosterBanner { get; set; }
    public string Title { get; set; }
    public string EnglishTitle { get; set; }
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public int TotalEpisodes { get; set; } = 1;
    public string TrailerPath { get; set; }
    public string MainGenreName { get; set; }
    public List<string> GenreNames { get; set; } = new List<string>();
    public string CountryName { get; set; }
    public int MovieId { get; set; }
}

public class HomeViewModel
{
    public List<MovieBannerViewModel> BannerMovies { get; set; } = new List<MovieBannerViewModel>();
    public List<Genre> PopularGenres { get; set; }
    public List<Movie> LatestMovies { get; set; }
}


// public class ScheduleViewModel
// {
//     public DateTime SelectedDate { get; set; }
//     public List<DateTime> SelectableDates { get; set; } = new List<DateTime>();
//     public Dictionary<DateTime, List<ScheduleItemViewModel>> ScheduledGroups { get; set; } =
//         new Dictionary<DateTime, List<ScheduleItemViewModel>>();
//     public string ViewType { get; set; } = "today"; // today, week, upcoming
// }

// // ViewModel cho mỗi mục lịch chiếu
// public class ScheduleItemViewModel
// {
//     public int ScheduleId { get; set; }
//     public int MovieId { get; set; }
//     public string MovieTitle { get; set; }
//     public string PosterPath { get; set; }
//     public int? EpisodeNumber { get; set; }
//     public string EpisodeTitle { get; set; }
//     public string ItemTitle { get; set; } // Tiêu đề chung (tên tập hoặc tên phim)
//     public DateTime ScheduledTime { get; set; }
//     public string Description { get; set; }
//     public ScheduleType EntryType { get; set; }
//     public string? PosterDoc { get; internal set; }
// }

// public class ScheduleAdminItemViewModel
// {
//     public int Id { get; set; }
//     public DateTime ScheduledTime { get; set; }
//     public string MovieTitle { get; set; }
//     public string EpisodeInfo { get; set; } // Gộp thông tin tập phim vào 1 chuỗi
//     public string Description { get; set; }
//     public ScheduleType EntryType { get; set; }
// }

// // ViewModel cho quản lý lịch chiếu trong Admin
// public class ScheduleViewModelForAdmin
// {
//     public int Id { get; set; } // Dùng cho việc Edit, không ảnh hưởng đến Create

//     [Required(ErrorMessage = "Vui lòng chọn phim")]
//     public int MovieId { get; set; }

//     [Required(ErrorMessage = "Vui lòng chọn thời gian chiếu")]
//     public DateTime ScheduledTime { get; set; } = DateTime.Now.AddDays(1).Date.AddHours(20); // Mặc định 20:00 ngày mai

//     [MaxLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
//     public string Description { get; set; }

//     [Required(ErrorMessage = "Vui lòng chọn loại lịch chiếu")]
//     public ScheduleType EntryType { get; set; } = ScheduleType.EpisodeRelease;

//     // ----- CÁC TRƯỜNG MỚI ĐỂ TẠO TẬP PHIM -----
//     // Các trường này chỉ bắt buộc khi EntryType là EpisodeRelease

//     [Display(Name = "Số tập mới")]
//     public int? NewEpisodeNumber { get; set; }

//     [Display(Name = "Tên tập mới")]
//     [MaxLength(200)]
//     public string NewEpisodeTitle { get; set; }

//     public int? EpisodeId { get; set; }
//     public List<Episode> Episodes { get; set; } = new List<Episode>();

//     // ----- TRƯỜNG HỖ TRỢ HIỂN THỊ TRÊN FORM -----
//     public List<Movie> Movies { get; set; } = new List<Movie>();
// }

public class ScheduleViewModel
{
    public DateTime SelectedDate { get; set; }
    public List<DateTime> SelectableDates { get; set; } = new List<DateTime>();
    public Dictionary<DateTime, List<ScheduleItemViewModel>> ScheduledGroups { get; set; } = new Dictionary<DateTime, List<ScheduleItemViewModel>>();
    public string ViewType { get; set; } = "today"; // today, week, upcoming
}

// ViewModel cho mỗi mục lịch chiếu
public class ScheduleItemViewModel
{
    public int ScheduleId { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public string PosterPath { get; set; }
    public int? EpisodeNumber { get; set; }
    public string EpisodeTitle { get; set; }
    public string ItemTitle { get; set; } // Tiêu đề chung (tên tập hoặc tên phim)

    public bool IsEpisodePublished { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string Description { get; set; }
    public ScheduleType EntryType { get; set; }
    public string? PosterDoc { get; internal set; }
}
public class ScheduleAdminItemViewModel
{
    public int Id { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string MovieTitle { get; set; }
    public string EpisodeInfo { get; set; } // Gộp thông tin tập phim vào 1 chuỗi
    public string Description { get; set; }
    public ScheduleType EntryType { get; set; }
    
}
// ViewModel cho quản lý lịch chiếu trong Admin
public class ScheduleViewModelForAdmin
{
    public int Id { get; set; } // Dùng cho việc Edit, không ảnh hưởng đến Create

    [Required(ErrorMessage = "Vui lòng chọn phim")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thời gian chiếu")]
    public DateTime ScheduledTime { get; set; } = DateTime.Now.AddDays(1).Date.AddHours(20); // Mặc định 20:00 ngày mai

    [MaxLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại lịch chiếu")]
    public ScheduleType EntryType { get; set; } = ScheduleType.EpisodeRelease;

    // ----- CÁC TRƯỜNG MỚI ĐỂ TẠO TẬP PHIM -----
    // Các trường này chỉ bắt buộc khi EntryType là EpisodeRelease

    [Display(Name = "Số tập mới")]
    public int? NewEpisodeNumber { get; set; }

    [Display(Name = "Tên tập mới")]
    [MaxLength(200)]
    public string NewEpisodeTitle { get; set; }

    public int? EpisodeId { get; set; }
    public List<Episode> Episodes { get; set; } = new List<Episode>();

    [Display(Name = "Video tập phim mới")]
    public IFormFile? NewEpisodeVideoFile { get; set; }
    // Dùng IFormFile? (nullable) để nó không bắt buộc phải có khi tạo lịch cho phim lẻ.

    // ----- TRƯỜNG HỖ TRỢ HIỂN THỊ TRÊN FORM -----
    public List<Movie> Movies { get; set; } = new List<Movie>();


}

public class MoviesByCountryViewModel
{
    public Country SelectedCountry { get; set; }
    public List<Movie> Movies { get; set; } = new List<Movie>();
}

public class MoviesByGenreViewModel
{
    public Genre SelectedGenre { get; set; }
    public List<Movie> Movies { get; set; } = new List<Movie>();
}


public class TopMoviesByViewsViewModel
{
    public List<MovieWithViewCount> Movies { get; set; }
}

// Helper class to combine Movie with its view count
public class MovieWithViewCount
{
    public Movie Movie { get; set; }
    public int ViewCount { get; set; } // This will directly come from movie.Views
}


public class UserProfileViewModel
{
    public AppUser User { get; set; }

    // Lists to display in different sections of the profile page
    public List<Movie> ContinueWatchingMovies { get; set; } = new List<Movie>();
    public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
    public List<Movie> WatchlistMovies { get; set; } = new List<Movie>(); // Example for "Danh sách"

    // Add properties for other sections like Notifications, etc. if needed
    // public List<Notification> UserNotifications { get; set; } = new List<Notification>();
    public List<Notification> UserNotifications { get; set; } = new List<Notification>();

}

// Thêm vào file ViewModels của bạn
public class NotificationViewModel
{
    public int UnreadCount { get; set; }
    public List<Notification> RecentNotifications { get; set; } = new List<Notification>();
}

public class ProfileViewModel
{
    public string Email { get; set; }

    [Display(Name = "Tên hiển thị")]
    public string DisplayName { get; set; }

    [Display(Name = "Giới tính")]
    public string Gender { get; set; } // "Nam", "Nữ", "Không xác định"

    public string AvatarUrl { get; set; }

    // Dùng để upload ảnh đại diện mới
    public IFormFile NewAvatar { get; set; }
}


// Có thể thêm vào file ViewModels.cs của bạn
public class ReelViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
    [MaxLength(255)]
    public string Title { get; set; }

    // Dùng để hiển thị video hiện tại khi edit
    public string? ExistingVideoPath { get; set; }

    // Dùng để upload file video mới
    [Display(Name = "File Video (mp4, mov,...)")]
    public IFormFile? VideoFile { get; set; }

    public bool IsPublished { get; set; } = true;

    // Dùng cho dropdown list trên form
    public List<SelectListItem> MovieList { get; set; } = new List<SelectListItem>();
}

public class SeriesMoviesViewModel
{
    /// <summary>
    /// Tiêu đề của trang, ví dụ: "Danh sách Phim Bộ".
    /// </summary>
    public string PageTitle { get; set; }

    /// <summary>
    /// Một mô tả ngắn cho trang (tùy chọn).
    /// </summary>
    public string? PageDescription { get; set; }

    /// <summary>
    /// Danh sách các phim bộ được lấy từ cơ sở dữ liệu.
    /// </summary>
    public List<Movie> Movies { get; set; } = new List<Movie>();
}

public class FeatureMoviesViewModel
{
    /// <summary>
    /// Tiêu đề của trang, ví dụ: "Danh sách Phim Lẻ".
    /// </summary>
    public string PageTitle { get; set; }

    /// <summary>
    /// Một mô tả ngắn cho trang (tùy chọn).
    /// </summary>
    public string? PageDescription { get; set; }

    /// <summary>
    /// Danh sách các phim lẻ được lấy từ cơ sở dữ liệu.
    /// </summary>
    public List<Movie> Movies { get; set; } = new List<Movie>();
}

public class TopMoviesViewModel
{
    public string PageTitle { get; set; }
    public string PageDescription { get; set; }
    public List<Movie> TopMovies { get; set; } = new List<Movie>();
}

public class RankedMovieViewModel
{
    public int Rank { get; set; }
    public Movie Movie { get; set; }
    public int AiredEpisodesCount { get; set; } // Số tập đã phát sóng
}

/// <summary>
/// ViewModel chính cho toàn bộ khu vực "Top Phim".
/// </summary>
public class TopRankedSectionViewModel
{
    public string SectionTitle { get; set; }
    public List<RankedMovieViewModel> RankedMovies { get; set; } = new List<RankedMovieViewModel>();
}






// public class CreateWatchPartyViewModel
// {
//     public int MovieId { get; set; }
//     public Movie Movie { get; set; } // Thông tin phim đầy đủ

//     [Display(Name = "Tên phòng")]
//     [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
//     [StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự.")]
//     public string RoomName { get; set; } // Phải khớp với Name trong WatchPartyRoom

//     [Display(Name = "Poster hiển thị")]
//     public string SelectedPosterUrl { get; set; } // Không bắt buộc nếu DB cho phép null

//     public IEnumerable<string> AvailablePosters { get; set; } // Các tùy chọn poster

//     [Display(Name = "Tự động bắt đầu")]
//     public bool AutoStart { get; set; } // Phải khớp với AutoStart trong WatchPartyRoom

//     [Display(Name = "Chỉ xem với bạn bè")]
//     public bool PrivateRoom { get; set; } // Phải khớp với Private trong WatchPartyRoom
// }


// public class MovieBannerViewModel
// {
//     // --- Thông tin cơ bản của phim ---

//     // Poster phim (đường dẫn tới file ảnh)
//     public string PosterPath { get; set; }

//     // Tên phim (tiêu đề chính)
//     [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
//     [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
//     public string Title { get; set; }

//     // Tên phim tiếng Anh (tùy chọn)
//     [MaxLength(200, ErrorMessage = "Tiêu đề tiếng Anh không được vượt quá 200 ký tự")]
//     public string EnglishTitle { get; set; }

//     // Mô tả phim
//     [Required(ErrorMessage = "Mô tả là bắt buộc")]
//     public string Description { get; set; }

//     // Năm phát hành
//     [Required(ErrorMessage = "Năm phát hành là bắt buộc")]
//     [Range(1900, 2100, ErrorMessage = "Năm phát hành phải từ 1900 đến 2100")]
//     public int ReleaseYear { get; set; }

//     // Tổng số tập (hoặc phần, season tùy cách bạn định nghĩa)
//     [Required(ErrorMessage = "Tổng số tập là bắt buộc")]
//     [Range(1, int.MaxValue, ErrorMessage = "Tổng số tập phải lớn hơn 0")]
//     public int TotalEpisodes { get; set; } = 1;

//     // Đường dẫn tới trailer phim
//     public string TrailerPath { get; set; }

//     // --- Thông tin liên quan ---

//     // Thể loại phim (ví dụ: Action, Comedy, Drama...)
//     // Bạn có thể muốn hiển thị nhiều thể loại hoặc chỉ một thể loại chính.
//     // Dưới đây là ví dụ hiển thị tên thể loại chính.
//     // Nếu phim có nhiều thể loại, bạn có thể muốn một danh sách chuỗi (List<string>)
//     // hoặc một danh sách các đối tượng Genre.
//     public string MainGenreName { get; set; }

//     // Danh sách tên của tất cả các thể loại nếu phim có nhiều thể loại
//     public List<string> GenreNames { get; set; } = new List<string>();

//     // Quốc gia sản xuất (tùy chọn hiển thị)
//     public string CountryName { get; set; }

//     // ID của phim, hữu ích khi click vào banner để chuyển đến trang chi tiết phim
//     public int MovieId { get; set; }

//     // Các thuộc tính khác có thể hữu ích cho banner tùy thuộc vào thiết kế UI:
//     // public double AverageRating { get; set; }
//     // public int Views { get; set; }
// }

// public class CreateWatchPartyViewModel
// {
//     public int MovieId { get; set; }
//     public Movie Movie { get; set; } // Thông tin phim đầy đủ

//     // --- Các thuộc tính cho form tạo phòng ---
//     [Display(Name = "Tên phòng")]
//     [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
//     [StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự.")]
//     public string RoomName { get; set; } // Phải khớp với Name trong WatchPartyRoom

//     [Display(Name = "Poster hiển thị")]
//     // [Required(ErrorMessage = "Vui lòng chọn một poster hiển thị.")] // Nếu bắt buộc chọn
//     public string SelectedPosterUrl { get; set; } // Phải khớp với SelectedPosterUrl trong WatchPartyRoom
//     public IEnumerable<string> AvailablePosters { get; set; } // Các tùy chọn poster

//     // --- Cài đặt thời gian ---
//     [Display(Name = "Tự động bắt đầu")]
//     public bool AutoStart { get; set; } // Phải khớp với AutoStart trong WatchPartyRoom

//     // --- Quyền riêng tư ---
//     [Display(Name = "Chỉ xem với bạn bè")]
//     public bool PrivateRoom { get; set; } // Phải khớp với Private trong WatchPartyRoom (lưu ý tên khác nhau)

//     // --- Các thuộc tính khác có thể thêm ---
//     // public string HostUserId { get; set; }
//     // public int? MaxParticipants { get; set; }
// }

// ViewModel cho Episode
// public class EpisodeViewModel
// {
//     public int Id { get; set; }

//     [Required(ErrorMessage = "Vui lòng chọn phim")]
//     public int MovieId { get; set; }

//     [Required(ErrorMessage = "Vui lòng nhập số tập")]
//     [Range(1, int.MaxValue, ErrorMessage = "Số tập phải lớn hơn 0")]
//     public int EpisodeNumber { get; set; }

//     [Required(ErrorMessage = "Vui lòng nhập tên tập")]
//     [StringLength(200, ErrorMessage = "Tên tập không được vượt quá 200 ký tự")]
//     public string Title { get; set; }

//     [StringLength(1000, ErrorMessage = "Mô tả tập không được vượt quá 1000 ký tự")]
//     public string Description { get; set; }

//     [Required(ErrorMessage = "Vui lòng tải lên video")]
//     public IFormFile VideoFile { get; set; }

//     [Range(1, 600, ErrorMessage = "Thời lượng phải từ 1-600 phút")]
//     public int Duration { get; set; }

//     [Required(ErrorMessage = "Vui lòng chọn ngày phát hành")]
//     public DateTime ReleaseDate { get; set; } = DateTime.Now;

//     // For display purposes (not required)
//     public string? VideoPath { get; set; }
//     public string MovieTitle { get; set; } // Không cần [Required]
// }

// public class MovieViewModel
// {
//     public int Id { get; set; }

//     [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
//     [MaxLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
//     public string Title { get; set; }

//     [MaxLength(200, ErrorMessage = "Tiêu đề tiếng Anh không được vượt quá 200 ký tự")]
//     public string EnglishTitle { get; set; }

//     [Required(ErrorMessage = "Mô tả là bắt buộc")]
//     public string Description { get; set; }

//     [Required(ErrorMessage = "Năm phát hành là bắt buộc")]
//     [Range(1900, 2100, ErrorMessage = "Năm phát hành phải từ 1900 đến 2100")]
//     public int ReleaseYear { get; set; }

//     [Required(ErrorMessage = "Quốc gia là bắt buộc")]
//     public int CountryId { get; set; }

//     [Required(ErrorMessage = "Thể loại chính là bắt buộc")]
//     public int GenreId { get; set; }

//     [MaxLength(500, ErrorMessage = "Tên đạo diễn không được vượt quá 500 ký tự")]
//     public string Director { get; set; }

//     [MaxLength(1000, ErrorMessage = "Danh sách diễn viên không được vượt quá 1000 ký tự")]
//     public string Cast { get; set; }

//     [Required(ErrorMessage = "Tổng số tập là bắt buộc")]
//     [Range(1, int.MaxValue, ErrorMessage = "Tổng số tập phải lớn hơn 0")]
//     public int TotalEpisodes { get; set; } = 1;

//     public bool IsCompleted { get; set; } = false;

//     [Required(ErrorMessage = "Vui lòng chọn file poster")]

//     public IFormFile PosterFile { get; set; }

//     public IFormFile TrailerFile { get; set; }

//     public List<int> SelectedGenreIds { get; set; } = new List<int>();
//     public List<Country> Countries { get; set; } = new List<Country>();
//     public List<Genre> Genres { get; set; } = new List<Genre>();

//     public string? PosterPath { get; set; } // Dùng khi chỉnh sửa

//     public string? TrailerPath { get; set; } // Dùng khi chỉnh sửa

//     // New property for episode uploads
//     public List<EpisodeViewModel> Episodes { get; set; } = new List<EpisodeViewModel>();
//     public List<EpisodeViewModel> ExistingEpisodes { get; set; } = new List<EpisodeViewModel>();
//     public EpisodeViewModel NewEpisode { get; set; } = new EpisodeViewModel();
// }
