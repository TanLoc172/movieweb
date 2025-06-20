
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieWebsite.Models;

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

    // Danh sách các thể loại đã chọn của phim
    public List<int> SelectedGenreIds { get; set; } = new List<int>();

    // Danh sách các quốc gia và thể loại để hiển thị dropdown
    public List<Country> Countries { get; set; } = new List<Country>();
    public List<Genre> Genres { get; set; } = new List<Genre>();

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
    }

    
public class HomeViewModel1
{
    public List<Genre> PopularGenres { get; set; }
    public List<Movie> LatestMovies { get; set; }
    public List<Movie> TopRatedMovies { get; set; }
    // Các section khác nếu cần
    public List<Genre> Genres { get; set; } = new List<Genre>();
}

// ViewModel dành riêng cho trang Dashboard
    public class DashboardViewModel
    {
        public int MovieCount { get; set; }
        public int GenreCount { get; set; }
        public int CountryCount { get; set; }
        public int EpisodeCount { get; set; }
        public int UserCount { get; set; }
    }

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



    public class CreateWatchPartyViewModel
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } // Thông tin phim đầy đủ

        [Display(Name = "Tên phòng")]
        [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự.")]
        public string RoomName { get; set; } // Phải khớp với Name trong WatchPartyRoom

        [Display(Name = "Poster hiển thị")]
        public string SelectedPosterUrl { get; set; } // Không bắt buộc nếu DB cho phép null

        public IEnumerable<string> AvailablePosters { get; set; } // Các tùy chọn poster

        [Display(Name = "Tự động bắt đầu")]
        public bool AutoStart { get; set; } // Phải khớp với AutoStart trong WatchPartyRoom

        [Display(Name = "Chỉ xem với bạn bè")]
        public bool PrivateRoom { get; set; } // Phải khớp với Private trong WatchPartyRoom
    }



    