// File: Models/Notification.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace MovieWebsite.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // ID của người dùng nhận thông báo
        public AppUser User { get; set; }

        public int? MovieId { get; set; } // Phim liên quan (có thể null nếu là thông báo hệ thống)
        public Movie Movie { get; set; }

        public int? EpisodeId { get; set; } // Tập phim liên quan (nếu có)
        public Episode Episode { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } // Nội dung thông báo

        [Required]
        public string Url { get; set; } // Link để click vào (ví dụ: link xem tập phim mới)

        public bool IsRead { get; set; } = false; // Trạng thái đã đọc hay chưa

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}