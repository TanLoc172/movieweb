
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieWebsite.Models
{
    // Enum để xác định loại lịch chiếu
    public enum ScheduleType
    {
        EpisodeRelease, // Lịch ra mắt tập mới
        MoviePremiere,  // Lịch công chiếu phim lẻ
        SpecialEvent    // Sự kiện đặc biệt, trailer...
    }

    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; } // Ngày giờ chiếu chính xác

        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        // Có thể null, vì lịch chiếu có thể cho cả phim (phim lẻ)
        public int? EpisodeId { get; set; }
        public Episode Episode { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } // Ví dụ: "Tập cuối", "Bản đặc biệt"

        public ScheduleType EntryType { get; set; } = ScheduleType.EpisodeRelease;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}