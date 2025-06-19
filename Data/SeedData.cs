using Microsoft.EntityFrameworkCore;
using MovieWebsite.Models;

namespace MovieWebsite.Data
{
    public static class SeedData
    {
        // public static void Seed(ModelBuilder modelBuilder)
        // {
        //     // Seed Countries
        //     modelBuilder.Entity<Country>().HasData(
        //         new Country { Id = 1, Name = "Việt Nam", Code = "VN" },
        //         new Country { Id = 2, Name = "Hàn Quốc", Code = "KR" },
        //         new Country { Id = 3, Name = "Trung Quốc", Code = "CN" },
        //         new Country { Id = 4, Name = "Nhật Bản", Code = "JP" },
        //         new Country { Id = 5, Name = "Mỹ", Code = "US" },
        //         new Country { Id = 6, Name = "Thái Lan", Code = "TH" }
        //     );

        //     // Seed Genres
        //     modelBuilder.Entity<Genre>().HasData(
        //         new Genre { Id = 1, Name = "Hành Động", Color = "#FF5722" },
        //         new Genre { Id = 2, Name = "Tình Cảm", Color = "#E91E63" },
        //         new Genre { Id = 3, Name = "Hài Hước", Color = "#FFC107" },
        //         new Genre { Id = 4, Name = "Kinh Dị", Color = "#424242" },
        //         new Genre { Id = 5, Name = "Khoa Học Viễn Tưởng", Color = "#2196F3" },
        //         new Genre { Id = 6, Name = "Phiêu Lưu", Color = "#4CAF50" },
        //         new Genre { Id = 7, Name = "Chính Kịch", Color = "#9C27B0" },
        //         new Genre { Id = 8, Name = "Hoạt Hình", Color = "#FF9800" }
        //     );

        //     // Seed Movies
        //     modelBuilder.Entity<Movie>().HasData(
        //         new Movie
        //         {
        //             Id = 1,
        //             Title = "Bố Già",
        //             EnglishTitle = "Old Father",
        //             Description = "Bộ phim kể về cuộc sống của một gia đình lao động nghèo ở Sài Gòn.",
        //             ReleaseYear = 2021,
        //             CountryId = 1,
        //             GenreId = 7,
        //             Director = "Trấn Thành",
        //             Cast = "Trấn Thành, Tuấn Trần, Ngô Kiến Huy",
        //             PosterPath = "/images/1.png",
        //             TrailerPath = "/videos/1.mp4",
        //             TotalEpisodes = 1,
        //             IsCompleted = true,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now,
        //             Views = 150000,
        //             AverageRating = 8.5,
        //             RatingCount = 1200
        //         },
        //         new Movie
        //         {
        //             Id = 2,
        //             Title = "Hạ Cánh Nơi Anh",
        //             EnglishTitle = "Crash Landing on You",
        //             Description = "Câu chuyện tình yêu giữa một nữ thừa kế Hàn Quốc và một sĩ quan Bắc Triều Tiên.",
        //             ReleaseYear = 2019,
        //             CountryId = 2,
        //             GenreId = 2,
        //             Director = "Lee Jeong-hyo",
        //             Cast = "Hyun Bin, Son Ye-jin",
        //             PosterPath = "/images/2.png",
        //             TrailerPath = "/videos/2.mp4",
        //             TotalEpisodes = 16,
        //             IsCompleted = true,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now,
        //             Views = 250000,
        //             AverageRating = 9.0,
        //             RatingCount = 2000
        //         },
        //         new Movie
        //         {
        //             Id = 3,
        //             Title = "Ký Sinh Trùng",
        //             EnglishTitle = "Parasite",
        //             Description = "Một gia đình nghèo tìm cách thâm nhập vào cuộc sống của một gia đình giàu có.",
        //             ReleaseYear = 2019,
        //             CountryId = 2,
        //             GenreId = 7,
        //             Director = "Bong Joon-ho",
        //             Cast = "Song Kang-ho, Lee Sun-kyun, Cho Yeo-jeong",
        //             PosterPath = "/images/3.png",
        //             TrailerPath = "/videos/3.mp4",
        //             TotalEpisodes = 1,
        //             IsCompleted = true,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now,
        //             Views = 300000,
        //             AverageRating = 9.2,
        //             RatingCount = 2500
        //         }
        //     );

        //     // Seed MovieGenres
        //     modelBuilder.Entity<MovieGenre>().HasData(
        //         new MovieGenre { MovieId = 1, GenreId = 7 },
        //         new MovieGenre { MovieId = 1, GenreId = 3 },
        //         new MovieGenre { MovieId = 2, GenreId = 2 },
        //         new MovieGenre { MovieId = 2, GenreId = 3 },
        //         new MovieGenre { MovieId = 3, GenreId = 7 },
        //         new MovieGenre { MovieId = 3, GenreId = 4 }
        //     );

        //     // Seed Episodes
        //     modelBuilder.Entity<Episode>().HasData(
        //         new Episode
        //         {
        //             Id = 1,
        //             MovieId = 2,
        //             EpisodeNumber = 1,
        //             Title = "Tập 1: Cuộc gặp gỡ định mệnh",
        //             Description = "Yoon Se-ri vô tình hạ cánh xuống Bắc Triều Tiên sau một tai nạn.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 70,
        //             ReleaseDate = new DateTime(2019, 12, 14),
        //             Views = 10000,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         },
        //         new Episode
        //         {
        //             Id = 2,
        //             MovieId = 2,
        //             EpisodeNumber = 2,
        //             Title = "Tập 2: Kế hoạch trốn thoát",
        //             Description = "Ri Jeong-hyeok giúp Se-ri tìm cách trở về Hàn Quốc.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 65,
        //             ReleaseDate = new DateTime(2019, 12, 15),
        //             Views = 9500,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         },
        //         new Episode
        //         {
        //             Id = 3,
        //             MovieId = 2,
        //             EpisodeNumber = 3,
        //             Title = "Tập 3: Bí mật bị hé lộ",
        //             Description = "Se-ri dần thích nghi với cuộc sống ở Bắc Triều Tiên.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 68,
        //             ReleaseDate = new DateTime(2019, 12, 21),
        //             Views = 9000,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         },
        //         new Episode
        //         {
        //             Id = 4,
        //             MovieId = 2,
        //             EpisodeNumber = 4,
        //             Title = "Tập 4: Tình cảm nảy nở",
        //             Description = "Tình cảm giữa Se-ri và Jeong-hyeok bắt đầu phát triển.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 72,
        //             ReleaseDate = new DateTime(2019, 12, 22),
        //             Views = 8800,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         },
        //         new Episode
        //         {
        //             Id = 5,
        //             MovieId = 2,
        //             EpisodeNumber = 5,
        //             Title = "Tập 5: Thách thức mới",
        //             Description = "Se-ri đối mặt với những nguy hiểm mới ở Bắc Triều Tiên.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 70,
        //             ReleaseDate = new DateTime(2019, 12, 28),
        //             Views = 8500,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         },
        //         new Episode
        //         {
        //             Id = 6,
        //             MovieId = 2,
        //             EpisodeNumber = 6,
        //             Title = "Tập 6: Hành trình nguy hiểm",
        //             Description = "Jeong-hyeok lên kế hoạch bảo vệ Se-ri.",
        //             VideoPath = "/videos/1.mp4",
        //             Duration = 67,
        //             ReleaseDate = new DateTime(2019, 12, 29),
        //             Views = 8200,
        //             CreatedAt = DateTime.Now,
        //             UpdatedAt = DateTime.Now
        //         }
        //     );

        //     // Seed Ratings
        //     modelBuilder.Entity<Rating>().HasData(
        //         new Rating
        //         {
        //             Id = 1,
        //             MovieId = 1,
        //             UserName = "NguyenVanA",
        //             UserEmail = "nguyenvana@example.com",
        //             Score = 8,
        //             Review = "Phim rất cảm động, diễn xuất của Trấn Thành tuyệt vời!",
        //             CreatedAt = DateTime.Now
        //         },
        //         new Rating
        //         {
        //             Id = 2,
        //             MovieId = 1,
        //             UserName = "TranThiB",
        //             UserEmail = "tranthib@example.com",
        //             Score = 9,
        //             Review = "Cốt truyện gần gũi, phản ánh đúng cuộc sống.",
        //             CreatedAt = DateTime.Now
        //         },
        //         new Rating
        //         {
        //             Id = 3,
        //             MovieId = 3,
        //             UserName = "LeVanC",
        //             UserEmail = "levanc@example.com",
        //             Score = 10,
        //             Review = "Một kiệt tác của điện ảnh Hàn Quốc!",
        //             CreatedAt = DateTime.Now
        //         }
        //     );

        //     // Seed Comments
        //     modelBuilder.Entity<Comment>().HasData(
        //         new Comment
        //         {
        //             Id = 1,
        //             MovieId = 1,
        //             EpisodeId = null,
        //             UserName = "NguyenVanA",
        //             UserEmail = "nguyenvana@example.com",
        //             Content = "Phim này thực sự làm mình khóc rất nhiều!",
        //             Likes = 50,
        //             Dislikes = 2,
        //             CreatedAt = DateTime.Now
        //         },
        //         new Comment
        //         {
        //             Id = 2,
        //             MovieId = 1,
        //             EpisodeId = null,
        //             UserName = "TranThiB",
        //             UserEmail = "tranthib@example.com",
        //             Content = "Cảm ơn Trấn Thành đã mang đến một bộ phim ý nghĩa!",
        //             ParentCommentId = 1,
        //             Likes = 30,
        //             Dislikes = 1,
        //             CreatedAt = DateTime.Now
        //         },
        //         new Comment
        //         {
        //             Id = 3,
        //             MovieId = 2,
        //             EpisodeId = 1,
        //             UserName = "LeVanC",
        //             UserEmail = "levanc@example.com",
        //             Content = "Tập 1 rất hấp dẫn, chemistry giữa hai diễn viên chính đỉnh cao!",
        //             Likes = 40,
        //             Dislikes = 0,
        //             CreatedAt = DateTime.Now
        //         },
        //         new Comment
        //         {
        //             Id = 4,
        //             MovieId = 2,
        //             EpisodeId = 2,
        //             UserName = "PhamThiD",
        //             UserEmail = "phamthid@example.com",
        //             Content = "Tập 2 càng cuốn, không thể rời mắt!",
        //             Likes = 35,
        //             Dislikes = 1,
        //             CreatedAt = DateTime.Now
        //         }
        //     );
        // }


        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed Countries (Giữ nguyên)
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Việt Nam", Code = "VN" },
                new Country { Id = 2, Name = "Hàn Quốc", Code = "KR" },
                new Country { Id = 3, Name = "Trung Quốc", Code = "CN" },
                new Country { Id = 4, Name = "Nhật Bản", Code = "JP" },
                new Country { Id = 5, Name = "Mỹ", Code = "US" },
                new Country { Id = 6, Name = "Thái Lan", Code = "TH" },
                new Country { Id = 7, Name = "Anh", Code = "UK" },
                new Country { Id = 8, Name = "Pháp", Code = "FR" },
                new Country { Id = 9, Name = "Canada", Code = "CA" }
            );

            // Seed Genres (Giữ nguyên)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Hành Động", Color = "#FF5722" },
                new Genre { Id = 2, Name = "Tình Cảm", Color = "#E91E63" },
                new Genre { Id = 3, Name = "Hài Hước", Color = "#FFC107" },
                new Genre { Id = 4, Name = "Kinh Dị", Color = "#424242" },
                new Genre { Id = 5, Name = "Khoa Học Viễn Tưởng", Color = "#2196F3" },
                new Genre { Id = 6, Name = "Phiêu Lưu", Color = "#4CAF50" },
                new Genre { Id = 7, Name = "Chính Kịch", Color = "#9C27B0" },
                new Genre { Id = 8, Name = "Hoạt Hình", Color = "#FF9800" },
                new Genre { Id = 9, Name = "Tâm Lý", Color = "#00BCD4" },
                new Genre { Id = 10, Name = "Kinh Dị Gia Đình", Color = "#795548" }
            );

            // Seed Movies (Original) - Giữ nguyên
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Title = "Bố Già",
                    EnglishTitle = "Old Father",
                    Description = "Bộ phim kể về cuộc sống của một gia đình lao động nghèo ở Sài Gòn.",
                    ReleaseYear = 2021,
                    CountryId = 1,
                    GenreId = 7,
                    Director = "Trấn Thành",
                    Cast = "Trấn Thành, Tuấn Trần, Ngô Kiến Huy",
                    PosterPath = "/images/1.png",
                    TrailerPath = "/videos/1.mp4",
                    TotalEpisodes = 1,
                    IsCompleted = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Views = 150000,
                    AverageRating = 8.5,
                    RatingCount = 1200
                },
                new Movie
                {
                    Id = 2,
                    Title = "Hạ Cánh Nơi Anh",
                    EnglishTitle = "Crash Landing on You",
                    Description = "Câu chuyện tình yêu giữa một nữ thừa kế Hàn Quốc và một sĩ quan Bắc Triều Tiên.",
                    ReleaseYear = 2019,
                    CountryId = 2,
                    GenreId = 2,
                    Director = "Lee Jeong-hyo",
                    Cast = "Hyun Bin, Son Ye-jin",
                    PosterPath = "/images/2.png",
                    TrailerPath = "/videos/2.mp4",
                    TotalEpisodes = 16,
                    IsCompleted = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Views = 250000,
                    AverageRating = 9.0,
                    RatingCount = 2000
                },
                new Movie
                {
                    Id = 3,
                    Title = "Ký Sinh Trùng",
                    EnglishTitle = "Parasite",
                    Description = "Một gia đình nghèo tìm cách thâm nhập vào cuộc sống của một gia đình giàu có.",
                    ReleaseYear = 2019,
                    CountryId = 2,
                    GenreId = 7,
                    Director = "Bong Joon-ho",
                    Cast = "Song Kang-ho, Lee Sun-kyun, Cho Yeo-jeong",
                    PosterPath = "/images/3.png",
                    TrailerPath = "/videos/3.mp4",
                    TotalEpisodes = 1,
                    IsCompleted = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Views = 300000,
                    AverageRating = 9.2,
                    RatingCount = 2500
                }
            );

            // Seed MovieGenres (Original) - Giữ nguyên
            modelBuilder.Entity<MovieGenre>().HasData(
                new MovieGenre { MovieId = 1, GenreId = 7 },
                new MovieGenre { MovieId = 1, GenreId = 3 },
                new MovieGenre { MovieId = 2, GenreId = 2 },
                new MovieGenre { MovieId = 2, GenreId = 3 },
                new MovieGenre { MovieId = 3, GenreId = 7 },
                new MovieGenre { MovieId = 3, GenreId = 4 }
            );

            // Seed Episodes (Original) - Giữ nguyên
            modelBuilder.Entity<Episode>().HasData(
                new Episode { Id = 1, MovieId = 2, EpisodeNumber = 1, Title = "Tập 1: Cuộc gặp gỡ định mệnh", Description = "Yoon Se-ri vô tình hạ cánh xuống Bắc Triều Tiên sau một tai nạn.", VideoPath = "/videos/1.mp4", Duration = 70, ReleaseDate = new DateTime(2019, 12, 14), Views = 10000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Episode { Id = 2, MovieId = 2, EpisodeNumber = 2, Title = "Tập 2: Kế hoạch trốn thoát", Description = "Ri Jeong-hyeok giúp Se-ri tìm cách trở về Hàn Quốc.", VideoPath = "/videos/1.mp4", Duration = 65, ReleaseDate = new DateTime(2019, 12, 15), Views = 9500, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Episode { Id = 3, MovieId = 2, EpisodeNumber = 3, Title = "Tập 3: Bí mật bị hé lộ", Description = "Se-ri dần thích nghi với cuộc sống ở Bắc Triều Tiên.", VideoPath = "/videos/1.mp4", Duration = 68, ReleaseDate = new DateTime(2019, 12, 21), Views = 9000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Episode { Id = 4, MovieId = 2, EpisodeNumber = 4, Title = "Tập 4: Tình cảm nảy nở", Description = "Tình cảm giữa Se-ri và Jeong-hyeok bắt đầu phát triển.", VideoPath = "/videos/1.mp4", Duration = 72, ReleaseDate = new DateTime(2019, 12, 22), Views = 8800, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Episode { Id = 5, MovieId = 2, EpisodeNumber = 5, Title = "Tập 5: Thách thức mới", Description = "Se-ri đối mặt với những nguy hiểm mới ở Bắc Triều Tiên.", VideoPath = "/videos/1.mp4", Duration = 70, ReleaseDate = new DateTime(2019, 12, 28), Views = 8500, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Episode { Id = 6, MovieId = 2, EpisodeNumber = 6, Title = "Tập 6: Hành trình nguy hiểm", Description = "Jeong-hyeok lên kế hoạch bảo vệ Se-ri.", VideoPath = "/videos/1.mp4", Duration = 67, ReleaseDate = new DateTime(2019, 12, 29), Views = 8200, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );

            // Seed Ratings (Original) - Giữ nguyên
            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, MovieId = 1, UserName = "NguyenVanA", UserEmail = "nguyenvana@example.com", Score = 8, Review = "Phim rất cảm động, diễn xuất của Trấn Thành tuyệt vời!", CreatedAt = DateTime.Now },
                new Rating { Id = 2, MovieId = 1, UserName = "TranThiB", UserEmail = "tranthib@example.com", Score = 9, Review = "Cốt truyện gần gũi, phản ánh đúng cuộc sống.", CreatedAt = DateTime.Now },
                new Rating { Id = 3, MovieId = 3, UserName = "LeVanC", UserEmail = "levanc@example.com", Score = 10, Review = "Một kiệt tác của điện ảnh Hàn Quốc!", CreatedAt = DateTime.Now }
            );

            // Seed Comments (Original) - Giữ nguyên
            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, MovieId = 1, EpisodeId = null, UserName = "NguyenVanA", UserEmail = "nguyenvana@example.com", Content = "Phim này thực sự làm mình khóc rất nhiều!", Likes = 50, Dislikes = 2, CreatedAt = DateTime.Now },
                new Comment { Id = 2, MovieId = 1, EpisodeId = null, UserName = "TranThiB", UserEmail = "tranthib@example.com", Content = "Cảm ơn Trấn Thành đã mang đến một bộ phim ý nghĩa!", ParentCommentId = 1, Likes = 30, Dislikes = 1, CreatedAt = DateTime.Now },
                new Comment { Id = 3, MovieId = 2, EpisodeId = 1, UserName = "LeVanC", UserEmail = "levanc@example.com", Content = "Tập 1 rất hấp dẫn, chemistry giữa hai diễn viên chính đỉnh cao!", Likes = 40, Dislikes = 0, CreatedAt = DateTime.Now },
                new Comment { Id = 4, MovieId = 2, EpisodeId = 2, UserName = "PhamThiD", UserEmail = "phamthid@example.com", Content = "Tập 2 càng cuốn, không thể rời mắt!", Likes = 35, Dislikes = 1, CreatedAt = DateTime.Now }
            );

            // Seed additional movies (Targeting around 40 total, so 37 new movies)
            int currentMovieId = 4;
            int currentEpisodeId = 7;

            var moviesToSeed = new List<Movie>();
            var movieGenresToSeed = new List<MovieGenre>();
            var episodesToSeed = new List<Episode>();

            var sampleMovies = new List<(string Title, string EnglishTitle, string Description, int ReleaseYear, int CountryId, int[] GenreIds, string Director, string Cast, string PosterPath, string TrailerPath, int TotalEpisodes, bool IsCompleted, int Views, double AverageRating, int RatingCount)>
        {
            // Việt Nam (VN) - 5 phim
            ("Đất Rừng Phương Nam", "Southern Forest Land", "Câu chuyện về hành trình trưởng thành của bé An tại miền Nam Việt Nam.", 2023, 1, new[] { 6, 7 }, "Võ Thanh Hòa", "Hạo Khang, Trấn Thành, Mai Cát Vi", "/images/4.png", "/videos/4.mp4", 1, true, 120000, 7.8, 800),
            ("Nhà Bà Nữ", "The House of No Man", "Câu chuyện về mâu thuẫn gia đình và tình yêu.", 2023, 1, new[] { 2, 7 }, "Trấn Thành", "Trấn Thành, Uyển Ân, Ngọc Châu", "/images/5.png", "/videos/5.mp4", 1, true, 180000, 7.5, 1500),
            ("Lật Mặt 6: Tấm Vé Định Mệnh", "Face Off 6: The Destiny Ticket", "Cuộc phiêu lưu để giành tấm vé số định mệnh.", 2023, 1, new[] { 3, 6 }, "Lý Hải", "Thái Hòa, Thanh Thức, Huỳnh Đông", "/images/6.png", "/videos/6.mp4", 1, true, 150000, 7.2, 1300),
            ("Tiệc Trăng Máu", "Full House", "Những bí mật của một nhóm bạn được phơi bày qua một trò chơi.", 2020, 1, new[] { 3, 7 }, "Nguyễn Quang Dũng", "Thái Hòa, Kiều Minh Tuấn, Đức Thịnh", "/images/7.png", "/videos/8.mp4", 1, true, 130000, 7.0, 1200),
            ("Em Chưa 18", "Sweet 18", "Câu chuyện tình yêu tuổi teen với nhiều tình huống dở khóc dở cười.", 2017, 1, new[] { 2, 3 }, "Lê Thanh Sơn", "Kaity Nguyễn, Will", "/images/3.png", "/videos/32.mp4", 1, true, 110000, 6.8, 1000),

            // Hàn Quốc (KR) - 6 phim
            ("Cô Dâu Băng Giá", "Frozen Bride", "Một câu chuyện cổ tích về tình yêu và băng giá.", 2020, 2, new[] { 2, 8 }, "Hayao Miyazaki", "Suzu Hirose, Kôki Mitsushima", "/images/7.png", "/videos/7.mp4", 1, true, 190000, 8.7, 1600),
            ("Chiếc Lá Cuốn Bay", "Bai Mai Tee Plid Plew", "Cuộc đời đầy biến cố của một cô gái chuyển giới.", 2019, 2, new[] { 7 }, "Sopon Sukapisoot", "Baifern Pimchanok, Push Puttichai", "/images/8.png", "/videos/8.mp4", 20, true, 400000, 8.5, 3500),
            ("Goblin", "Guardian: The Lonely and Great God", "Câu chuyện tình yêu giữa một goblin và một cô dâu loài người.", 2016, 2, new[] { 2, 5 }, "Kim Eun-sook", "Gong Yoo, Kim Go-eun, Lee Dong-wook", "/images/1.png", "/videos/9.mp4", 16, true, 300000, 9.0, 2500),
            ("Vinh Quang Trong Thù Hận", "The Glory", "Một cô gái tìm cách trả thù những kẻ đã bắt nạt mình.", 2022, 2, new[] { 7, 9 }, "Ahn Gil-ho", "Song Hye-kyo, Lee Do-hyun", "/images/3.png", "/videos/33.mp4", 16, true, 350000, 9.2, 3000),
            ("Hospital Playlist", "Hospital Playlist", "Cuộc sống và tình bạn của 5 bác sĩ.", 2020, 2, new[] { 2, 7 }, "Shin Won-ho", "Jo Jung-suk, Yoo Yeon-seok, Jung Kyung-ho", "/images/4.png", "/videos/34.mp4", 24, true, 280000, 9.3, 2300),
            ("Reply 1988", "Reply 1988", "Câu chuyện về tình bạn và gia đình tại Seoul năm 1988.", 2015, 2, new[] { 2, 7 }, "Shin Won-ho", "Lee Hye-ri, Ryu Jun-yeol, Go Kyung-pyo", "/images/5.png", "/videos/35.mp4", 20, true, 290000, 9.4, 2400),

            // Trung Quốc (CN) - 4 phim
            ("The Wandering Earth", "The Wandering Earth", "Nhân loại di chuyển Trái Đất ra khỏi hệ Mặt Trời.", 2019, 3, new[] { 5, 6 }, "Frant Gwo", "Wu Jing, Qing Xu", "/images/1.png", "/videos/10.mp4", 1, true, 320000, 7.0, 2800),
            ("Ngoạ Hổ Tàng Long", "Crouching Tiger, Hidden Dragon", "Kiếm hiệp tình duyên với những màn võ thuật đẹp mắt.", 2000, 3, new[] { 1, 6 }, "Lý An", "Châu Nhuận Phát, Dương Tử Quỳnh, Chương Tử Di", "/images/11.png", "/videos/11.mp4", 1, true, 250000, 8.2, 2000),
            ("Tam Sinh Tam Thế Thập Lý Đào Hoa", "Eternal Love", "Chuyện tình xuyên không gian và thời gian.", 2017, 3, new[] { 2, 5 }, "Lâm Ngọc Phượng", "Dương Mịch, Triệu Hựu Đình", "/images/2.png", "/videos/12.mp4", 58, true, 450000, 8.8, 4000),
            ("Chiến Lang 2", "Wolf Warrior 2", "Người lính giải cứu công dân Trung Quốc ở châu Phi.", 2017, 3, new[] { 1, 6 }, "Ngô Kinh", "Ngô Kinh, Frank Grillo", "/images/36.png", "/videos/6.mp4", 1, true, 380000, 7.5, 3200),

            // Nhật Bản (JP) - 4 phim
            ("Your Name", "Kimi no Na wa.", "Câu chuyện tình yêu kỳ diệu giữa hai người xa lạ đổi linh hồn.", 2016, 4, new[] { 2, 5 }, "Makoto Shinkai", "Ryunosuke Kamiki, Mone Kamishiraishi", "/images/3.png", "/videos/13.mp4", 1, true, 420000, 9.3, 3800),
            ("Spirited Away", "Sen to Chihiro no Kamikakushi", "Hành trình phiêu lưu của cô bé Chihiro tại thế giới linh hồn.", 2001, 4, new[] { 8, 6 }, "Hayao Miyazaki", "Rumi Hiiragi, Miyu Irino", "/images/4.png", "/videos/14.mp4", 1, true, 380000, 9.2, 3600),
            ("One Piece Film: Red", "One Piece Film: Red", "Chuyến phiêu lưu âm nhạc của nhóm Mũ Rơm.", 2022, 4, new[] { 8, 1, 6 }, "Goro Taniguchi", "Mayumi Tanaka, Kazuya Nakai", "/images/15.png", "/videos/5.mp4", 1, true, 300000, 8.0, 2500),
            ("Your Lie in April", "Shigatsu wa Kimi no Uso", "Câu chuyện tình cảm học đường về âm nhạc và tình yêu.", 2016, 4, new[] { 2, 7 }, "Hiroshi Nishikori", "Haruka Misaki, Kousei Arima", "/images/7.png", "/videos/37.mp4", 22, true, 220000, 8.5, 1900),

            // Mỹ (US) - 10 phim
            ("Avengers: Endgame", "Avengers: Endgame", "Cuộc chiến cuối cùng của các siêu anh hùng chống lại Thanos.", 2019, 5, new[] { 1, 5, 6 }, "Anthony Russo, Joe Russo", "Robert Downey Jr., Chris Evans, Mark Ruffalo", "/images/6.png", "/videos/16.mp4", 1, true, 500000, 9.5, 5000),
            ("Joker", "Joker", "Nguồn gốc của gã hề phản diện Joker.", 2019, 5, new[] { 7, 9 }, "Todd Phillips", "Joaquin Phoenix, Robert De Niro", "/images/17.png", "/videos/7.mp4", 1, true, 450000, 9.0, 4000),
            ("Fast & Furious 9", "F9", "Dom và gia đình đối mặt với kẻ thù mới.", 2021, 5, new[] { 1, 6 }, "Justin Lin", "Vin Diesel, Michelle Rodriguez", "/images/18.png", "/videos/8.mp4", 1, true, 300000, 6.5, 2500),
            ("Spider-Man: No Way Home", "Spider-Man: No Way Home", "Spider-Man đối mặt với đa vũ trụ.", 2021, 5, new[] { 1, 5, 6 }, "Jon Watts", "Tom Holland, Zendaya, Benedict Cumberbatch", "/images/1.png", "/videos/19.mp4", 1, true, 550000, 9.0, 5200),
            ("Dune", "Dune", "Hành trình của Paul Atreides tại hành tinh Arrakis.", 2021, 5, new[] { 5, 6 }, "Denis Villeneuve", "Timothée Chalamet, Rebecca Ferguson", "/images/2.png", "/videos/2.mp4", 1, true, 350000, 8.0, 3000),
            ("Encanto", "Encanto", "Gia đình Madrigal với phép thuật.", 2021, 5, new[] { 8, 2 }, "Jared Bush, Byron Howard", "Stephanie Beatriz, María Cecilia Botero", "/images/1.png", "/videos/2.mp4", 1, true, 330000, 8.0, 2900),
            ("The Lion King", "The Lion King", "Câu chuyện về chú sư tử Simba.", 2019, 5, new[] { 8, 6 }, "Jon Favreau", "Donald Glover, Beyoncé, Seth Rogen", "/images/4.png", "/videos/22.mp4", 1, true, 360000, 7.0, 3100),
            ("The Dark Knight", "The Dark Knight", "Batman đối đầu với Joker.", 2008, 5, new[] { 1, 7, 9 }, "Christopher Nolan", "Christian Bale, Heath Ledger, Aaron Eckhart", "/images/8.png", "/videos/38.mp4", 1, true, 480000, 9.6, 4500),
            ("Inception", "Inception", "Cuộc hành trình vào thế giới giấc mơ.", 2010, 5, new[] { 5, 9 }, "Christopher Nolan", "Leonardo DiCaprio, Joseph Gordon-Levitt", "/images/3.png", "/videos/39.mp4", 1, true, 460000, 9.3, 4200),
            ("Forrest Gump", "Forrest Gump", "Cuộc đời của một người đàn ông đặc biệt.", 1994, 5, new[] { 2, 7 }, "Robert Zemeckis", "Tom Hanks, Robin Wright", "/images/40.png", "/videos/4.mp4", 1, true, 400000, 9.1, 3800),

            // Thái Lan (TH) - 2 phim
            ("Bad Genius", "Chalard Games Goeng", "Câu chuyện về những học sinh thiên tài gian lận thi cử.", 2017, 6, new[] { 7, 1 }, "Nattawut Poonpiriya", "Chutimon Chuengcharoensukying, Non Chanon Santinatornkul", "/images/3.png", "/videos/23.mp4", 1, true, 200000, 7.8, 1800),
            ("Friend Zone", "Friend Zone", "Hành trình tìm kiếm tình yêu của một chàng trai.", 2019, 6, new[] { 2, 3 }, "Chayanop Boonprakob", "Nine Naphat Siwopornthong, Pimchanok Luevisadpaipa", "/images/24.png", "/videos/4.mp4", 1, true, 220000, 7.2, 1900),

            // Anh (UK) - 2 phim
            ("The Imitation Game", "The Imitation Game", "Câu chuyện về nhà toán học Alan Turing.", 2014, 7, new[] { 7, 9 }, "Morten Tyldum", "Benedict Cumberbatch, Keira Knightley", "/images/5.png", "/videos/41.mp4", 1, true, 180000, 8.0, 1500),
            ("Harry Potter and the Sorcerer's Stone", "Harry Potter and the Philosopher's Stone", "Cuộc phiêu lưu đầu tiên của Harry Potter.", 2001, 7, new[] { 6, 8, 5 }, "Chris Columbus", "Daniel Radcliffe, Rupert Grint, Emma Watson", "/images/6.png", "/videos/42.mp4", 1, true, 400000, 8.5, 3800),

            // Pháp (FR) - 2 phim
            ("Amélie", "Le Fabuleux Destin d'Amélie Poulain", "Cuộc sống kỳ diệu của một cô gái ở Paris.", 2001, 8, new[] { 2, 3, 7 }, "Jean-Pierre Jeunet", "Audrey Tautou, Mathieu Kassovitz", "/images/7.png", "/videos/43.mp4", 1, true, 190000, 8.3, 1700),
            ("The Intouchables", "Intouchables", "Câu chuyện tình bạn giữa một triệu phú và người chăm sóc anh.", 2011, 8, new[] { 2, 7 }, "Olivier Nakache, Éric Toledano", "François Cluzet, Omar Sy", "/images/8.png", "/videos/44.mp4", 1, true, 210000, 8.6, 1900),

            // Canada (CA) - 1 phim
            ("Room", "Room", "Câu chuyện về một người mẹ và con trai thoát khỏi nơi giam cầm.", 2015, 9, new[] { 7, 9 }, "Lenny Abrahamson", "Brie Larson, Jacob Tremblay", "/images/4.png", "/videos/45.mp4", 1, true, 160000, 8.1, 1400)
        };

            foreach (var movieData in sampleMovies)
            {
                var movie = new Movie
                {
                    Id = currentMovieId,
                    Title = movieData.Title,
                    EnglishTitle = movieData.EnglishTitle,
                    Description = movieData.Description,
                    ReleaseYear = movieData.ReleaseYear,
                    CountryId = movieData.CountryId,
                    GenreId = movieData.GenreIds.FirstOrDefault(), // Lấy thể loại chính đầu tiên
                    Director = movieData.Director,
                    Cast = movieData.Cast,
                    PosterPath = movieData.PosterPath,
                    TrailerPath = movieData.TrailerPath,
                    TotalEpisodes = movieData.TotalEpisodes,
                    IsCompleted = movieData.IsCompleted,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Views = movieData.Views,
                    AverageRating = movieData.AverageRating,
                    RatingCount = movieData.RatingCount
                };
                moviesToSeed.Add(movie);

                // Add to MovieGenres
                foreach (var genreId in movieData.GenreIds)
                {
                    if (movieData.GenreIds.FirstOrDefault() != genreId)
                    {
                        movieGenresToSeed.Add(new MovieGenre { MovieId = currentMovieId, GenreId = genreId });
                    }
                }

                // Seed 5 Episodes for each movie (or fewer if TotalEpisodes is small and they are not series)
                // For single-episode movies, we can either not seed episodes or seed one episode as a placeholder.
                // Here, I'll seed 5 episodes for consistency, but in a real app, this logic might be more nuanced.
                for (int i = 1; i <= Math.Min(5, movieData.TotalEpisodes); i++) // Ensure we don't exceed TotalEpisodes if it's less than 5
                {
                    episodesToSeed.Add(new Episode
                    {
                        Id = currentEpisodeId++,
                        MovieId = currentMovieId,
                        EpisodeNumber = i,
                        Title = $"Tập {i}: {movie.Title}",
                        Description = $"Mô tả tập {i} của phim {movie.Title}",
                        VideoPath = $"/videos/{movieData.PosterPath.Split('/')[2].Split('.')[0]}.mp4", // Giả định tên file video giống poster
                        Duration = 60,
                        ReleaseDate = DateTime.Now.AddDays(-(5 - i) * 7),
                        Views = new Random().Next(5000, 15000),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                currentMovieId++;
            }

            modelBuilder.Entity<Movie>().HasData(moviesToSeed);
            modelBuilder.Entity<MovieGenre>().HasData(movieGenresToSeed);
            modelBuilder.Entity<Episode>().HasData(episodesToSeed);
        }
    }
}
