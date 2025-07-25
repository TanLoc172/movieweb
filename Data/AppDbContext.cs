using Microsoft.EntityFrameworkCore;
using MovieWebsite.Models;
using MovieWebsite.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace MovieWebsite.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WatchPartyRoom> WatchPartyRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserFavoriteMovie> UserFavoriteMovies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
         public DbSet<Reel> Reels { get; set; }
         public DbSet<AnalyticsCounter> AnalyticsCounters { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship for Movie and Genre
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // Configure self-referencing for Comment (reply)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes for performance
            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Title);

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.ReleaseYear);

            modelBuilder.Entity<Episode>()
                .HasIndex(e => new { e.MovieId, e.EpisodeNumber })
                .IsUnique();


            modelBuilder.Entity<Schedule>()
                            .HasOne(s => s.Movie)
                            .WithMany(m => m.Schedules)
                            .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Episode)
                .WithMany(e => e.Schedules)
                .HasForeignKey(s => s.EpisodeId)
                .IsRequired(false); // EpisodeId có thể null

            // Ensure a user can only favorite a movie once
            modelBuilder.Entity<UserFavoriteMovie>()
            .HasKey(ufm => new { ufm.UserId, ufm.MovieId });

            // Configure the many-to-many relationship for Favorites
            modelBuilder.Entity<UserFavoriteMovie>()
                .HasOne(ufm => ufm.AppUser)
                .WithMany(u => u.FavoriteMovies)
                .HasForeignKey(ufm => ufm.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete from user to favorites

            modelBuilder.Entity<UserFavoriteMovie>()
                .HasOne(ufm => ufm.Movie)
                .WithMany(m => m.FavoritedByUsers)
                .HasForeignKey(ufm => ufm.MovieId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete from movie to favorites


            // Apply seed data
            SeedData.Seed(modelBuilder);
        }
    }
        
}