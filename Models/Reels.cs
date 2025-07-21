using MovieWebsite.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Models
{
    public class Reel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } // Tiêu đề hoặc mô tả ngắn cho Reel

        [Required]
        public string VideoPath { get; set; } // Đường dẫn tới file video

        public string? ThumbnailPath { get; set; } // (Tùy chọn) Đường dẫn ảnh thumbnail

        // --- Foreign Key đến người tạo (Admin) ---
        [Required]
        public string UploaderId { get; set; }
        [ForeignKey("UploaderId")]
        public AppUser Uploader { get; set; }

        public int Views { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public bool IsPublished { get; set; } = true; // Admin có thể ẩn/hiện

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}