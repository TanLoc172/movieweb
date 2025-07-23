// Trong Models/AnalyticsCounter.cs
using System.ComponentModel.DataAnnotations;

namespace MovieWebsite.Models
{
    public class AnalyticsCounter
    {
        [Key]
        [MaxLength(50)]
        public string CounterName { get; set; } // Ví dụ: "TotalPageViews"

        public long CountValue { get; set; }
    }
}