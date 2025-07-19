using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Models
{
    public class UserFavoriteMovie
    {
        // [Key]
        // public int Id { get; set; }

        // [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        // [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;

        // Optional: You can add a composite key if you don't want the 'Id' column,
        // but using 'Id' is common practice.
        // [Index(IsUnique = true, Name = "IX_UserMovieFavorite")]
        // public string UserId { get; set; }
        // [Index(IsUnique = true, Name = "IX_UserMovieFavorite")]
        // public int MovieId { get; set; }
    }
}