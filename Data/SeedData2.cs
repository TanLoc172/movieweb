// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using MovieWebsite.Models; // Giả định Model của bạn nằm trong namespace này

// namespace MovieWebsite.Data
// {
//     public static class SeedData
//     {
//         public static void Seed(ModelBuilder modelBuilder)
//         {
//             // =================================================================
//             // SEED COUNTRIES & GENRES (Giữ nguyên)
//             // =================================================================
//             modelBuilder
//                 .Entity<Country>()
//                 .HasData(
//                     new Country
//                     {
//                         Id = 1,
//                         Name = "Việt Nam",
//                         Code = "VN",
//                     },
//                     new Country
//                     {
//                         Id = 2,
//                         Name = "Hàn Quốc",
//                         Code = "KR",
//                     },
//                     new Country
//                     {
//                         Id = 3,
//                         Name = "Trung Quốc",
//                         Code = "CN",
//                     },
//                     new Country
//                     {
//                         Id = 4,
//                         Name = "Nhật Bản",
//                         Code = "JP",
//                     },
//                     new Country
//                     {
//                         Id = 5,
//                         Name = "Mỹ",
//                         Code = "US",
//                     },
//                     new Country
//                     {
//                         Id = 6,
//                         Name = "Thái Lan",
//                         Code = "TH",
//                     },
//                     new Country
//                     {
//                         Id = 7,
//                         Name = "Anh",
//                         Code = "UK",
//                     },
//                     new Country
//                     {
//                         Id = 8,
//                         Name = "Pháp",
//                         Code = "FR",
//                     },
//                     new Country
//                     {
//                         Id = 9,
//                         Name = "Canada",
//                         Code = "CA",
//                     }
//                 );

//             modelBuilder
//                 .Entity<Genre>()
//                 .HasData(
//                     new Genre
//                     {
//                         Id = 1,
//                         Name = "Hành Động",
//                         Color = "#FF5722",
//                     },
//                     new Genre
//                     {
//                         Id = 2,
//                         Name = "Tình Cảm",
//                         Color = "#E91E63",
//                     },
//                     new Genre
//                     {
//                         Id = 3,
//                         Name = "Hài Hước",
//                         Color = "#FFC107",
//                     },
//                     new Genre
//                     {
//                         Id = 4,
//                         Name = "Kinh Dị",
//                         Color = "#424242",
//                     },
//                     new Genre
//                     {
//                         Id = 5,
//                         Name = "Khoa Học Viễn Tưởng",
//                         Color = "#2196F3",
//                     },
//                     new Genre
//                     {
//                         Id = 6,
//                         Name = "Phiêu Lưu",
//                         Color = "#4CAF50",
//                     },
//                     new Genre
//                     {
//                         Id = 7,
//                         Name = "Chính Kịch",
//                         Color = "#9C27B0",
//                     },
//                     new Genre
//                     {
//                         Id = 8,
//                         Name = "Hoạt Hình",
//                         Color = "#FF9800",
//                     },
//                     new Genre
//                     {
//                         Id = 9,
//                         Name = "Tâm Lý",
//                         Color = "#00BCD4",
//                     },
//                     new Genre
//                     {
//                         Id = 10,
//                         Name = "Gia Đình",
//                         Color = "#795548",
//                     }
//                 );

//             // =================================================================
//             // SEED ROLES (Giữ nguyên)
//             // =================================================================
//             var adminRole = new IdentityRole
//             {
//                 Id = Guid.NewGuid().ToString(),
//                 Name = "Admin",
//                 NormalizedName = "ADMIN",
//             };
//             var userRole = new IdentityRole
//             {
//                 Id = Guid.NewGuid().ToString(),
//                 Name = "User",
//                 NormalizedName = "USER",
//             };

//             modelBuilder.Entity<IdentityRole>().HasData(adminRole, userRole);

//             // =================================================================
//             // SEED MOVIES, EPISODES & MOVIEGENRES (Dữ liệu phim được viết chi tiết từng cái)
//             // =================================================================

//             var movies = new List<Movie>();
//             var episodes = new List<Episode>();
//             var movieGenres = new List<MovieGenre>();

//             int movieIdCounter = 0;
//             int episodeIdCounter = 0;
//             var now = DateTime.UtcNow;

//             // --- PHIM VIỆT NAM ---

//             // 1. Mắt Biếc
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Mắt Biếc",
//                     EnglishTitle = "Dreamy Eyes",
//                     Description =
//                         "Chuyện phim kể về mối tình si của chàng trai Ngạn dành cho Hà Lan từ thuở thiếu thời, qua những năm tháng chiến tranh và biến cố cuộc đời, lấy bối cảnh làng Đo Đo yên bình.",
//                     ReleaseYear = 2019,
//                     CountryId = 1, // Việt Nam
//                     GenreId = 2, // Tình Cảm
//                     Director = "Victor Vũ",
//                     Cast = "Trần Nghĩa, Trúc Anh, Trần Phong, Khánh Vân",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1, // Phim lẻ
//                     IsCompleted = true,
//                     Views = 1500000,
//                     AverageRating = 8.5,
//                     RatingCount = 25000,
//                     CreatedAt = now.AddDays(-1200),
//                     UpdatedAt = now.AddDays(-1200),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 9 }); // Tâm Lý
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7500, // Khoảng 2 tiếng 5 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 2. Tiệc Trăng Máu
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Tiệc Trăng Máu",
//                     EnglishTitle = "Full House (Vietnamese)",
//                     Description =
//                         "Một buổi tối tụ họp bạn bè bỗng trở nên căng thẳng khi mọi người quyết định chơi một trò chơi tiết lộ bí mật qua điện thoại, phơi bày những góc khuất ít ai ngờ tới.",
//                     ReleaseYear = 2020,
//                     CountryId = 1, // Việt Nam
//                     GenreId = 3, // Hài Hước
//                     Director = "Nguyễn Quang Dũng",
//                     Cast = "Thái Hòa, Kiều Minh Tuấn, Kaity Nguyễn, Thu Trang, Tiến Luật",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1200000,
//                     AverageRating = 7.8,
//                     RatingCount = 18000,
//                     CreatedAt = now.AddDays(-1000),
//                     UpdatedAt = now.AddDays(-1000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 4 }); // Giật gân / Bí ẩn
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 6000, // Khoảng 1 tiếng 40 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 3. Bố Già
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Bố Già",
//                     EnglishTitle = "Dad, I'm Sorry",
//                     Description =
//                         "Câu chuyện cảm động về tình cha con, những xung đột thế hệ và tình làng nghĩa xóm trong một xóm lao động nghèo tại Sài Gòn, xoay quanh ông Tư và cậu con trai Sang.",
//                     ReleaseYear = 2021,
//                     CountryId = 1, // Việt Nam
//                     GenreId = 10, // Gia Đình
//                     Director = "Vũ Ngọc Đãng",
//                     Cast = "Trấn Thành, Tuấn Trần, Ngọc Giàu, Lê Giang",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 2000000,
//                     AverageRating = 8.8,
//                     RatingCount = 35000,
//                     CreatedAt = now.AddDays(-900),
//                     UpdatedAt = now.AddDays(-900),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình cảm (gia đình)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 8100, // Khoảng 2 tiếng 15 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // --- PHIM TRUNG QUỐC ---

//             // 4. Tam Sinh Tam Thế Thập Lý Đào Hoa (Series)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Tam Sinh Tam Thế Thập Lý Đào Hoa",
//                     EnglishTitle = "Eternal Love",
//                     Description =
//                         "Chuyện tình bi thương kéo dài ba kiếp giữa hồ yêu Bạch Thiển và thượng thần Dạ Hoa giữa chốn thiên giới đầy biến cố, tranh đấu quyền lực và định mệnh nghiệt ngã.",
//                     ReleaseYear = 2017,
//                     CountryId = 3, // Trung Quốc
//                     GenreId = 2, // Tình Cảm
//                     Director = "Lưu Dịch Sơ",
//                     Cast = "Dương Mịch, Triệu Hựu Đình, Địch Lệ Nhiệt Ba, Cao Vỹ Quang",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 58,
//                     IsCompleted = true,
//                     Views = 5000000,
//                     AverageRating = 9.0,
//                     RatingCount = 50000,
//                     CreatedAt = now.AddDays(-2500),
//                     UpdatedAt = now.AddDays(-2500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo (Fantasy)
//             for (int i = 1; i <= 58; i++)
//             {
//                 episodeIdCounter++;
//                 episodes.Add(
//                     new Episode
//                     {
//                         Id = episodeIdCounter,
//                         MovieId = movieIdCounter,
//                         EpisodeNumber = i,
//                         Title =
//                             $"Tập {i}: {(i % 5 == 1 ? "Kiếp Nợ Đầu Tiên" : (i % 5 == 2 ? "Ân Oán Thiên Giới" : (i % 5 == 3 ? "Sự Hy Sinh Vì Tình Yêu" : (i % 5 == 4 ? "Tái Ngộ Trong Mơ" : "Hồi Kết Đau Thương"))))}",
//                         VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                         Duration = 2700, // Khoảng 45 phút
//                         ReleaseDate = movies.Last().CreatedAt.AddDays(i * 3),
//                         CreatedAt = now,
//                         UpdatedAt = now,
//                     }
//                 );
//             }

//             // 5. Diệp Vấn
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Diệp Vấn",
//                     EnglishTitle = "Ip Man",
//                     Description =
//                         "Diệp Vấn, một võ sư Vịnh Xuân quyền tài ba, phải đối mặt với những thử thách khắc nghiệt và sự áp bức của quân Nhật tại Phật Sơn những năm 1930.",
//                     ReleaseYear = 2008,
//                     CountryId = 3, // Trung Quốc
//                     GenreId = 1, // Hành Động
//                     Director = "Diệp Vĩ Tín",
//                     Cast = "Chân Tử Đan, Nhậm Đạt Hoa, Phạm Minh, Lý Trạch Uyển",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 900000,
//                     AverageRating = 8.2,
//                     RatingCount = 15000,
//                     CreatedAt = now.AddDays(-5000),
//                     UpdatedAt = now.AddDays(-5000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu (chiến đấu)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7800, // Khoảng 2 tiếng 10 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 6. Anh Hùng
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Anh Hùng",
//                     EnglishTitle = "Hero",
//                     Description =
//                         "Trong thời Chiến Quốc, một người vô danh (Vô Danh) nhận nhiệm vụ ám sát Tần Thủy Hoàng, nhưng câu chuyện được kể qua nhiều góc nhìn khác nhau.",
//                     ReleaseYear = 2002,
//                     CountryId = 3, // Trung Quốc
//                     GenreId = 1, // Hành Động
//                     Director = "Trương Nghệ Mưu",
//                     Cast = "Lý Liên Kiệt, Chương Tử Di, Trương Mạn Ngọc, Trần Huệ Lâm",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 750000,
//                     AverageRating = 8.0,
//                     RatingCount = 12000,
//                     CreatedAt = now.AddDays(-7000),
//                     UpdatedAt = now.AddDays(-7000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7200, // Khoảng 2 tiếng
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // --- PHIM HÀN QUỐC ---

//             // 7. Ký Sinh Trùng (Parasite)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Ký Sinh Trùng",
//                     EnglishTitle = "Parasite",
//                     Description =
//                         "Một gia đình nghèo từ tầng hầm tìm cách 'ký sinh' vào cuộc sống của một gia đình giàu có, dẫn đến những tình huống trớ trêu và bi kịch không thể lường trước.",
//                     ReleaseYear = 2019,
//                     CountryId = 2, // Hàn Quốc
//                     GenreId = 7, // Chính Kịch
//                     Director = "Bong Joon Ho",
//                     Cast = "Song Kang-ho, Choi Woo-shik, Park So-dam, Cho Yeo-jeong",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1800000,
//                     AverageRating = 9.2,
//                     RatingCount = 28000,
//                     CreatedAt = now.AddDays(-1500),
//                     UpdatedAt = now.AddDays(-1500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài Hước (đen)
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 4 }); // Giật gân
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7500, // Khoảng 2 tiếng 5 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 8. Hậu Duệ Mặt Trời (Series)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Hậu Duệ Mặt Trời",
//                     EnglishTitle = "Descendants of the Sun",
//                     Description =
//                         "Câu chuyện tình yêu lãng mạn và đầy thử thách giữa đại úy Yoo Shi-jin của lực lượng đặc nhiệm và bác sĩ Kang Mo-yeon trong bối cảnh quân sự căng thẳng tại quốc gia hư cấu Uruk.",
//                     ReleaseYear = 2016,
//                     CountryId = 2, // Hàn Quốc
//                     GenreId = 2, // Tình Cảm
//                     Director = "Lee Eung-bok, Baek Sang-hoon",
//                     Cast = "Song Joong-ki, Song Hye-kyo, Kim Ji-won, Jin Goo",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 16,
//                     IsCompleted = true,
//                     Views = 4500000,
//                     AverageRating = 8.9,
//                     RatingCount = 45000,
//                     CreatedAt = now.AddDays(-3000),
//                     UpdatedAt = now.AddDays(-3000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             for (int i = 1; i <= 16; i++)
//             {
//                 episodeIdCounter++;
//                 episodes.Add(
//                     new Episode
//                     {
//                         Id = episodeIdCounter,
//                         MovieId = movieIdCounter,
//                         EpisodeNumber = i,
//                         Title =
//                             $"Tập {i}: {(i % 4 == 1 ? "Mầm Sống Tình Yêu" : (i % 4 == 2 ? "Nơi Trái Tim Thuộc Về" : (i % 4 == 3 ? "Vượt Qua Bão Tố" : "Hạnh Phúc Vỡ Òa")))}",
//                         VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                         Duration = 3000, // Khoảng 50 phút
//                         ReleaseDate = movies.Last().CreatedAt.AddDays(i * 5),
//                         CreatedAt = now,
//                         UpdatedAt = now,
//                     }
//                 );
//             }

//             // 9. Goblin (Yêu Tinh)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Goblin",
//                     EnglishTitle = "Guardian: The Lonely and Great God",
//                     Description =
//                         "Câu chuyện về Kim Shin, một yêu tinh bất tử hơn 900 năm tuổi tìm kiếm cô dâu của mình để kết thúc lời nguyền và tìm thấy sự bình yên, cùng mối tình định mệnh với nữ sinh trung học.",
//                     ReleaseYear = 2016,
//                     CountryId = 2, // Hàn Quốc
//                     GenreId = 2, // Tình Cảm
//                     Director = "Lee Eung-bok",
//                     Cast = "Gong Yoo, Kim Go-eun, Lee Dong-wook, Yoo In-na, Yook Sung-jae",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 16,
//                     IsCompleted = true,
//                     Views = 4000000,
//                     AverageRating = 9.1,
//                     RatingCount = 40000,
//                     CreatedAt = now.AddDays(-3000),
//                     UpdatedAt = now.AddDays(-3000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo (Fantasy)
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             for (int i = 1; i <= 16; i++)
//             {
//                 episodeIdCounter++;
//                 episodes.Add(
//                     new Episode
//                     {
//                         Id = episodeIdCounter,
//                         MovieId = movieIdCounter,
//                         EpisodeNumber = i,
//                         Title =
//                             $"Tập {i}: {(i % 4 == 1 ? "Ký Ức Bị Lãng Quên" : (i % 4 == 2 ? "Lời Hẹn Ước Xưa" : (i % 4 == 3 ? "Số Phận Trêu Ngươi" : "Bình Minh Của Tình Yêu")))}",
//                         VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                         Duration = 3000, // Khoảng 50 phút
//                         ReleaseDate = movies.Last().CreatedAt.AddDays(i * 5),
//                         CreatedAt = now,
//                         UpdatedAt = now,
//                     }
//                 );
//             }

//             // 10. Chuyến Tàu Sinh Tử (Train to Busan)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Chuyến Tàu Sinh Tử",
//                     EnglishTitle = "Train to Busan",
//                     Description =
//                         "Khi một dịch bệnh zombie bùng phát dữ dội, những hành khách trên chuyến tàu cao tốc từ Seoul đến Busan phải chiến đấu tuyệt vọng để sinh tồn trước sự tấn công của xác sống.",
//                     ReleaseYear = 2016,
//                     CountryId = 2, // Hàn Quốc
//                     GenreId = 1, // Hành Động
//                     Director = "Yeon Sang-ho",
//                     Cast = "Gong Yoo, Ma Dong-seok, Jung Yu-mi, Choi Woo-shik",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1100000,
//                     AverageRating = 8.0,
//                     RatingCount = 19000,
//                     CreatedAt = now.AddDays(-2800),
//                     UpdatedAt = now.AddDays(-2800),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 4 }); // Kinh Dị
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7000, // Khoảng 1 tiếng 56 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // --- PHIM MỸ/ANH ---

//             // 11. Kỵ Sĩ Bóng Đêm (The Dark Knight)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Kỵ Sĩ Bóng Đêm",
//                     EnglishTitle = "The Dark Knight",
//                     Description =
//                         "Batman phải đối mặt với tên tội phạm Joker đầy điên loạn và mưu mô, buộc anh phải đưa ra những lựa chọn khó khăn để bảo vệ Gotham khỏi hỗn loạn.",
//                     ReleaseYear = 2008,
//                     CountryId = 5, // Mỹ
//                     GenreId = 1, // Hành Động
//                     Director = "Christopher Nolan",
//                     Cast = "Christian Bale, Heath Ledger, Aaron Eckhart, Michael Caine",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 2500000,
//                     AverageRating = 9.0,
//                     RatingCount = 40000,
//                     CreatedAt = now.AddDays(-5500),
//                     UpdatedAt = now.AddDays(-5500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Siêu Anh Hùng (Superhero)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 8000, // Khoảng 2 tiếng 15 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 12. Kẻ Đánh Cắp Giấc Mơ (Inception)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Kẻ Đánh Cắp Giấc Mơ",
//                     EnglishTitle = "Inception",
//                     Description =
//                         "Một đạo chích chuyên xâm nhập vào giấc mơ của người khác để đánh cắp bí mật, được giao nhiệm vụ 'cấy' một ý tưởng vào tiềm thức của một CEO.",
//                     ReleaseYear = 2010,
//                     CountryId = 5, // Mỹ
//                     GenreId = 5, // Khoa Học Viễn Tưởng
//                     Director = "Christopher Nolan",
//                     Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Tom Hardy",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 2200000,
//                     AverageRating = 8.8,
//                     RatingCount = 38000,
//                     CreatedAt = now.AddDays(-5000),
//                     UpdatedAt = now.AddDays(-5000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7900, // Khoảng 2 tiếng 14 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 13. Forrest Gump
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Forrest Gump",
//                     EnglishTitle = "Forrest Gump",
//                     Description =
//                         "Cuộc đời phi thường của một người đàn ông có chỉ số IQ thấp nhưng trái tim nhân hậu, vô tình chứng kiến và ảnh hưởng đến nhiều sự kiện lịch sử quan trọng của nước Mỹ thế kỷ 20.",
//                     ReleaseYear = 1994,
//                     CountryId = 5, // Mỹ
//                     GenreId = 7, // Chính Kịch
//                     Director = "Robert Zemeckis",
//                     Cast = "Tom Hanks, Robin Wright, Gary Sinise, Sally Field",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1900000,
//                     AverageRating = 8.8,
//                     RatingCount = 32000,
//                     CreatedAt = now.AddDays(-10000),
//                     UpdatedAt = now.AddDays(-10000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia Đình
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 8800, // Khoảng 2 tiếng 22 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 14. Chúa Tể Của Những Chiếc Nhẫn: Hiệp Hội Nhẫn Thân
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Chúa Tể Của Những Chiếc Nhẫn: Hiệp Hội Nhẫn Thân",
//                     EnglishTitle = "The Lord of the Rings: The Fellowship of the Ring",
//                     Description =
//                         "Cuộc hành trình của người Hobbit Frodo Baggins cùng các đồng minh kỳ lạ trên con đường phá hủy Chiếc Nhẫn Quyền Lực để cứu Trung Địa khỏi bóng tối của Sauron.",
//                     ReleaseYear = 2001,
//                     CountryId = 7, // Anh (co-production with NZ)
//                     GenreId = 6, // Phiêu Lưu
//                     Director = "Peter Jackson",
//                     Cast = "Elijah Wood, Ian McKellen, Viggo Mortensen, Orlando Bloom",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1600000,
//                     AverageRating = 8.8,
//                     RatingCount = 29000,
//                     CreatedAt = now.AddDays(-8000),
//                     UpdatedAt = now.AddDays(-8000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Giả tưởng (Fantasy)
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 9500, // Khoảng 2 tiếng 38 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 15. Hố Đen Tử Thần (Interstellar)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Hố Đen Tử Thần",
//                     EnglishTitle = "Interstellar",
//                     Description =
//                         "Trong tương lai, Trái Đất đang dần cạn kiệt tài nguyên và sắp diệt vong. Một nhóm phi hành gia thực hiện nhiệm vụ xuyên không gian tìm kiếm ngôi nhà mới cho nhân loại.",
//                     ReleaseYear = 2014,
//                     CountryId = 5, // Mỹ
//                     GenreId = 5, // Khoa Học Viễn Tưởng
//                     Director = "Christopher Nolan",
//                     Cast = "Matthew McConaughey, Anne Hathaway, Jessica Chastain, Michael Caine",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1700000,
//                     AverageRating = 8.6,
//                     RatingCount = 30000,
//                     CreatedAt = now.AddDays(-4000),
//                     UpdatedAt = now.AddDays(-4000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 9000, // Khoảng 2 tiếng 49 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // --- PHIM CÁC NƯỚC KHÁC ---

//             // 16. Tên Cậu Là Gì? (Your Name) - Nhật Bản
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Tên Cậu Là Gì?",
//                     EnglishTitle = "Your Name.",
//                     Description =
//                         "Hai thiếu niên, một cô gái ở vùng nông thôn và một chàng trai ở Tokyo, phát hiện họ trao đổi cơ thể một cách bí ẩn mỗi khi ngủ, tạo nên những câu chuyện dở khóc dở cười và cảm động.",
//                     ReleaseYear = 2016,
//                     CountryId = 4, // Nhật Bản
//                     GenreId = 2, // Tình Cảm
//                     Director = "Makoto Shinkai",
//                     Cast = "Ryunosuke Kamiki (lồng tiếng), Mone Kamishiraishi (lồng tiếng)",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1300000,
//                     AverageRating = 8.9,
//                     RatingCount = 22000,
//                     CreatedAt = now.AddDays(-2600),
//                     UpdatedAt = now.AddDays(-2600),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 6800, // Khoảng 1 tiếng 47 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 17. Những Kẻ Trộm Vặt (Shoplifters) - Nhật Bản
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Những Kẻ Trộm Vặt",
//                     EnglishTitle = "Shoplifters",
//                     Description =
//                         "Một gia đình sống bằng nghề trộm cắp nhỏ bé và những công việc lặt vặt tại Tokyo tìm thấy một cô bé bị bỏ rơi, dẫn đến những quyết định đạo đức phức tạp.",
//                     ReleaseYear = 2018,
//                     CountryId = 4, // Nhật Bản
//                     GenreId = 7, // Chính Kịch
//                     Director = "Hirokazu Kore-eda",
//                     Cast = "Lily Franky, Sakura Ando, Mayu Matsuoka, Sôsuke Ikematsu",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 400000,
//                     AverageRating = 8.3,
//                     RatingCount = 7000,
//                     CreatedAt = now.AddDays(-2000),
//                     UpdatedAt = now.AddDays(-2000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia Đình
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Tình huống (dở khóc dở cười)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7500, // Khoảng 2 tiếng 5 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 18. Thiên Tài Bất Hảo (Bad Genius) - Thái Lan
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Thiên Tài Bất Hảo",
//                     EnglishTitle = "Bad Genius",
//                     Description =
//                         "Một nữ sinh thiên tài lập kế hoạch gian lận thi cử tinh vi với quy mô quốc tế, nhưng hành động của cô ngày càng liều lĩnh và đầy rủi ro.",
//                     ReleaseYear = 2017,
//                     CountryId = 6, // Thái Lan
//                     GenreId = 7, // Chính Kịch
//                     Director = "Nattawut Poonpiriya",
//                     Cast =
//                         "Chutimon Chuengcharoensukying, Non Chanon, Thiti Mahayotaruk, Eisaya Hosuwan",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 600000,
//                     AverageRating = 7.5,
//                     RatingCount = 9000,
//                     CreatedAt = now.AddDays(-2500),
//                     UpdatedAt = now.AddDays(-2500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Giật gân / Thriller
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 3 }); // Hài hước (tình huống)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 5800, // Khoảng 1 tiếng 38 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 19. Amélie (Pháp)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Amélie",
//                     EnglishTitle = "Amélie",
//                     Description =
//                         "Câu chuyện về Amélie Poulain, một cô gái làm bồi bàn ở Paris, người bí mật thực hiện những hành động nhỏ bé đầy ý nghĩa để mang lại hạnh phúc cho những người xung quanh.",
//                     ReleaseYear = 2001,
//                     CountryId = 8, // Pháp
//                     GenreId = 3, // Hài Hước
//                     Director = "Jean-Pierre Jeunet",
//                     Cast = "Audrey Tautou, Mathieu Kassovitz",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 500000,
//                     AverageRating = 8.3,
//                     RatingCount = 10000,
//                     CreatedAt = now.AddDays(-8000),
//                     UpdatedAt = now.AddDays(-8000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch (nhẹ nhàng)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 5700, // Khoảng 1 tiếng 37 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 20. Harry Potter và Hòn Đá Phù Thủy
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Harry Potter Và Hòn Đá Phù Thủy",
//                     EnglishTitle = "Harry Potter and the Sorcerer's Stone",
//                     Description =
//                         "Harry Potter khám phá mình là một phù thủy và bắt đầu hành trình học tập tại Trường Hogwarts danh tiếng, nơi cậu kết bạn và đối mặt với những bí ẩn đầu tiên.",
//                     ReleaseYear = 2001,
//                     CountryId = 5, // Mỹ (coprod with UK)
//                     GenreId = 6, // Phiêu Lưu (thay thế cho Hoạt Hình không phù hợp)
//                     Director = "Chris Columbus",
//                     Cast = "Daniel Radcliffe, Rupert Grint, Emma Watson, Richard Harris",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 2800000,
//                     AverageRating = 7.6,
//                     RatingCount = 35000,
//                     CreatedAt = now.AddDays(-8000),
//                     UpdatedAt = now.AddDays(-8000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Giả tưởng (Fantasy)
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia đình
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7300, // Khoảng 2 tiếng 1 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 21. Nhà Tù Shawshank (The Shawshank Redemption) - Mỹ
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Nhà Tù Shawshank",
//                     EnglishTitle = "The Shawshank Redemption",
//                     Description =
//                         "Một kế toán bị kết án oan tù chung thân tìm cách vượt ngục khỏi nhà tù Shawshank khắc nghiệt, đồng thời nuôi dưỡng hy vọng và tình bạn.",
//                     ReleaseYear = 1994,
//                     CountryId = 5, // Mỹ
//                     GenreId = 7, // Chính Kịch
//                     Director = "Frank Darabont",
//                     Cast = "Tim Robbins, Morgan Freeman, Bob Gunton",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1500000,
//                     AverageRating = 9.3,
//                     RatingCount = 38000,
//                     CreatedAt = now.AddDays(-11000),
//                     UpdatedAt = now.AddDays(-11000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 2 }); // Tình Cảm (tình bạn)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 8500, // Khoảng 2 tiếng 22 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 22. Vùng Đất Linh Hồn (Spirited Away) - Nhật Bản
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Vùng Đất Linh Hồn",
//                     EnglishTitle = "Spirited Away",
//                     Description =
//                         "Một cô bé 10 tuổi tên Chihiro bị lạc vào thế giới của các vị thần và linh hồn, nơi cô phải làm việc để giải cứu cha mẹ mình khỏi lời nguyền.",
//                     ReleaseYear = 2001,
//                     CountryId = 4, // Nhật Bản
//                     GenreId = 8, // Hoạt Hình
//                     Director = "Hayao Miyazaki",
//                     Cast =
//                         "Rumi Hiiragi (lồng tiếng), Miyu Irino (lồng tiếng), Mari Natsuki (lồng tiếng)",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1400000,
//                     AverageRating = 8.6,
//                     RatingCount = 24000,
//                     CreatedAt = now.AddDays(-8000),
//                     UpdatedAt = now.AddDays(-8000),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 5 }); // Kỳ ảo
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7000, // Khoảng 1 tiếng 53 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 23. Những Kẻ Bất Khả Xâm Phạm (The Intouchables) - Pháp
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Những Kẻ Bất Khả Xâm Phạm",
//                     EnglishTitle = "The Intouchables",
//                     Description =
//                         "Câu chuyện có thật đầy cảm hứng về tình bạn kỳ lạ giữa một nhà tài phiệt giàu có bị liệt toàn thân và người chăm sóc là một thanh niên đến từ khu ổ chuột Paris.",
//                     ReleaseYear = 2011,
//                     CountryId = 8, // Pháp
//                     GenreId = 3, // Hài Hước
//                     Director = "Olivier Nakache, Éric Toledano",
//                     Cast = "François Cluzet, Omar Sy, Anne Le Ny",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 950000,
//                     AverageRating = 8.5,
//                     RatingCount = 16000,
//                     CreatedAt = now.AddDays(-4500),
//                     UpdatedAt = now.AddDays(-4500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia đình (tình bạn)
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 6900, // Khoảng 1 tiếng 52 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 24. Người Về Từ Cõi Chết (The Revenant) - Mỹ/Canada
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Người Về Từ Cõi Chết",
//                     EnglishTitle = "The Revenant",
//                     Description =
//                         "Trong vùng đất hoang dã khắc nghiệt của thế kỷ 19, một người thợ săn bị bỏ mặc cho chết sau cuộc tấn công của gấu và phải chiến đấu phi thường để sống sót và trả thù.",
//                     ReleaseYear = 2015,
//                     CountryId = 5, // Mỹ
//                     GenreId = 6, // Phiêu Lưu
//                     Director = "Alejandro G. Iñárritu",
//                     Cast = "Leonardo DiCaprio, Tom Hardy, Domhnall Gleeson",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1100000,
//                     AverageRating = 8.0,
//                     RatingCount = 20000,
//                     CreatedAt = now.AddDays(-3500),
//                     UpdatedAt = now.AddDays(-3500),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 1 }); // Hành Động
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 7 }); // Chính Kịch
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7500, // Khoảng 2 tiếng 36 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // 25. Coco - Mỹ (Hoạt Hình)
//             movieIdCounter++;
//             movies.Add(
//                 new Movie
//                 {
//                     Id = movieIdCounter,
//                     Title = "Coco",
//                     EnglishTitle = "Coco",
//                     Description =
//                         "Một cậu bé đam mê âm nhạc bị cấm đoán khám phá bí mật về Lễ hội Những Người Chết đầy màu sắc ở Mexico, gặp gỡ tổ tiên và những bài học về gia đình.",
//                     ReleaseYear = 2017,
//                     CountryId = 5, // Mỹ
//                     GenreId = 8, // Hoạt Hình
//                     Director = "Lee Unkrich",
//                     Cast =
//                         "Anthony Gonzalez (lồng tiếng), Gael García Bernal (lồng tiếng), Benjamin Bratt (lồng tiếng)",
//                     PosterPath = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterDoc = $"/Uploads/images/{movieIdCounter}.png",
//                     PosterBanner = $"/Uploads/images/{movieIdCounter}.png",
//                     Poster = $"/Uploads/images/{movieIdCounter}.png",
//                     TrailerPath = $"/Uploads/trailer/{movieIdCounter}.mp4",
//                     TotalEpisodes = 1,
//                     IsCompleted = true,
//                     Views = 1700000,
//                     AverageRating = 8.5,
//                     RatingCount = 27000,
//                     CreatedAt = now.AddDays(-2400),
//                     UpdatedAt = now.AddDays(-2400),
//                 }
//             );
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 6 }); // Phiêu Lưu
//             movieGenres.Add(new MovieGenre { MovieId = movieIdCounter, GenreId = 10 }); // Gia đình
//             episodeIdCounter++;
//             episodes.Add(
//                 new Episode
//                 {
//                     Id = episodeIdCounter,
//                     MovieId = movieIdCounter,
//                     EpisodeNumber = 1,
//                     Title = "Trọn Bộ",
//                     VideoPath = $"/Uploads/videos/{movieIdCounter}.mp4",
//                     Duration = 7500, // Khoảng 1 tiếng 45 phút
//                     ReleaseDate = movies.Last().CreatedAt,
//                     CreatedAt = now,
//                     UpdatedAt = now,
//                 }
//             );

//             // =================================================================
//             // APPLYING SEED DATA TO MODELBUILDER
//             // =================================================================
//             modelBuilder.Entity<Movie>().HasData(movies);
//             modelBuilder.Entity<Episode>().HasData(episodes);
//             modelBuilder.Entity<MovieGenre>().HasData(movieGenres);
//         }
//     }
// }
