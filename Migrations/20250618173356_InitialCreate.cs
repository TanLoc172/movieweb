using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    FlagPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EnglishTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false),
                    Director = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Cast = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    PosterPath = table.Column<string>(type: "TEXT", nullable: false),
                    TrailerPath = table.Column<string>(type: "TEXT", nullable: false),
                    TotalEpisodes = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageRating = table.Column<double>(type: "REAL", nullable: false),
                    RatingCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    VideoPath = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Review = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ParentCommentId = table.Column<int>(type: "INTEGER", nullable: true),
                    Likes = table.Column<int>(type: "INTEGER", nullable: false),
                    Dislikes = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "FlagPath", "Name" },
                values: new object[,]
                {
                    { 1, "VN", null, "Việt Nam" },
                    { 2, "KR", null, "Hàn Quốc" },
                    { 3, "CN", null, "Trung Quốc" },
                    { 4, "JP", null, "Nhật Bản" },
                    { 5, "US", null, "Mỹ" },
                    { 6, "TH", null, "Thái Lan" },
                    { 7, "UK", null, "Anh" },
                    { 8, "FR", null, "Pháp" },
                    { 9, "CA", null, "Canada" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Color", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "#FF5722", null, "Hành Động" },
                    { 2, "#E91E63", null, "Tình Cảm" },
                    { 3, "#FFC107", null, "Hài Hước" },
                    { 4, "#424242", null, "Kinh Dị" },
                    { 5, "#2196F3", null, "Khoa Học Viễn Tưởng" },
                    { 6, "#4CAF50", null, "Phiêu Lưu" },
                    { 7, "#9C27B0", null, "Chính Kịch" },
                    { 8, "#FF9800", null, "Hoạt Hình" },
                    { 9, "#00BCD4", null, "Tâm Lý" },
                    { 10, "#795548", null, "Kinh Dị Gia Đình" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AverageRating", "Cast", "CountryId", "CreatedAt", "Description", "Director", "EnglishTitle", "GenreId", "IsCompleted", "PosterPath", "RatingCount", "ReleaseYear", "Title", "TotalEpisodes", "TrailerPath", "UpdatedAt", "Views" },
                values: new object[,]
                {
                    { 1, 8.5, "Trấn Thành, Tuấn Trần, Ngô Kiến Huy", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7550), "Bộ phim kể về cuộc sống của một gia đình lao động nghèo ở Sài Gòn.", "Trấn Thành", "Old Father", 7, true, "/images/1.png", 1200, 2021, "Bố Già", 1, "/videos/1.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7550), 150000 },
                    { 2, 9.0, "Hyun Bin, Son Ye-jin", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7560), "Câu chuyện tình yêu giữa một nữ thừa kế Hàn Quốc và một sĩ quan Bắc Triều Tiên.", "Lee Jeong-hyo", "Crash Landing on You", 2, true, "/images/2.png", 2000, 2019, "Hạ Cánh Nơi Anh", 16, "/videos/2.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7560), 250000 },
                    { 3, 9.1999999999999993, "Song Kang-ho, Lee Sun-kyun, Cho Yeo-jeong", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7570), "Một gia đình nghèo tìm cách thâm nhập vào cuộc sống của một gia đình giàu có.", "Bong Joon-ho", "Parasite", 7, true, "/images/3.png", 2500, 2019, "Ký Sinh Trùng", 1, "/videos/3.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7570), 300000 },
                    { 4, 7.7999999999999998, "Hạo Khang, Trấn Thành, Mai Cát Vi", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7990), "Câu chuyện về hành trình trưởng thành của bé An tại miền Nam Việt Nam.", "Võ Thanh Hòa", "Southern Forest Land", 6, true, "/images/4.png", 800, 2023, "Đất Rừng Phương Nam", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7990), 120000 },
                    { 5, 7.5, "Trấn Thành, Uyển Ân, Ngọc Châu", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8110), "Câu chuyện về mâu thuẫn gia đình và tình yêu.", "Trấn Thành", "The House of No Man", 2, true, "/images/5.png", 1500, 2023, "Nhà Bà Nữ", 1, "/videos/5.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8110), 180000 },
                    { 6, 7.2000000000000002, "Thái Hòa, Thanh Thức, Huỳnh Đông", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8140), "Cuộc phiêu lưu để giành tấm vé số định mệnh.", "Lý Hải", "Face Off 6: The Destiny Ticket", 3, true, "/images/6.png", 1300, 2023, "Lật Mặt 6: Tấm Vé Định Mệnh", 1, "/videos/6.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8140), 150000 },
                    { 7, 7.0, "Thái Hòa, Kiều Minh Tuấn, Đức Thịnh", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8190), "Những bí mật của một nhóm bạn được phơi bày qua một trò chơi.", "Nguyễn Quang Dũng", "Full House", 3, true, "/images/7.png", 1200, 2020, "Tiệc Trăng Máu", 1, "/videos/8.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8190), 130000 },
                    { 8, 6.7999999999999998, "Kaity Nguyễn, Will", 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8210), "Câu chuyện tình yêu tuổi teen với nhiều tình huống dở khóc dở cười.", "Lê Thanh Sơn", "Sweet 18", 2, true, "/images/3.png", 1000, 2017, "Em Chưa 18", 1, "/videos/32.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8210), 110000 },
                    { 9, 8.6999999999999993, "Suzu Hirose, Kôki Mitsushima", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8240), "Một câu chuyện cổ tích về tình yêu và băng giá.", "Hayao Miyazaki", "Frozen Bride", 2, true, "/images/7.png", 1600, 2020, "Cô Dâu Băng Giá", 1, "/videos/7.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8240), 190000 },
                    { 10, 8.5, "Baifern Pimchanok, Push Puttichai", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8260), "Cuộc đời đầy biến cố của một cô gái chuyển giới.", "Sopon Sukapisoot", "Bai Mai Tee Plid Plew", 7, true, "/images/8.png", 3500, 2019, "Chiếc Lá Cuốn Bay", 20, "/videos/8.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8260), 400000 },
                    { 11, 9.0, "Gong Yoo, Kim Go-eun, Lee Dong-wook", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8350), "Câu chuyện tình yêu giữa một goblin và một cô dâu loài người.", "Kim Eun-sook", "Guardian: The Lonely and Great God", 2, true, "/images/1.png", 2500, 2016, "Goblin", 16, "/videos/9.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8350), 300000 },
                    { 12, 9.1999999999999993, "Song Hye-kyo, Lee Do-hyun", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8430), "Một cô gái tìm cách trả thù những kẻ đã bắt nạt mình.", "Ahn Gil-ho", "The Glory", 7, true, "/images/3.png", 3000, 2022, "Vinh Quang Trong Thù Hận", 16, "/videos/33.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8430), 350000 },
                    { 13, 9.3000000000000007, "Jo Jung-suk, Yoo Yeon-seok, Jung Kyung-ho", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8510), "Cuộc sống và tình bạn của 5 bác sĩ.", "Shin Won-ho", "Hospital Playlist", 2, true, "/images/4.png", 2300, 2020, "Hospital Playlist", 24, "/videos/34.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8510), 280000 },
                    { 14, 9.4000000000000004, "Lee Hye-ri, Ryu Jun-yeol, Go Kyung-pyo", 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8620), "Câu chuyện về tình bạn và gia đình tại Seoul năm 1988.", "Shin Won-ho", "Reply 1988", 2, true, "/images/5.png", 2400, 2015, "Reply 1988", 20, "/videos/35.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8620), 290000 },
                    { 15, 7.0, "Wu Jing, Qing Xu", 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8690), "Nhân loại di chuyển Trái Đất ra khỏi hệ Mặt Trời.", "Frant Gwo", "The Wandering Earth", 5, true, "/images/1.png", 2800, 2019, "The Wandering Earth", 1, "/videos/10.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8700), 320000 },
                    { 16, 8.1999999999999993, "Châu Nhuận Phát, Dương Tử Quỳnh, Chương Tử Di", 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8720), "Kiếm hiệp tình duyên với những màn võ thuật đẹp mắt.", "Lý An", "Crouching Tiger, Hidden Dragon", 1, true, "/images/11.png", 2000, 2000, "Ngoạ Hổ Tàng Long", 1, "/videos/11.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8720), 250000 },
                    { 17, 8.8000000000000007, "Dương Mịch, Triệu Hựu Đình", 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8760), "Chuyện tình xuyên không gian và thời gian.", "Lâm Ngọc Phượng", "Eternal Love", 2, true, "/images/2.png", 4000, 2017, "Tam Sinh Tam Thế Thập Lý Đào Hoa", 58, "/videos/12.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8760), 450000 },
                    { 18, 7.5, "Ngô Kinh, Frank Grillo", 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8840), "Người lính giải cứu công dân Trung Quốc ở châu Phi.", "Ngô Kinh", "Wolf Warrior 2", 1, true, "/images/36.png", 3200, 2017, "Chiến Lang 2", 1, "/videos/6.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8840), 380000 },
                    { 19, 9.3000000000000007, "Ryunosuke Kamiki, Mone Kamishiraishi", 4, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8870), "Câu chuyện tình yêu kỳ diệu giữa hai người xa lạ đổi linh hồn.", "Makoto Shinkai", "Kimi no Na wa.", 2, true, "/images/3.png", 3800, 2016, "Your Name", 1, "/videos/13.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8870), 420000 },
                    { 20, 9.1999999999999993, "Rumi Hiiragi, Miyu Irino", 4, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8890), "Hành trình phiêu lưu của cô bé Chihiro tại thế giới linh hồn.", "Hayao Miyazaki", "Sen to Chihiro no Kamikakushi", 8, true, "/images/4.png", 3600, 2001, "Spirited Away", 1, "/videos/14.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8890), 380000 },
                    { 21, 8.0, "Mayumi Tanaka, Kazuya Nakai", 4, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8910), "Chuyến phiêu lưu âm nhạc của nhóm Mũ Rơm.", "Goro Taniguchi", "One Piece Film: Red", 8, true, "/images/15.png", 2500, 2022, "One Piece Film: Red", 1, "/videos/5.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8910), 300000 },
                    { 22, 8.5, "Haruka Misaki, Kousei Arima", 4, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8940), "Câu chuyện tình cảm học đường về âm nhạc và tình yêu.", "Hiroshi Nishikori", "Shigatsu wa Kimi no Uso", 2, true, "/images/7.png", 1900, 2016, "Your Lie in April", 22, "/videos/37.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8940), 220000 },
                    { 23, 9.5, "Robert Downey Jr., Chris Evans, Mark Ruffalo", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9050), "Cuộc chiến cuối cùng của các siêu anh hùng chống lại Thanos.", "Anthony Russo, Joe Russo", "Avengers: Endgame", 1, true, "/images/6.png", 5000, 2019, "Avengers: Endgame", 1, "/videos/16.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9050), 500000 },
                    { 24, 9.0, "Joaquin Phoenix, Robert De Niro", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9070), "Nguồn gốc của gã hề phản diện Joker.", "Todd Phillips", "Joker", 7, true, "/images/17.png", 4000, 2019, "Joker", 1, "/videos/7.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9070), 450000 },
                    { 25, 6.5, "Vin Diesel, Michelle Rodriguez", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9090), "Dom và gia đình đối mặt với kẻ thù mới.", "Justin Lin", "F9", 1, true, "/images/18.png", 2500, 2021, "Fast & Furious 9", 1, "/videos/8.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9100), 300000 },
                    { 26, 9.0, "Tom Holland, Zendaya, Benedict Cumberbatch", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9120), "Spider-Man đối mặt với đa vũ trụ.", "Jon Watts", "Spider-Man: No Way Home", 1, true, "/images/1.png", 5200, 2021, "Spider-Man: No Way Home", 1, "/videos/19.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9120), 550000 },
                    { 27, 8.0, "Timothée Chalamet, Rebecca Ferguson", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9140), "Hành trình của Paul Atreides tại hành tinh Arrakis.", "Denis Villeneuve", "Dune", 5, true, "/images/2.png", 3000, 2021, "Dune", 1, "/videos/2.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9140), 350000 },
                    { 28, 8.0, "Stephanie Beatriz, María Cecilia Botero", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9160), "Gia đình Madrigal với phép thuật.", "Jared Bush, Byron Howard", "Encanto", 8, true, "/images/1.png", 2900, 2021, "Encanto", 1, "/videos/2.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9170), 330000 },
                    { 29, 7.0, "Donald Glover, Beyoncé, Seth Rogen", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9190), "Câu chuyện về chú sư tử Simba.", "Jon Favreau", "The Lion King", 8, true, "/images/4.png", 3100, 2019, "The Lion King", 1, "/videos/22.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9190), 360000 },
                    { 30, 9.5999999999999996, "Christian Bale, Heath Ledger, Aaron Eckhart", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9210), "Batman đối đầu với Joker.", "Christopher Nolan", "The Dark Knight", 1, true, "/images/8.png", 4500, 2008, "The Dark Knight", 1, "/videos/38.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9210), 480000 },
                    { 31, 9.3000000000000007, "Leonardo DiCaprio, Joseph Gordon-Levitt", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9240), "Cuộc hành trình vào thế giới giấc mơ.", "Christopher Nolan", "Inception", 5, true, "/images/3.png", 4200, 2010, "Inception", 1, "/videos/39.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9240), 460000 },
                    { 32, 9.0999999999999996, "Tom Hanks, Robin Wright", 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9260), "Cuộc đời của một người đàn ông đặc biệt.", "Robert Zemeckis", "Forrest Gump", 2, true, "/images/40.png", 3800, 1994, "Forrest Gump", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9260), 400000 },
                    { 33, 7.7999999999999998, "Chutimon Chuengcharoensukying, Non Chanon Santinatornkul", 6, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9280), "Câu chuyện về những học sinh thiên tài gian lận thi cử.", "Nattawut Poonpiriya", "Chalard Games Goeng", 7, true, "/images/3.png", 1800, 2017, "Bad Genius", 1, "/videos/23.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9280), 200000 },
                    { 34, 7.2000000000000002, "Nine Naphat Siwopornthong, Pimchanok Luevisadpaipa", 6, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9310), "Hành trình tìm kiếm tình yêu của một chàng trai.", "Chayanop Boonprakob", "Friend Zone", 2, true, "/images/24.png", 1900, 2019, "Friend Zone", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9310), 220000 },
                    { 35, 8.0, "Benedict Cumberbatch, Keira Knightley", 7, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9330), "Câu chuyện về nhà toán học Alan Turing.", "Morten Tyldum", "The Imitation Game", 7, true, "/images/5.png", 1500, 2014, "The Imitation Game", 1, "/videos/41.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9330), 180000 },
                    { 36, 8.5, "Daniel Radcliffe, Rupert Grint, Emma Watson", 7, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9350), "Cuộc phiêu lưu đầu tiên của Harry Potter.", "Chris Columbus", "Harry Potter and the Philosopher's Stone", 6, true, "/images/6.png", 3800, 2001, "Harry Potter and the Sorcerer's Stone", 1, "/videos/42.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9350), 400000 },
                    { 37, 8.3000000000000007, "Audrey Tautou, Mathieu Kassovitz", 8, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9400), "Cuộc sống kỳ diệu của một cô gái ở Paris.", "Jean-Pierre Jeunet", "Le Fabuleux Destin d'Amélie Poulain", 2, true, "/images/7.png", 1700, 2001, "Amélie", 1, "/videos/43.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9400), 190000 },
                    { 38, 8.5999999999999996, "François Cluzet, Omar Sy", 8, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9430), "Câu chuyện tình bạn giữa một triệu phú và người chăm sóc anh.", "Olivier Nakache, Éric Toledano", "Intouchables", 2, true, "/images/8.png", 1900, 2011, "The Intouchables", 1, "/videos/44.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9430), 210000 },
                    { 39, 8.0999999999999996, "Brie Larson, Jacob Tremblay", 9, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9450), "Câu chuyện về một người mẹ và con trai thoát khỏi nơi giam cầm.", "Lenny Abrahamson", "Room", 7, true, "/images/4.png", 1400, 2015, "Room", 1, "/videos/45.mp4", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9450), 160000 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "Dislikes", "EpisodeId", "Likes", "MovieId", "ParentCommentId", "UserEmail", "UserName" },
                values: new object[] { 1, "Phim này thực sự làm mình khóc rất nhiều!", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7740), 2, null, 50, 1, null, "nguyenvana@example.com", "NguyenVanA" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "CreatedAt", "Description", "Duration", "EpisodeNumber", "MovieId", "ReleaseDate", "Title", "UpdatedAt", "VideoPath", "Views" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7630), "Yoon Se-ri vô tình hạ cánh xuống Bắc Triều Tiên sau một tai nạn.", 70, 1, 2, new DateTime(2019, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 1: Cuộc gặp gỡ định mệnh", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7640), "/videos/1.mp4", 10000 },
                    { 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7640), "Ri Jeong-hyeok giúp Se-ri tìm cách trở về Hàn Quốc.", 65, 2, 2, new DateTime(2019, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 2: Kế hoạch trốn thoát", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7640), "/videos/1.mp4", 9500 },
                    { 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7650), "Se-ri dần thích nghi với cuộc sống ở Bắc Triều Tiên.", 68, 3, 2, new DateTime(2019, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 3: Bí mật bị hé lộ", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7650), "/videos/1.mp4", 9000 },
                    { 4, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7660), "Tình cảm giữa Se-ri và Jeong-hyeok bắt đầu phát triển.", 72, 4, 2, new DateTime(2019, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 4: Tình cảm nảy nở", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7660), "/videos/1.mp4", 8800 },
                    { 5, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7660), "Se-ri đối mặt với những nguy hiểm mới ở Bắc Triều Tiên.", 70, 5, 2, new DateTime(2019, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 5: Thách thức mới", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7660), "/videos/1.mp4", 8500 },
                    { 6, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7670), "Jeong-hyeok lên kế hoạch bảo vệ Se-ri.", 67, 6, 2, new DateTime(2019, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 6: Hành trình nguy hiểm", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7670), "/videos/1.mp4", 8200 },
                    { 7, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8100), "Mô tả tập 1 của phim Đất Rừng Phương Nam", 60, 1, 4, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8050), "Tập 1: Đất Rừng Phương Nam", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8100), "/videos/4.mp4", 7320 },
                    { 8, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8130), "Mô tả tập 1 của phim Nhà Bà Nữ", 60, 1, 5, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8120), "Tập 1: Nhà Bà Nữ", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8130), "/videos/5.mp4", 7889 },
                    { 9, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8180), "Mô tả tập 1 của phim Lật Mặt 6: Tấm Vé Định Mệnh", 60, 1, 6, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8180), "Tập 1: Lật Mặt 6: Tấm Vé Định Mệnh", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8180), "/videos/6.mp4", 11808 },
                    { 10, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8210), "Mô tả tập 1 của phim Tiệc Trăng Máu", 60, 1, 7, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8200), "Tập 1: Tiệc Trăng Máu", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8210), "/videos/7.mp4", 5885 },
                    { 11, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8230), "Mô tả tập 1 của phim Em Chưa 18", 60, 1, 8, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8230), "Tập 1: Em Chưa 18", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8230), "/videos/3.mp4", 8605 },
                    { 12, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8260), "Mô tả tập 1 của phim Cô Dâu Băng Giá", 60, 1, 9, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8250), "Tập 1: Cô Dâu Băng Giá", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8260), "/videos/7.mp4", 14678 },
                    { 13, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8280), "Mô tả tập 1 của phim Chiếc Lá Cuốn Bay", 60, 1, 10, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8280), "Tập 1: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8280), "/videos/8.mp4", 11673 },
                    { 14, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8300), "Mô tả tập 2 của phim Chiếc Lá Cuốn Bay", 60, 2, 10, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8290), "Tập 2: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8300), "/videos/8.mp4", 9678 },
                    { 15, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8310), "Mô tả tập 3 của phim Chiếc Lá Cuốn Bay", 60, 3, 10, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8300), "Tập 3: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8310), "/videos/8.mp4", 12665 },
                    { 16, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8320), "Mô tả tập 4 của phim Chiếc Lá Cuốn Bay", 60, 4, 10, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8320), "Tập 4: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8330), "/videos/8.mp4", 11795 },
                    { 17, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8340), "Mô tả tập 5 của phim Chiếc Lá Cuốn Bay", 60, 5, 10, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8330), "Tập 5: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8340), "/videos/8.mp4", 8068 },
                    { 18, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8360), "Mô tả tập 1 của phim Goblin", 60, 1, 11, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8360), "Tập 1: Goblin", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8360), "/videos/1.mp4", 12950 },
                    { 19, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8380), "Mô tả tập 2 của phim Goblin", 60, 2, 11, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8370), "Tập 2: Goblin", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8380), "/videos/1.mp4", 14339 },
                    { 20, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8390), "Mô tả tập 3 của phim Goblin", 60, 3, 11, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8390), "Tập 3: Goblin", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8400), "/videos/1.mp4", 5942 },
                    { 21, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8410), "Mô tả tập 4 của phim Goblin", 60, 4, 11, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8400), "Tập 4: Goblin", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8410), "/videos/1.mp4", 10874 },
                    { 22, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8420), "Mô tả tập 5 của phim Goblin", 60, 5, 11, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8420), "Tập 5: Goblin", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8420), "/videos/1.mp4", 14510 },
                    { 23, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8450), "Mô tả tập 1 của phim Vinh Quang Trong Thù Hận", 60, 1, 12, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8440), "Tập 1: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8450), "/videos/3.mp4", 13702 },
                    { 24, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8460), "Mô tả tập 2 của phim Vinh Quang Trong Thù Hận", 60, 2, 12, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8460), "Tập 2: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8460), "/videos/3.mp4", 13481 },
                    { 25, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8480), "Mô tả tập 3 của phim Vinh Quang Trong Thù Hận", 60, 3, 12, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8470), "Tập 3: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8480), "/videos/3.mp4", 13090 },
                    { 26, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8490), "Mô tả tập 4 của phim Vinh Quang Trong Thù Hận", 60, 4, 12, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8490), "Tập 4: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8490), "/videos/3.mp4", 14303 },
                    { 27, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8500), "Mô tả tập 5 của phim Vinh Quang Trong Thù Hận", 60, 5, 12, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8500), "Tập 5: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8510), "/videos/3.mp4", 13578 },
                    { 28, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8530), "Mô tả tập 1 của phim Hospital Playlist", 60, 1, 13, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8520), "Tập 1: Hospital Playlist", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8530), "/videos/4.mp4", 6983 },
                    { 29, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8540), "Mô tả tập 2 của phim Hospital Playlist", 60, 2, 13, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8540), "Tập 2: Hospital Playlist", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8540), "/videos/4.mp4", 10028 },
                    { 30, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8560), "Mô tả tập 3 của phim Hospital Playlist", 60, 3, 13, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8550), "Tập 3: Hospital Playlist", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8560), "/videos/4.mp4", 8199 },
                    { 31, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8590), "Mô tả tập 4 của phim Hospital Playlist", 60, 4, 13, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8590), "Tập 4: Hospital Playlist", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8600), "/videos/4.mp4", 7972 },
                    { 32, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8610), "Mô tả tập 5 của phim Hospital Playlist", 60, 5, 13, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8600), "Tập 5: Hospital Playlist", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8610), "/videos/4.mp4", 5722 },
                    { 33, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8630), "Mô tả tập 1 của phim Reply 1988", 60, 1, 14, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8630), "Tập 1: Reply 1988", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8630), "/videos/5.mp4", 11703 },
                    { 34, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8650), "Mô tả tập 2 của phim Reply 1988", 60, 2, 14, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8640), "Tập 2: Reply 1988", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8650), "/videos/5.mp4", 8671 },
                    { 35, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8660), "Mô tả tập 3 của phim Reply 1988", 60, 3, 14, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8650), "Tập 3: Reply 1988", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8660), "/videos/5.mp4", 6898 },
                    { 36, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8670), "Mô tả tập 4 của phim Reply 1988", 60, 4, 14, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8670), "Tập 4: Reply 1988", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8670), "/videos/5.mp4", 12045 },
                    { 37, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8690), "Mô tả tập 5 của phim Reply 1988", 60, 5, 14, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8680), "Tập 5: Reply 1988", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8690), "/videos/5.mp4", 12181 },
                    { 38, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8710), "Mô tả tập 1 của phim The Wandering Earth", 60, 1, 15, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8710), "Tập 1: The Wandering Earth", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8710), "/videos/1.mp4", 12677 },
                    { 39, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8750), "Mô tả tập 1 của phim Ngoạ Hổ Tàng Long", 60, 1, 16, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8750), "Tập 1: Ngoạ Hổ Tàng Long", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8750), "/videos/11.mp4", 6501 },
                    { 40, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8780), "Mô tả tập 1 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 1, 17, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8770), "Tập 1: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8780), "/videos/2.mp4", 13690 },
                    { 41, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8800), "Mô tả tập 2 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 2, 17, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8780), "Tập 2: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8800), "/videos/2.mp4", 5718 },
                    { 42, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8810), "Mô tả tập 3 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 3, 17, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8800), "Tập 3: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8810), "/videos/2.mp4", 10474 },
                    { 43, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8820), "Mô tả tập 4 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 4, 17, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8820), "Tập 4: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8820), "/videos/2.mp4", 11022 },
                    { 44, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8840), "Mô tả tập 5 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 5, 17, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8830), "Tập 5: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8840), "/videos/2.mp4", 10756 },
                    { 45, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8860), "Mô tả tập 1 của phim Chiến Lang 2", 60, 1, 18, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8860), "Tập 1: Chiến Lang 2", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8860), "/videos/36.mp4", 8448 },
                    { 46, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8880), "Mô tả tập 1 của phim Your Name", 60, 1, 19, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8880), "Tập 1: Your Name", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8880), "/videos/3.mp4", 9875 },
                    { 47, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8910), "Mô tả tập 1 của phim Spirited Away", 60, 1, 20, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8900), "Tập 1: Spirited Away", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8910), "/videos/4.mp4", 10777 },
                    { 48, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8940), "Mô tả tập 1 của phim One Piece Film: Red", 60, 1, 21, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8930), "Tập 1: One Piece Film: Red", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8940), "/videos/15.mp4", 8536 },
                    { 49, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8960), "Mô tả tập 1 của phim Your Lie in April", 60, 1, 22, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8950), "Tập 1: Your Lie in April", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8960), "/videos/7.mp4", 6867 },
                    { 50, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8970), "Mô tả tập 2 của phim Your Lie in April", 60, 2, 22, new DateTime(2025, 5, 29, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8970), "Tập 2: Your Lie in April", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(8970), "/videos/7.mp4", 7684 },
                    { 51, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9010), "Mô tả tập 3 của phim Your Lie in April", 60, 3, 22, new DateTime(2025, 6, 5, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9010), "Tập 3: Your Lie in April", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9010), "/videos/7.mp4", 13749 },
                    { 52, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9030), "Mô tả tập 4 của phim Your Lie in April", 60, 4, 22, new DateTime(2025, 6, 12, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9020), "Tập 4: Your Lie in April", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9030), "/videos/7.mp4", 12046 },
                    { 53, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9040), "Mô tả tập 5 của phim Your Lie in April", 60, 5, 22, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9040), "Tập 5: Your Lie in April", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9040), "/videos/7.mp4", 7756 },
                    { 54, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9070), "Mô tả tập 1 của phim Avengers: Endgame", 60, 1, 23, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9060), "Tập 1: Avengers: Endgame", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9070), "/videos/6.mp4", 12974 },
                    { 55, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9090), "Mô tả tập 1 của phim Joker", 60, 1, 24, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9080), "Tập 1: Joker", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9090), "/videos/17.mp4", 13266 },
                    { 56, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9110), "Mô tả tập 1 của phim Fast & Furious 9", 60, 1, 25, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9110), "Tập 1: Fast & Furious 9", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9110), "/videos/18.mp4", 14428 },
                    { 57, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9140), "Mô tả tập 1 của phim Spider-Man: No Way Home", 60, 1, 26, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9130), "Tập 1: Spider-Man: No Way Home", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9140), "/videos/1.mp4", 11695 },
                    { 58, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9160), "Mô tả tập 1 của phim Dune", 60, 1, 27, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9150), "Tập 1: Dune", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9160), "/videos/2.mp4", 9442 },
                    { 59, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9180), "Mô tả tập 1 của phim Encanto", 60, 1, 28, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9170), "Tập 1: Encanto", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9180), "/videos/1.mp4", 12984 },
                    { 60, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9210), "Mô tả tập 1 của phim The Lion King", 60, 1, 29, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9200), "Tập 1: The Lion King", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9210), "/videos/4.mp4", 10444 },
                    { 61, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9230), "Mô tả tập 1 của phim The Dark Knight", 60, 1, 30, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9230), "Tập 1: The Dark Knight", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9230), "/videos/8.mp4", 12461 },
                    { 62, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9250), "Mô tả tập 1 của phim Inception", 60, 1, 31, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9250), "Tập 1: Inception", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9260), "/videos/3.mp4", 8919 },
                    { 63, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9280), "Mô tả tập 1 của phim Forrest Gump", 60, 1, 32, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9270), "Tập 1: Forrest Gump", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9280), "/videos/40.mp4", 7012 },
                    { 64, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9300), "Mô tả tập 1 của phim Bad Genius", 60, 1, 33, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9300), "Tập 1: Bad Genius", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9300), "/videos/3.mp4", 10404 },
                    { 65, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9320), "Mô tả tập 1 của phim Friend Zone", 60, 1, 34, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9320), "Tập 1: Friend Zone", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9320), "/videos/24.mp4", 7629 },
                    { 66, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9350), "Mô tả tập 1 của phim The Imitation Game", 60, 1, 35, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9340), "Tập 1: The Imitation Game", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9350), "/videos/5.mp4", 10444 },
                    { 67, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9370), "Mô tả tập 1 của phim Harry Potter and the Sorcerer's Stone", 60, 1, 36, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9370), "Tập 1: Harry Potter and the Sorcerer's Stone", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9370), "/videos/6.mp4", 9227 },
                    { 68, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9420), "Mô tả tập 1 của phim Amélie", 60, 1, 37, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9420), "Tập 1: Amélie", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9420), "/videos/7.mp4", 11993 },
                    { 69, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9440), "Mô tả tập 1 của phim The Intouchables", 60, 1, 38, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9440), "Tập 1: The Intouchables", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9450), "/videos/8.mp4", 6040 },
                    { 70, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9470), "Mô tả tập 1 của phim Room", 60, 1, 39, new DateTime(2025, 5, 22, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9460), "Tập 1: Room", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(9470), "/videos/4.mp4", 8437 }
                });

            migrationBuilder.InsertData(
                table: "MovieGenres",
                columns: new[] { "GenreId", "MovieId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 7, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 3 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 3, 8 },
                    { 8, 9 },
                    { 5, 11 },
                    { 9, 12 },
                    { 7, 13 },
                    { 7, 14 },
                    { 6, 15 },
                    { 6, 16 },
                    { 5, 17 },
                    { 6, 18 },
                    { 5, 19 },
                    { 6, 20 },
                    { 1, 21 },
                    { 6, 21 },
                    { 7, 22 },
                    { 5, 23 },
                    { 6, 23 },
                    { 9, 24 },
                    { 6, 25 },
                    { 5, 26 },
                    { 6, 26 },
                    { 6, 27 },
                    { 2, 28 },
                    { 6, 29 },
                    { 7, 30 },
                    { 9, 30 },
                    { 9, 31 },
                    { 7, 32 },
                    { 1, 33 },
                    { 3, 34 },
                    { 9, 35 },
                    { 5, 36 },
                    { 8, 36 },
                    { 3, 37 },
                    { 7, 37 },
                    { 7, 38 },
                    { 9, 39 }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "CreatedAt", "MovieId", "Review", "Score", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7700), 1, "Phim rất cảm động, diễn xuất của Trấn Thành tuyệt vời!", 8, "nguyenvana@example.com", "NguyenVanA" },
                    { 2, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7710), 1, "Cốt truyện gần gũi, phản ánh đúng cuộc sống.", 9, "tranthib@example.com", "TranThiB" },
                    { 3, new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7710), 3, "Một kiệt tác của điện ảnh Hàn Quốc!", 10, "levanc@example.com", "LeVanC" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "Dislikes", "EpisodeId", "Likes", "MovieId", "ParentCommentId", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { 2, "Cảm ơn Trấn Thành đã mang đến một bộ phim ý nghĩa!", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7740), 1, null, 30, 1, 1, "tranthib@example.com", "TranThiB" },
                    { 3, "Tập 1 rất hấp dẫn, chemistry giữa hai diễn viên chính đỉnh cao!", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7750), 0, 1, 40, 2, null, "levanc@example.com", "LeVanC" },
                    { 4, "Tập 2 càng cuốn, không thể rời mắt!", new DateTime(2025, 6, 19, 0, 33, 56, 452, DateTimeKind.Local).AddTicks(7750), 1, 2, 35, 2, null, "phamthid@example.com", "PhamThiD" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EpisodeId",
                table: "Comments",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MovieId",
                table: "Comments",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_MovieId_EpisodeNumber",
                table: "Episodes",
                columns: new[] { "MovieId", "EpisodeNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryId",
                table: "Movies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ReleaseYear",
                table: "Movies",
                column: "ReleaseYear");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
