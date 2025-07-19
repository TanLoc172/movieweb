using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Models; // Giả định Model của bạn nằm trong namespace này

namespace MovieWebsite.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // =================================================================
            // SEED COUNTRIES & GENRES (Giữ nguyên)
            // =================================================================
            modelBuilder
                .Entity<Country>()
                .HasData(
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

            modelBuilder
                .Entity<Genre>()
                .HasData(
                    new Genre { Id = 1, Name = "Hành Động", Color = "#FF5722" },
                    new Genre { Id = 2, Name = "Tình Cảm", Color = "#E91E63" },
                    new Genre { Id = 3, Name = "Hài Hước", Color = "#FFC107" },
                    new Genre { Id = 4, Name = "Kinh Dị", Color = "#424242" },
                    new Genre { Id = 5, Name = "Khoa Học Viễn Tưởng", Color = "#2196F3" },
                    new Genre { Id = 6, Name = "Phiêu Lưu", Color = "#4CAF50" },
                    new Genre { Id = 7, Name = "Chính Kịch", Color = "#9C27B0" },
                    new Genre { Id = 8, Name = "Hoạt Hình", Color = "#FF9800" },
                    new Genre { Id = 9, Name = "Tâm Lý", Color = "#00BCD4" },
                    new Genre { Id = 10, Name = "Gia Đình", Color = "#795548" }
                );

            // =================================================================
            // SEED ROLES (Giữ nguyên)
            // =================================================================
            var adminRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN",
            };
            var userRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "USER",
            };

            modelBuilder.Entity<IdentityRole>().HasData(adminRole, userRole);

            // =================================================================
            // SEED MOVIES, EPISODES & MOVIEGENRES (Dữ liệu phim được viết lại theo yêu cầu)
            // Mỗi quốc gia có 3 bộ phim, mỗi phim có 2 tập, mỗi tập 20 phút.
            // =================================================================

            var movies = new List<Movie>();
            var episodes = new List<Episode>();
            var movieGenres = new List<MovieGenre>();

            // Khởi tạo lại bộ đếm để đảm bảo ID tuần tự cho dữ liệu mới
            int movieIdCounter = 0;
            int episodeIdCounter = 0;
            var now = DateTime.UtcNow;
            int durationPerEpisode = 1200; // 20 phút = 1200 giây

            // --- PHIM VIỆT NAM (CountryId = 1) ---
            // 1. Mắt Biếc
            movieIdCounter++;
            movies.Add(new Movie
            {
                Id = movieIdCounter, Title = "Mắt Biếc", EnglishTitle = "Dreamy Eyes",
                Description = "Chuyện phim kể về mối tình si của chàng trai Ngạn dành cho Hà Lan từ thuở thiếu thời...",
                ReleaseYear = 2019, CountryId = 1, GenreId = 2, // Tình Cảm
                Director = "Victor Vũ", Cast = "Trần Nghĩa, Trúc Anh",
                PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
                PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
                TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
                TotalEpisodes = 2, IsCompleted = true, Views = 1500000, AverageRating = 8.5, RatingCount = 25000,
                CreatedAt = now.AddDays(-1200), UpdatedAt = now.AddDays(-1200)
            });
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // 2. Tiệc Trăng Máu
            movieIdCounter++;
            movies.Add(new Movie
            {
                Id = movieIdCounter, Title = "Tiệc Trăng Máu", EnglishTitle = "Full House (Vietnamese)",
                Description = "Một buổi tối tụ họp bạn bè bỗng trở nên căng thẳng khi mọi người quyết định chơi một trò chơi...",
                ReleaseYear = 2020, CountryId = 1, GenreId = 3, // Hài Hước
                Director = "Nguyễn Quang Dũng", Cast = "Thái Hòa, Kiều Minh Tuấn",
                PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
                PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
                TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
                TotalEpisodes = 2, IsCompleted = true, Views = 1200000, AverageRating = 7.8, RatingCount = 18000,
                CreatedAt = now.AddDays(-1000), UpdatedAt = now.AddDays(-1000)
            });
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 4 }); // Giật gân
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // 3. Bố Già
            movieIdCounter++;
            movies.Add(new Movie
            {
                Id = movieIdCounter, Title = "Bố Già", EnglishTitle = "Dad, I'm Sorry",
                Description = "Câu chuyện cảm động về tình cha con, những xung đột thế hệ và tình làng nghĩa xóm...",
                ReleaseYear = 2021, CountryId = 1, GenreId = 10, // Gia Đình
                Director = "Vũ Ngọc Đãng", Cast = "Trấn Thành, Tuấn Trần",
                PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
                PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
                TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
                TotalEpisodes = 2, IsCompleted = true, Views = 2000000, AverageRating = 8.8, RatingCount = 35000,
                CreatedAt = now.AddDays(-900), UpdatedAt = now.AddDays(-900)
            });
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia Đình
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // --- PHIM HÀN QUỐC (CountryId = 2) ---
            // 4. Ký Sinh Trùng (Parasite)
            movieIdCounter++;
            movies.Add(new Movie
            {
                Id = movieIdCounter, Title = "Ký Sinh Trùng", EnglishTitle = "Parasite",
                Description = "Một gia đình nghèo tìm cách 'ký sinh' vào cuộc sống của một gia đình giàu có...",
                ReleaseYear = 2019, CountryId = 2, GenreId = 7, // Chính Kịch
                Director = "Bong Joon Ho", Cast = "Song Kang-ho, Choi Woo-shik",
                PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
                PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
                TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
                TotalEpisodes = 2, IsCompleted = true, Views = 1800000, AverageRating = 9.2, RatingCount = 28000,
                CreatedAt = now.AddDays(-1500), UpdatedAt = now.AddDays(-1500)
            });
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước (đen)
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 5. Hậu Duệ Mặt Trời (Descendants of the Sun)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Hậu Duệ Mặt Trời", EnglishTitle = "Descendants of the Sun",
            //     Description = "Câu chuyện tình yêu giữa đại úy Yoo Shi-jin và bác sĩ Kang Mo-yeon...",
            //     ReleaseYear = 2016, CountryId = 2, GenreId = 2, // Tình Cảm
            //     Director = "Lee Eung-bok", Cast = "Song Joong-ki, Song Hye-kyo",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 4500000, AverageRating = 8.9, RatingCount = 45000,
            //     CreatedAt = now.AddDays(-3000), UpdatedAt = now.AddDays(-3000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 6. Goblin (Yêu Tinh)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Goblin", EnglishTitle = "Guardian: The Lonely and Great God",
            //     Description = "Câu chuyện về Kim Shin, một yêu tinh bất tử và mối tình định mệnh với nữ sinh trung học...",
            //     ReleaseYear = 2016, CountryId = 2, GenreId = 2, // Tình Cảm
            //     Director = "Lee Eung-bok", Cast = "Gong Yoo, Kim Go-eun",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 4000000, AverageRating = 9.1, RatingCount = 40000,
            //     CreatedAt = now.AddDays(-3000), UpdatedAt = now.AddDays(-3000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM TRUNG QUỐC (CountryId = 3) ---
            // // 7. Tam Sinh Tam Thế Thập Lý Đào Hoa
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Tam Sinh Tam Thế Thập Lý Đào Hoa", EnglishTitle = "Eternal Love",
            //     Description = "Chuyện tình bi thương kéo dài ba kiếp giữa hồ yêu Bạch Thiển và thượng thần Dạ Hoa...",
            //     ReleaseYear = 2017, CountryId = 3, GenreId = 2, // Tình Cảm
            //     Director = "Lưu Dịch Sơ", Cast = "Dương Mịch, Triệu Hựu Đình",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 5000000, AverageRating = 9.0, RatingCount = 50000,
            //     CreatedAt = now.AddDays(-2500), UpdatedAt = now.AddDays(-2500)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 8. Diệp Vấn
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Diệp Vấn", EnglishTitle = "Ip Man",
            //     Description = "Diệp Vấn, một võ sư Vịnh Xuân quyền tài ba, phải đối mặt với những thử thách khắc nghiệt...",
            //     ReleaseYear = 2008, CountryId = 3, GenreId = 1, // Hành Động
            //     Director = "Diệp Vĩ Tín", Cast = "Chân Tử Đan",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 900000, AverageRating = 8.2, RatingCount = 15000,
            //     CreatedAt = now.AddDays(-5000), UpdatedAt = now.AddDays(-5000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 9. Anh Hùng
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Anh Hùng", EnglishTitle = "Hero",
            //     Description = "Trong thời Chiến Quốc, một người vô danh nhận nhiệm vụ ám sát Tần Thủy Hoàng...",
            //     ReleaseYear = 2002, CountryId = 3, GenreId = 1, // Hành Động
            //     Director = "Trương Nghệ Mưu", Cast = "Lý Liên Kiệt, Chương Tử Di",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 750000, AverageRating = 8.0, RatingCount = 12000,
            //     CreatedAt = now.AddDays(-7000), UpdatedAt = now.AddDays(-7000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM NHẬT BẢN (CountryId = 4) ---
            // // 10. Tên Cậu Là Gì? (Your Name)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Tên Cậu Là Gì?", EnglishTitle = "Your Name.",
            //     Description = "Hai thiếu niên trao đổi cơ thể một cách bí ẩn mỗi khi ngủ...",
            //     ReleaseYear = 2016, CountryId = 4, GenreId = 2, // Tình Cảm
            //     Director = "Makoto Shinkai", Cast = "Ryunosuke Kamiki (lồng tiếng)",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1300000, AverageRating = 8.9, RatingCount = 22000,
            //     CreatedAt = now.AddDays(-2600), UpdatedAt = now.AddDays(-2600)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 11. Những Kẻ Trộm Vặt (Shoplifters)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Những Kẻ Trộm Vặt", EnglishTitle = "Shoplifters",
            //     Description = "Một gia đình sống bằng nghề trộm cắp nhỏ bé tìm thấy một cô bé bị bỏ rơi...",
            //     ReleaseYear = 2018, CountryId = 4, GenreId = 7, // Chính Kịch
            //     Director = "Hirokazu Kore-eda", Cast = "Lily Franky, Sakura Ando",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 400000, AverageRating = 8.3, RatingCount = 7000,
            //     CreatedAt = now.AddDays(-2000), UpdatedAt = now.AddDays(-2000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia đình
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 12. Vùng Đất Linh Hồn (Spirited Away)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Vùng Đất Linh Hồn", EnglishTitle = "Spirited Away",
            //     Description = "Một cô bé 10 tuổi bị lạc vào thế giới của các vị thần và linh hồn...",
            //     ReleaseYear = 2001, CountryId = 4, GenreId = 8, // Hoạt Hình
            //     Director = "Hayao Miyazaki", Cast = "Rumi Hiiragi (lồng tiếng)",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1400000, AverageRating = 8.6, RatingCount = 24000,
            //     CreatedAt = now.AddDays(-8000), UpdatedAt = now.AddDays(-8000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 8 }); // Hoạt Hình
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM MỸ (CountryId = 5) ---
            // // 13. Kỵ Sĩ Bóng Đêm (The Dark Knight)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Kỵ Sĩ Bóng Đêm", EnglishTitle = "The Dark Knight",
            //     Description = "Batman phải đối mặt với tên tội phạm Joker đầy điên loạn...",
            //     ReleaseYear = 2008, CountryId = 5, GenreId = 1, // Hành Động
            //     Director = "Christopher Nolan", Cast = "Christian Bale, Heath Ledger",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 2500000, AverageRating = 9.0, RatingCount = 40000,
            //     CreatedAt = now.AddDays(-5500), UpdatedAt = now.AddDays(-5500)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 14. Kẻ Đánh Cắp Giấc Mơ (Inception)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Kẻ Đánh Cắp Giấc Mơ", EnglishTitle = "Inception",
            //     Description = "Một đạo chích chuyên xâm nhập vào giấc mơ của người khác...",
            //     ReleaseYear = 2010, CountryId = 5, GenreId = 5, // Khoa Học Viễn Tưởng
            //     Director = "Christopher Nolan", Cast = "Leonardo DiCaprio",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 2200000, AverageRating = 8.8, RatingCount = 38000,
            //     CreatedAt = now.AddDays(-5000), UpdatedAt = now.AddDays(-5000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Khoa Học Viễn Tưởng
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 15. Forrest Gump
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Forrest Gump", EnglishTitle = "Forrest Gump",
            //     Description = "Cuộc đời phi thường của một người đàn ông có chỉ số IQ thấp nhưng trái tim nhân hậu...",
            //     ReleaseYear = 1994, CountryId = 5, GenreId = 7, // Chính Kịch
            //     Director = "Robert Zemeckis", Cast = "Tom Hanks",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1900000, AverageRating = 8.8, RatingCount = 32000,
            //     CreatedAt = now.AddDays(-10000), UpdatedAt = now.AddDays(-10000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM THÁI LAN (CountryId = 6) ---
            // // 16. Thiên Tài Bất Hảo (Bad Genius)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Thiên Tài Bất Hảo", EnglishTitle = "Bad Genius",
            //     Description = "Một nữ sinh thiên tài lập kế hoạch gian lận thi cử tinh vi...",
            //     ReleaseYear = 2017, CountryId = 6, GenreId = 7, // Chính Kịch
            //     Director = "Nattawut Poonpiriya", Cast = "Chutimon Chuengcharoensukying",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 600000, AverageRating = 7.5, RatingCount = 9000,
            //     CreatedAt = now.AddDays(-2500), UpdatedAt = now.AddDays(-2500)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Giật gân
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 17. Bad Boy 2 (Placeholder)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Bad Boy 2", EnglishTitle = "Bad Boy 2",
            //     Description = "Câu chuyện về một tay đua đường phố và những cuộc phiêu lưu tốc độ.",
            //     ReleaseYear = 2018, CountryId = 6, GenreId = 1, // Hành Động
            //     Director = "TBD", Cast = "Actor A, Actor B",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 200000, AverageRating = 6.5, RatingCount = 3000,
            //     CreatedAt = now.AddDays(-2200), UpdatedAt = now.AddDays(-2200)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 18. Friend Zone (Placeholder)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Friend Zone", EnglishTitle = "Friend Zone",
            //     Description = "Câu chuyện hài hước về tình bạn và tình yêu chớm nở.",
            //     ReleaseYear = 2019, CountryId = 6, GenreId = 3, // Hài Hước
            //     Director = "TBD", Cast = "Actor C, Actor D",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 300000, AverageRating = 7.0, RatingCount = 5000,
            //     CreatedAt = now.AddDays(-2000), UpdatedAt = now.AddDays(-2000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM ANH (CountryId = 7) ---
            // // 19. Chúa Tể Của Những Chiếc Nhẫn: Hiệp Hội Nhẫn Thân
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Chúa Tể Của Những Chiếc Nhẫn: Hiệp Hội Nhẫn Thân", EnglishTitle = "The Lord of the Rings: The Fellowship of the Ring",
            //     Description = "Cuộc hành trình của người Hobbit Frodo Baggins cùng các đồng minh...",
            //     ReleaseYear = 2001, CountryId = 7, GenreId = 6, // Phiêu Lưu
            //     Director = "Peter Jackson", Cast = "Elijah Wood, Ian McKellen",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1600000, AverageRating = 8.8, RatingCount = 29000,
            //     CreatedAt = now.AddDays(-8000), UpdatedAt = now.AddDays(-8000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Giả tưởng
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 20. Harry Potter và Hòn Đá Phù Thủy
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Harry Potter Và Hòn Đá Phù Thủy", EnglishTitle = "Harry Potter and the Sorcerer's Stone",
            //     Description = "Harry Potter khám phá mình là một phù thủy và bắt đầu hành trình học tập tại Trường Hogwarts...",
            //     ReleaseYear = 2001, CountryId = 7, GenreId = 6, // Phiêu Lưu
            //     Director = "Chris Columbus", Cast = "Daniel Radcliffe, Emma Watson",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 2800000, AverageRating = 7.6, RatingCount = 35000,
            //     CreatedAt = now.AddDays(-8000), UpdatedAt = now.AddDays(-8000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia đình
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 21. Sherlock Holmes (Placeholder)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Sherlock Holmes", EnglishTitle = "Sherlock Holmes",
            //     Description = "Thám tử tài ba Sherlock Holmes phá những vụ án ly kỳ tại London.",
            //     ReleaseYear = 2009, CountryId = 7, GenreId = 1, // Hành Động
            //     Director = "Guy Ritchie", Cast = "Robert Downey Jr., Jude Law",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1000000, AverageRating = 7.6, RatingCount = 15000,
            //     CreatedAt = now.AddDays(-4800), UpdatedAt = now.AddDays(-4800)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM PHÁP (CountryId = 8) ---
            // // 22. Amélie
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Amélie", EnglishTitle = "Amélie",
            //     Description = "Câu chuyện về Amélie Poulain, một cô gái làm bồi bàn ở Paris, người bí mật mang lại hạnh phúc...",
            //     ReleaseYear = 2001, CountryId = 8, GenreId = 3, // Hài Hước
            //     Director = "Jean-Pierre Jeunet", Cast = "Audrey Tautou",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 500000, AverageRating = 8.3, RatingCount = 10000,
            //     CreatedAt = now.AddDays(-8000), UpdatedAt = now.AddDays(-8000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 23. Những Kẻ Bất Khả Xâm Phạm (The Intouchables)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Những Kẻ Bất Khả Xâm Phạm", EnglishTitle = "The Intouchables",
            //     Description = "Câu chuyện có thật về tình bạn kỳ lạ giữa một nhà tài phiệt bị liệt và người chăm sóc...",
            //     ReleaseYear = 2011, CountryId = 8, GenreId = 3, // Hài Hước
            //     Director = "Olivier Nakache, Éric Toledano", Cast = "François Cluzet, Omar Sy",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 950000, AverageRating = 8.5, RatingCount = 16000,
            //     CreatedAt = now.AddDays(-4500), UpdatedAt = now.AddDays(-4500)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 24. Paris, je t'aime (Placeholder)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Paris, je t'aime", EnglishTitle = "Paris, je t'aime",
            //     Description = "Tuyển tập các câu chuyện ngắn về tình yêu tại Paris.",
            //     ReleaseYear = 2006, CountryId = 8, GenreId = 2, // Tình Cảm
            //     Director = "Various", Cast = "Various Actors",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 150000, AverageRating = 7.0, RatingCount = 4000,
            //     CreatedAt = now.AddDays(-6000), UpdatedAt = now.AddDays(-6000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // // --- PHIM CANADA (CountryId = 9) ---
            // // 25. Người Về Từ Cõi Chết (The Revenant)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Người Về Từ Cõi Chết", EnglishTitle = "The Revenant",
            //     Description = "Trong vùng đất hoang dã khắc nghiệt, một người thợ săn chiến đấu để sống sót và trả thù.",
            //     ReleaseYear = 2015, CountryId = 9, GenreId = 6, // Phiêu Lưu
            //     Director = "Alejandro G. Iñárritu", Cast = "Leonardo DiCaprio, Tom Hardy",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 1100000, AverageRating = 8.0, RatingCount = 20000,
            //     CreatedAt = now.AddDays(-3500), UpdatedAt = now.AddDays(-3500)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 26. Scott Pilgrim vs. the World (filmed in Canada)
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Scott Pilgrim vs. the World", EnglishTitle = "Scott Pilgrim vs. the World",
            //     Description = "Một nhạc sĩ lập dị phải chiến đấu với bảy tình cũ độc ác của bạn gái mới của mình.",
            //     ReleaseYear = 2010, CountryId = 9, GenreId = 1, // Hành Động
            //     Director = "Edgar Wright", Cast = "Michael Cera, Mary Elizabeth Winstead",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 800000, AverageRating = 7.5, RatingCount = 12000,
            //     CreatedAt = now.AddDays(-4000), UpdatedAt = now.AddDays(-4000)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Khoa học Viễn tưởng (comic adaptation)
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });

            // // 27. Room
            // movieIdCounter++;
            // movies.Add(new Movie
            // {
            //     Id = movieIdCounter, Title = "Room", EnglishTitle = "Room",
            //     Description = "Một bà mẹ trẻ và con trai nhỏ của cô bị giam cầm trong một căn phòng nhỏ...",
            //     ReleaseYear = 2015, CountryId = 9, GenreId = 7, // Chính Kịch
            //     Director = "Lenny Abrahamson", Cast = "Brie Larson, Jacob Tremblay",
            //     PosterPath = $"/Uploads/images/{movieIdCounter}.png", PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
            //     PosterBanner = $"/Uploads/images/{movieIdCounter}.png", Poster = $"/Uploads/images/{movieIdCounter}.png",
            //     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
            //     TotalEpisodes = 2, IsCompleted = true, Views = 700000, AverageRating = 8.1, RatingCount = 10000,
            //     CreatedAt = now.AddDays(-3600), UpdatedAt = now.AddDays(-3600)
            // });
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
            // movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 9 }); // Tâm Lý
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 1, Title = "Tập 1", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep1.mp4", Duration = durationPerEpisode });
            // episodes.Add(new Episode { Id = ++episodeIdCounter, MovieId = movieIdCounter, EpisodeNumber = 2, Title = "Tập 2", VideoPath = $"/Uploads/videos/{movieIdCounter}_ep2.mp4", Duration = durationPerEpisode });


            // =================================================================
            // APPLYING SEED DATA TO MODELBUILDER
            // =================================================================
            modelBuilder.Entity<Movie>().HasData(movies);
            modelBuilder.Entity<Episode>().HasData(episodes);
            modelBuilder.Entity<MovieGenre>().HasData(movieGenres);
        }
    }
}