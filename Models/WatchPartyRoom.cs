// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;

// namespace MovieWebsite.Models
// {
//     public class WatchPartyRoom
//     {
//         public int Id { get; set; }
//         public string Name { get; set; }
//         public string InviteCode { get; set; } // Một chuỗi ngẫu nhiên duy nhất
//         public int MovieId { get; set; }
//         public Movie Movie { get; set; }
//         public DateTime CreatedAt { get; set; } = DateTime.Now;
//         public bool IsActive { get; set; } = true;
//     }
// }


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Cần cho [ForeignKey] nếu có

namespace MovieWebsite.Models
{
    public class WatchPartyRoom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự.")]
        public string Name { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Mã mời phải có 8 ký tự.")]
        [RegularExpression("^[A-Z0-9]*$", ErrorMessage = "Mã mời chỉ được chứa ký tự chữ và số viết hoa.")]
        public string InviteCode { get; set; } // Một chuỗi ngẫu nhiên duy nhất

        // --- Liên kết với Movie ---
        public int MovieId { get; set; }
        [ForeignKey("MovieId")] // Đảm bảo ràng buộc khóa ngoại
        public Movie Movie { get; set; }

        // --- Các thuộc tính quản lý phòng ---
        public DateTime CreatedAt { get; set; } // Thời điểm tạo phòng

        public bool IsActive { get; set; } // Phòng còn hoạt động hay không

        // --- Các thuộc tính mới để lưu trữ lựa chọn từ form tạo phòng ---
        // Poster hiển thị khi tham gia phòng
        [StringLength(255)]
        public string SelectedPosterUrl { get; set; }

        // Liệu phòng có tự động bắt đầu khi có người tham gia đủ hoặc đến giờ không
        public bool AutoStart { get; set; }

        // Quyền riêng tư của phòng (chỉ bạn bè có link, hay công khai)
        public bool Private { get; set; } // Tên thuộc tính trong DB có thể là Private hoặc IsPrivate

        // --- Các thuộc tính khác có thể thêm ---
        // [StringLength(100)]
        // public string HostUserId { get; set; } // ID của người tạo phòng
        // public int? MaxParticipants { get; set; } // Số lượng người tối đa
        // public DateTime? ScheduledStartTime { get; set; } // Thời gian dự kiến bắt đầu
    }
}