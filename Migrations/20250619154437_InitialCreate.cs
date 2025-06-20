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
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

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
                name: "WatchPartyRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    InviteCode = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    SelectedPosterUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    AutoStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    Private = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchPartyRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchPartyRooms_Movies_MovieId",
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
                    { 1, 8.5, "Trấn Thành, Tuấn Trần, Ngô Kiến Huy", 1, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9820), "Bộ phim kể về cuộc sống của một gia đình lao động nghèo ở Sài Gòn.", "Trấn Thành", "Old Father", 7, true, "/images/1.png", 1200, 2021, "Bố Già", 1, "/videos/1.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9820), 150000 },
                    { 2, 9.0, "Hyun Bin, Son Ye-jin", 2, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9830), "Câu chuyện tình yêu giữa một nữ thừa kế Hàn Quốc và một sĩ quan Bắc Triều Tiên.", "Lee Jeong-hyo", "Crash Landing on You", 2, true, "/images/2.png", 2000, 2019, "Hạ Cánh Nơi Anh", 16, "/videos/2.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9830), 250000 },
                    { 3, 9.1999999999999993, "Song Kang-ho, Lee Sun-kyun, Cho Yeo-jeong", 2, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9840), "Một gia đình nghèo tìm cách thâm nhập vào cuộc sống của một gia đình giàu có.", "Bong Joon-ho", "Parasite", 7, true, "/images/3.png", 2500, 2019, "Ký Sinh Trùng", 1, "/videos/3.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9840), 300000 },
                    { 4, 7.7999999999999998, "Hạo Khang, Trấn Thành, Mai Cát Vi", 1, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(220), "Câu chuyện về hành trình trưởng thành của bé An tại miền Nam Việt Nam.", "Võ Thanh Hòa", "Southern Forest Land", 6, true, "/images/4.png", 800, 2023, "Đất Rừng Phương Nam", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(220), 120000 },
                    { 5, 7.5, "Trấn Thành, Uyển Ân, Ngọc Châu", 1, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(360), "Câu chuyện về mâu thuẫn gia đình và tình yêu.", "Trấn Thành", "The House of No Man", 2, true, "/images/5.png", 1500, 2023, "Nhà Bà Nữ", 1, "/videos/5.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(360), 180000 },
                    { 6, 7.2000000000000002, "Thái Hòa, Thanh Thức, Huỳnh Đông", 1, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(390), "Cuộc phiêu lưu để giành tấm vé số định mệnh.", "Lý Hải", "Face Off 6: The Destiny Ticket", 3, true, "/images/6.png", 1300, 2023, "Lật Mặt 6: Tấm Vé Định Mệnh", 1, "/videos/6.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(390), 150000 },
                    { 7, 7.0, "Thái Hòa, Kiều Minh Tuấn, Đức Thịnh", 1, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(420), "Những bí mật của một nhóm bạn được phơi bày qua một trò chơi.", "Nguyễn Quang Dũng", "Full House", 3, true, "/images/7.png", 1200, 2020, "Tiệc Trăng Máu", 1, "/videos/8.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(420), 130000 },
                    { 8, 6.7999999999999998, "Kaity Nguyễn, Will", 1, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(440), "Câu chuyện tình yêu tuổi teen với nhiều tình huống dở khóc dở cười.", "Lê Thanh Sơn", "Sweet 18", 2, true, "/images/3.png", 1000, 2017, "Em Chưa 18", 1, "/videos/32.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(440), 110000 },
                    { 9, 8.6999999999999993, "Suzu Hirose, Kôki Mitsushima", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(470), "Một câu chuyện cổ tích về tình yêu và băng giá.", "Hayao Miyazaki", "Frozen Bride", 2, true, "/images/7.png", 1600, 2020, "Cô Dâu Băng Giá", 1, "/videos/7.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(470), 190000 },
                    { 10, 8.5, "Baifern Pimchanok, Push Puttichai", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(490), "Cuộc đời đầy biến cố của một cô gái chuyển giới.", "Sopon Sukapisoot", "Bai Mai Tee Plid Plew", 7, true, "/images/8.png", 3500, 2019, "Chiếc Lá Cuốn Bay", 20, "/videos/8.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(490), 400000 },
                    { 11, 9.0, "Gong Yoo, Kim Go-eun, Lee Dong-wook", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(570), "Câu chuyện tình yêu giữa một goblin và một cô dâu loài người.", "Kim Eun-sook", "Guardian: The Lonely and Great God", 2, true, "/images/1.png", 2500, 2016, "Goblin", 16, "/videos/9.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(570), 300000 },
                    { 12, 9.1999999999999993, "Song Hye-kyo, Lee Do-hyun", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(670), "Một cô gái tìm cách trả thù những kẻ đã bắt nạt mình.", "Ahn Gil-ho", "The Glory", 7, true, "/images/3.png", 3000, 2022, "Vinh Quang Trong Thù Hận", 16, "/videos/33.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(670), 350000 },
                    { 13, 9.3000000000000007, "Jo Jung-suk, Yoo Yeon-seok, Jung Kyung-ho", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(750), "Cuộc sống và tình bạn của 5 bác sĩ.", "Shin Won-ho", "Hospital Playlist", 2, true, "/images/4.png", 2300, 2020, "Hospital Playlist", 24, "/videos/34.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(750), 280000 },
                    { 14, 9.4000000000000004, "Lee Hye-ri, Ryu Jun-yeol, Go Kyung-pyo", 2, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(820), "Câu chuyện về tình bạn và gia đình tại Seoul năm 1988.", "Shin Won-ho", "Reply 1988", 2, true, "/images/5.png", 2400, 2015, "Reply 1988", 20, "/videos/35.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(820), 290000 },
                    { 15, 7.0, "Wu Jing, Qing Xu", 3, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(900), "Nhân loại di chuyển Trái Đất ra khỏi hệ Mặt Trời.", "Frant Gwo", "The Wandering Earth", 5, true, "/images/1.png", 2800, 2019, "The Wandering Earth", 1, "/videos/10.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(910), 320000 },
                    { 16, 8.1999999999999993, "Châu Nhuận Phát, Dương Tử Quỳnh, Chương Tử Di", 3, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(930), "Kiếm hiệp tình duyên với những màn võ thuật đẹp mắt.", "Lý An", "Crouching Tiger, Hidden Dragon", 1, true, "/images/11.png", 2000, 2000, "Ngoạ Hổ Tàng Long", 1, "/videos/11.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(930), 250000 },
                    { 17, 8.8000000000000007, "Dương Mịch, Triệu Hựu Đình", 3, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(950), "Chuyện tình xuyên không gian và thời gian.", "Lâm Ngọc Phượng", "Eternal Love", 2, true, "/images/2.png", 4000, 2017, "Tam Sinh Tam Thế Thập Lý Đào Hoa", 58, "/videos/12.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(950), 450000 },
                    { 18, 7.5, "Ngô Kinh, Frank Grillo", 3, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1050), "Người lính giải cứu công dân Trung Quốc ở châu Phi.", "Ngô Kinh", "Wolf Warrior 2", 1, true, "/images/36.png", 3200, 2017, "Chiến Lang 2", 1, "/videos/6.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1050), 380000 },
                    { 19, 9.3000000000000007, "Ryunosuke Kamiki, Mone Kamishiraishi", 4, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1080), "Câu chuyện tình yêu kỳ diệu giữa hai người xa lạ đổi linh hồn.", "Makoto Shinkai", "Kimi no Na wa.", 2, true, "/images/3.png", 3800, 2016, "Your Name", 1, "/videos/13.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1080), 420000 },
                    { 20, 9.1999999999999993, "Rumi Hiiragi, Miyu Irino", 4, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1100), "Hành trình phiêu lưu của cô bé Chihiro tại thế giới linh hồn.", "Hayao Miyazaki", "Sen to Chihiro no Kamikakushi", 8, true, "/images/4.png", 3600, 2001, "Spirited Away", 1, "/videos/14.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1100), 380000 },
                    { 21, 8.0, "Mayumi Tanaka, Kazuya Nakai", 4, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1120), "Chuyến phiêu lưu âm nhạc của nhóm Mũ Rơm.", "Goro Taniguchi", "One Piece Film: Red", 8, true, "/images/15.png", 2500, 2022, "One Piece Film: Red", 1, "/videos/5.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1120), 300000 },
                    { 22, 8.5, "Haruka Misaki, Kousei Arima", 4, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1140), "Câu chuyện tình cảm học đường về âm nhạc và tình yêu.", "Hiroshi Nishikori", "Shigatsu wa Kimi no Uso", 2, true, "/images/7.png", 1900, 2016, "Your Lie in April", 22, "/videos/37.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1150), 220000 },
                    { 23, 9.5, "Robert Downey Jr., Chris Evans, Mark Ruffalo", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1230), "Cuộc chiến cuối cùng của các siêu anh hùng chống lại Thanos.", "Anthony Russo, Joe Russo", "Avengers: Endgame", 1, true, "/images/6.png", 5000, 2019, "Avengers: Endgame", 1, "/videos/16.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1230), 500000 },
                    { 24, 9.0, "Joaquin Phoenix, Robert De Niro", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1250), "Nguồn gốc của gã hề phản diện Joker.", "Todd Phillips", "Joker", 7, true, "/images/17.png", 4000, 2019, "Joker", 1, "/videos/7.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1250), 450000 },
                    { 25, 6.5, "Vin Diesel, Michelle Rodriguez", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1270), "Dom và gia đình đối mặt với kẻ thù mới.", "Justin Lin", "F9", 1, true, "/images/18.png", 2500, 2021, "Fast & Furious 9", 1, "/videos/8.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1270), 300000 },
                    { 26, 9.0, "Tom Holland, Zendaya, Benedict Cumberbatch", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1290), "Spider-Man đối mặt với đa vũ trụ.", "Jon Watts", "Spider-Man: No Way Home", 1, true, "/images/1.png", 5200, 2021, "Spider-Man: No Way Home", 1, "/videos/19.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1300), 550000 },
                    { 27, 8.0, "Timothée Chalamet, Rebecca Ferguson", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1320), "Hành trình của Paul Atreides tại hành tinh Arrakis.", "Denis Villeneuve", "Dune", 5, true, "/images/2.png", 3000, 2021, "Dune", 1, "/videos/2.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1320), 350000 },
                    { 28, 8.0, "Stephanie Beatriz, María Cecilia Botero", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1340), "Gia đình Madrigal với phép thuật.", "Jared Bush, Byron Howard", "Encanto", 8, true, "/images/1.png", 2900, 2021, "Encanto", 1, "/videos/2.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1340), 330000 },
                    { 29, 7.0, "Donald Glover, Beyoncé, Seth Rogen", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1360), "Câu chuyện về chú sư tử Simba.", "Jon Favreau", "The Lion King", 8, true, "/images/4.png", 3100, 2019, "The Lion King", 1, "/videos/22.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1370), 360000 },
                    { 30, 9.5999999999999996, "Christian Bale, Heath Ledger, Aaron Eckhart", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1390), "Batman đối đầu với Joker.", "Christopher Nolan", "The Dark Knight", 1, true, "/images/8.png", 4500, 2008, "The Dark Knight", 1, "/videos/38.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1390), 480000 },
                    { 31, 9.3000000000000007, "Leonardo DiCaprio, Joseph Gordon-Levitt", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1410), "Cuộc hành trình vào thế giới giấc mơ.", "Christopher Nolan", "Inception", 5, true, "/images/3.png", 4200, 2010, "Inception", 1, "/videos/39.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1410), 460000 },
                    { 32, 9.0999999999999996, "Tom Hanks, Robin Wright", 5, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1440), "Cuộc đời của một người đàn ông đặc biệt.", "Robert Zemeckis", "Forrest Gump", 2, true, "/images/40.png", 3800, 1994, "Forrest Gump", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1450), 400000 },
                    { 33, 7.7999999999999998, "Chutimon Chuengcharoensukying, Non Chanon Santinatornkul", 6, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1470), "Câu chuyện về những học sinh thiên tài gian lận thi cử.", "Nattawut Poonpiriya", "Chalard Games Goeng", 7, true, "/images/3.png", 1800, 2017, "Bad Genius", 1, "/videos/23.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1470), 200000 },
                    { 34, 7.2000000000000002, "Nine Naphat Siwopornthong, Pimchanok Luevisadpaipa", 6, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1490), "Hành trình tìm kiếm tình yêu của một chàng trai.", "Chayanop Boonprakob", "Friend Zone", 2, true, "/images/24.png", 1900, 2019, "Friend Zone", 1, "/videos/4.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1490), 220000 },
                    { 35, 8.0, "Benedict Cumberbatch, Keira Knightley", 7, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1510), "Câu chuyện về nhà toán học Alan Turing.", "Morten Tyldum", "The Imitation Game", 7, true, "/images/5.png", 1500, 2014, "The Imitation Game", 1, "/videos/41.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1510), 180000 },
                    { 36, 8.5, "Daniel Radcliffe, Rupert Grint, Emma Watson", 7, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1530), "Cuộc phiêu lưu đầu tiên của Harry Potter.", "Chris Columbus", "Harry Potter and the Philosopher's Stone", 6, true, "/images/6.png", 3800, 2001, "Harry Potter and the Sorcerer's Stone", 1, "/videos/42.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1540), 400000 },
                    { 37, 8.3000000000000007, "Audrey Tautou, Mathieu Kassovitz", 8, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1560), "Cuộc sống kỳ diệu của một cô gái ở Paris.", "Jean-Pierre Jeunet", "Le Fabuleux Destin d'Amélie Poulain", 2, true, "/images/7.png", 1700, 2001, "Amélie", 1, "/videos/43.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1560), 190000 },
                    { 38, 8.5999999999999996, "François Cluzet, Omar Sy", 8, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1580), "Câu chuyện tình bạn giữa một triệu phú và người chăm sóc anh.", "Olivier Nakache, Éric Toledano", "Intouchables", 2, true, "/images/8.png", 1900, 2011, "The Intouchables", 1, "/videos/44.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1580), 210000 },
                    { 39, 8.0999999999999996, "Brie Larson, Jacob Tremblay", 9, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1630), "Câu chuyện về một người mẹ và con trai thoát khỏi nơi giam cầm.", "Lenny Abrahamson", "Room", 7, true, "/images/4.png", 1400, 2015, "Room", 1, "/videos/45.mp4", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1630), 160000 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "Dislikes", "EpisodeId", "Likes", "MovieId", "ParentCommentId", "UserEmail", "UserName" },
                values: new object[] { 1, "Phim này thực sự làm mình khóc rất nhiều!", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(10), 2, null, 50, 1, null, "nguyenvana@example.com", "NguyenVanA" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "CreatedAt", "Description", "Duration", "EpisodeNumber", "MovieId", "ReleaseDate", "Title", "UpdatedAt", "VideoPath", "Views" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9900), "Yoon Se-ri vô tình hạ cánh xuống Bắc Triều Tiên sau một tai nạn.", 70, 1, 2, new DateTime(2019, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 1: Cuộc gặp gỡ định mệnh", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9910), "/videos/1.mp4", 10000 },
                    { 2, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9910), "Ri Jeong-hyeok giúp Se-ri tìm cách trở về Hàn Quốc.", 65, 2, 2, new DateTime(2019, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 2: Kế hoạch trốn thoát", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9910), "/videos/1.mp4", 9500 },
                    { 3, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9920), "Se-ri dần thích nghi với cuộc sống ở Bắc Triều Tiên.", 68, 3, 2, new DateTime(2019, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 3: Bí mật bị hé lộ", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9920), "/videos/1.mp4", 9000 },
                    { 4, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9920), "Tình cảm giữa Se-ri và Jeong-hyeok bắt đầu phát triển.", 72, 4, 2, new DateTime(2019, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 4: Tình cảm nảy nở", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9930), "/videos/1.mp4", 8800 },
                    { 5, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9930), "Se-ri đối mặt với những nguy hiểm mới ở Bắc Triều Tiên.", 70, 5, 2, new DateTime(2019, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 5: Thách thức mới", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9930), "/videos/1.mp4", 8500 },
                    { 6, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9940), "Jeong-hyeok lên kế hoạch bảo vệ Se-ri.", 67, 6, 2, new DateTime(2019, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập 6: Hành trình nguy hiểm", new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9940), "/videos/1.mp4", 8200 },
                    { 7, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(350), "Mô tả tập 1 của phim Đất Rừng Phương Nam", 60, 1, 4, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(290), "Tập 1: Đất Rừng Phương Nam", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(350), "/videos/4.mp4", 14276 },
                    { 8, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(380), "Mô tả tập 1 của phim Nhà Bà Nữ", 60, 1, 5, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(380), "Tập 1: Nhà Bà Nữ", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(390), "/videos/5.mp4", 9942 },
                    { 9, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(410), "Mô tả tập 1 của phim Lật Mặt 6: Tấm Vé Định Mệnh", 60, 1, 6, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(400), "Tập 1: Lật Mặt 6: Tấm Vé Định Mệnh", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(410), "/videos/6.mp4", 12817 },
                    { 10, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(430), "Mô tả tập 1 của phim Tiệc Trăng Máu", 60, 1, 7, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(430), "Tập 1: Tiệc Trăng Máu", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(430), "/videos/7.mp4", 9339 },
                    { 11, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(460), "Mô tả tập 1 của phim Em Chưa 18", 60, 1, 8, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(460), "Tập 1: Em Chưa 18", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(460), "/videos/3.mp4", 10316 },
                    { 12, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(490), "Mô tả tập 1 của phim Cô Dâu Băng Giá", 60, 1, 9, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(480), "Tập 1: Cô Dâu Băng Giá", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(490), "/videos/7.mp4", 12510 },
                    { 13, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(510), "Mô tả tập 1 của phim Chiếc Lá Cuốn Bay", 60, 1, 10, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(500), "Tập 1: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(510), "/videos/8.mp4", 10308 },
                    { 14, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(520), "Mô tả tập 2 của phim Chiếc Lá Cuốn Bay", 60, 2, 10, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(520), "Tập 2: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(520), "/videos/8.mp4", 8605 },
                    { 15, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(530), "Mô tả tập 3 của phim Chiếc Lá Cuốn Bay", 60, 3, 10, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(530), "Tập 3: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(540), "/videos/8.mp4", 12862 },
                    { 16, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(550), "Mô tả tập 4 của phim Chiếc Lá Cuốn Bay", 60, 4, 10, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(540), "Tập 4: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(550), "/videos/8.mp4", 5888 },
                    { 17, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(560), "Mô tả tập 5 của phim Chiếc Lá Cuốn Bay", 60, 5, 10, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(560), "Tập 5: Chiếc Lá Cuốn Bay", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(560), "/videos/8.mp4", 5162 },
                    { 18, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(590), "Mô tả tập 1 của phim Goblin", 60, 1, 11, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(580), "Tập 1: Goblin", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(590), "/videos/1.mp4", 13890 },
                    { 19, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(600), "Mô tả tập 2 của phim Goblin", 60, 2, 11, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(590), "Tập 2: Goblin", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(600), "/videos/1.mp4", 8230 },
                    { 20, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(610), "Mô tả tập 3 của phim Goblin", 60, 3, 11, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(610), "Tập 3: Goblin", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(610), "/videos/1.mp4", 13619 },
                    { 21, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(630), "Mô tả tập 4 của phim Goblin", 60, 4, 11, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(620), "Tập 4: Goblin", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(630), "/videos/1.mp4", 6923 },
                    { 22, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(640), "Mô tả tập 5 của phim Goblin", 60, 5, 11, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(630), "Tập 5: Goblin", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(640), "/videos/1.mp4", 5023 },
                    { 23, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(680), "Mô tả tập 1 của phim Vinh Quang Trong Thù Hận", 60, 1, 12, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(680), "Tập 1: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(680), "/videos/3.mp4", 13565 },
                    { 24, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(700), "Mô tả tập 2 của phim Vinh Quang Trong Thù Hận", 60, 2, 12, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(690), "Tập 2: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(700), "/videos/3.mp4", 5181 },
                    { 25, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(710), "Mô tả tập 3 của phim Vinh Quang Trong Thù Hận", 60, 3, 12, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(710), "Tập 3: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(710), "/videos/3.mp4", 8581 },
                    { 26, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(720), "Mô tả tập 4 của phim Vinh Quang Trong Thù Hận", 60, 4, 12, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(720), "Tập 4: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(730), "/videos/3.mp4", 9663 },
                    { 27, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(740), "Mô tả tập 5 của phim Vinh Quang Trong Thù Hận", 60, 5, 12, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(730), "Tập 5: Vinh Quang Trong Thù Hận", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(740), "/videos/3.mp4", 14658 },
                    { 28, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(760), "Mô tả tập 1 của phim Hospital Playlist", 60, 1, 13, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(760), "Tập 1: Hospital Playlist", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(760), "/videos/4.mp4", 12450 },
                    { 29, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(780), "Mô tả tập 2 của phim Hospital Playlist", 60, 2, 13, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(770), "Tập 2: Hospital Playlist", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(780), "/videos/4.mp4", 9185 },
                    { 30, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(790), "Mô tả tập 3 của phim Hospital Playlist", 60, 3, 13, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(780), "Tập 3: Hospital Playlist", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(790), "/videos/4.mp4", 6086 },
                    { 31, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(800), "Mô tả tập 4 của phim Hospital Playlist", 60, 4, 13, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(800), "Tập 4: Hospital Playlist", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(800), "/videos/4.mp4", 11760 },
                    { 32, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(820), "Mô tả tập 5 của phim Hospital Playlist", 60, 5, 13, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(810), "Tập 5: Hospital Playlist", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(820), "/videos/4.mp4", 10596 },
                    { 33, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(840), "Mô tả tập 1 của phim Reply 1988", 60, 1, 14, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(830), "Tập 1: Reply 1988", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(840), "/videos/5.mp4", 9241 },
                    { 34, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(860), "Mô tả tập 2 của phim Reply 1988", 60, 2, 14, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(850), "Tập 2: Reply 1988", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(860), "/videos/5.mp4", 12152 },
                    { 35, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(870), "Mô tả tập 3 của phim Reply 1988", 60, 3, 14, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(870), "Tập 3: Reply 1988", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(870), "/videos/5.mp4", 10888 },
                    { 36, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(880), "Mô tả tập 4 của phim Reply 1988", 60, 4, 14, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(880), "Tập 4: Reply 1988", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(880), "/videos/5.mp4", 11863 },
                    { 37, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(900), "Mô tả tập 5 của phim Reply 1988", 60, 5, 14, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(890), "Tập 5: Reply 1988", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(900), "/videos/5.mp4", 7505 },
                    { 38, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(920), "Mô tả tập 1 của phim The Wandering Earth", 60, 1, 15, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(910), "Tập 1: The Wandering Earth", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(920), "/videos/1.mp4", 14944 },
                    { 39, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(940), "Mô tả tập 1 của phim Ngoạ Hổ Tàng Long", 60, 1, 16, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(940), "Tập 1: Ngoạ Hổ Tàng Long", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(940), "/videos/11.mp4", 10968 },
                    { 40, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(970), "Mô tả tập 1 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 1, 17, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(960), "Tập 1: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(970), "/videos/2.mp4", 11051 },
                    { 41, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(980), "Mô tả tập 2 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 2, 17, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(980), "Tập 2: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(980), "/videos/2.mp4", 13362 },
                    { 42, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1000), "Mô tả tập 3 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 3, 17, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(990), "Tập 3: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1000), "/videos/2.mp4", 6508 },
                    { 43, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1010), "Mô tả tập 4 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 4, 17, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1000), "Tập 4: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1010), "/videos/2.mp4", 7147 },
                    { 44, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1050), "Mô tả tập 5 của phim Tam Sinh Tam Thế Thập Lý Đào Hoa", 60, 5, 17, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1040), "Tập 5: Tam Sinh Tam Thế Thập Lý Đào Hoa", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1050), "/videos/2.mp4", 5382 },
                    { 45, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1070), "Mô tả tập 1 của phim Chiến Lang 2", 60, 1, 18, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1060), "Tập 1: Chiến Lang 2", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1070), "/videos/36.mp4", 7379 },
                    { 46, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1090), "Mô tả tập 1 của phim Your Name", 60, 1, 19, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1090), "Tập 1: Your Name", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1090), "/videos/3.mp4", 10165 },
                    { 47, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1110), "Mô tả tập 1 của phim Spirited Away", 60, 1, 20, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1110), "Tập 1: Spirited Away", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1110), "/videos/4.mp4", 9305 },
                    { 48, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1140), "Mô tả tập 1 của phim One Piece Film: Red", 60, 1, 21, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1130), "Tập 1: One Piece Film: Red", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1140), "/videos/15.mp4", 7384 },
                    { 49, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1160), "Mô tả tập 1 của phim Your Lie in April", 60, 1, 22, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1150), "Tập 1: Your Lie in April", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1160), "/videos/7.mp4", 14941 },
                    { 50, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1170), "Mô tả tập 2 của phim Your Lie in April", 60, 2, 22, new DateTime(2025, 5, 29, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1170), "Tập 2: Your Lie in April", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1170), "/videos/7.mp4", 9526 },
                    { 51, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1190), "Mô tả tập 3 của phim Your Lie in April", 60, 3, 22, new DateTime(2025, 6, 5, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1180), "Tập 3: Your Lie in April", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1190), "/videos/7.mp4", 7174 },
                    { 52, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1200), "Mô tả tập 4 của phim Your Lie in April", 60, 4, 22, new DateTime(2025, 6, 12, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1200), "Tập 4: Your Lie in April", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1200), "/videos/7.mp4", 14835 },
                    { 53, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1210), "Mô tả tập 5 của phim Your Lie in April", 60, 5, 22, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1210), "Tập 5: Your Lie in April", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1220), "/videos/7.mp4", 13063 },
                    { 54, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1240), "Mô tả tập 1 của phim Avengers: Endgame", 60, 1, 23, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1240), "Tập 1: Avengers: Endgame", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1240), "/videos/6.mp4", 13466 },
                    { 55, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1260), "Mô tả tập 1 của phim Joker", 60, 1, 24, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1260), "Tập 1: Joker", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1270), "/videos/17.mp4", 8427 },
                    { 56, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1290), "Mô tả tập 1 của phim Fast & Furious 9", 60, 1, 25, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1280), "Tập 1: Fast & Furious 9", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1290), "/videos/18.mp4", 10527 },
                    { 57, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1310), "Mô tả tập 1 của phim Spider-Man: No Way Home", 60, 1, 26, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1310), "Tập 1: Spider-Man: No Way Home", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1310), "/videos/1.mp4", 8049 },
                    { 58, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1340), "Mô tả tập 1 của phim Dune", 60, 1, 27, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1330), "Tập 1: Dune", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1340), "/videos/2.mp4", 10038 },
                    { 59, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1360), "Mô tả tập 1 của phim Encanto", 60, 1, 28, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1350), "Tập 1: Encanto", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1360), "/videos/1.mp4", 10289 },
                    { 60, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1380), "Mô tả tập 1 của phim The Lion King", 60, 1, 29, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1380), "Tập 1: The Lion King", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1380), "/videos/4.mp4", 7918 },
                    { 61, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1410), "Mô tả tập 1 của phim The Dark Knight", 60, 1, 30, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1400), "Tập 1: The Dark Knight", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1410), "/videos/8.mp4", 6036 },
                    { 62, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1440), "Mô tả tập 1 của phim Inception", 60, 1, 31, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1430), "Tập 1: Inception", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1440), "/videos/3.mp4", 13883 },
                    { 63, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1460), "Mô tả tập 1 của phim Forrest Gump", 60, 1, 32, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1460), "Tập 1: Forrest Gump", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1460), "/videos/40.mp4", 7673 },
                    { 64, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1480), "Mô tả tập 1 của phim Bad Genius", 60, 1, 33, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1480), "Tập 1: Bad Genius", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1490), "/videos/3.mp4", 12132 },
                    { 65, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1510), "Mô tả tập 1 của phim Friend Zone", 60, 1, 34, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1500), "Tập 1: Friend Zone", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1510), "/videos/24.mp4", 13497 },
                    { 66, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1530), "Mô tả tập 1 của phim The Imitation Game", 60, 1, 35, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1520), "Tập 1: The Imitation Game", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1530), "/videos/5.mp4", 9578 },
                    { 67, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1550), "Mô tả tập 1 của phim Harry Potter and the Sorcerer's Stone", 60, 1, 36, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1550), "Tập 1: Harry Potter and the Sorcerer's Stone", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1550), "/videos/6.mp4", 6473 },
                    { 68, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1580), "Mô tả tập 1 của phim Amélie", 60, 1, 37, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1570), "Tập 1: Amélie", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1580), "/videos/7.mp4", 8317 },
                    { 69, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1600), "Mô tả tập 1 của phim The Intouchables", 60, 1, 38, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1590), "Tập 1: The Intouchables", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1600), "/videos/8.mp4", 9724 },
                    { 70, new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1650), "Mô tả tập 1 của phim Room", 60, 1, 39, new DateTime(2025, 5, 22, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1640), "Tập 1: Room", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(1650), "/videos/4.mp4", 7517 }
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
                    { 1, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9970), 1, "Phim rất cảm động, diễn xuất của Trấn Thành tuyệt vời!", 8, "nguyenvana@example.com", "NguyenVanA" },
                    { 2, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9970), 1, "Cốt truyện gần gũi, phản ánh đúng cuộc sống.", 9, "tranthib@example.com", "TranThiB" },
                    { 3, new DateTime(2025, 6, 19, 22, 44, 37, 361, DateTimeKind.Local).AddTicks(9980), 3, "Một kiệt tác của điện ảnh Hàn Quốc!", 10, "levanc@example.com", "LeVanC" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "Dislikes", "EpisodeId", "Likes", "MovieId", "ParentCommentId", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { 2, "Cảm ơn Trấn Thành đã mang đến một bộ phim ý nghĩa!", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(10), 1, null, 30, 1, 1, "tranthib@example.com", "TranThiB" },
                    { 3, "Tập 1 rất hấp dẫn, chemistry giữa hai diễn viên chính đỉnh cao!", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(20), 0, 1, 40, 2, null, "levanc@example.com", "LeVanC" },
                    { 4, "Tập 2 càng cuốn, không thể rời mắt!", new DateTime(2025, 6, 19, 22, 44, 37, 362, DateTimeKind.Local).AddTicks(20), 1, 2, 35, 2, null, "phamthid@example.com", "PhamThiD" }
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

            migrationBuilder.CreateIndex(
                name: "IX_WatchPartyRooms_MovieId",
                table: "WatchPartyRooms",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "WatchPartyRooms");

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
