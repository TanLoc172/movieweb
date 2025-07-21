using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddReelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5592f94-cbee-4ef4-87aa-e2b9faabb9d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9b7cef5-d120-4efd-8ce0-b82f1ed32dbc");

            migrationBuilder.CreateTable(
                name: "Reels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    VideoPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "TEXT", nullable: true),
                    UploaderId = table.Column<string>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    Likes = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reels_AspNetUsers_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "193b4458-4ea7-4dc0-9363-ca51d60f3040", null, "User", "USER" },
                    { "34d1e12e-bab3-4bb8-a9c6-e697e454a3cf", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1650), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1650) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1660), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1680), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1680) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1690), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1690) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1710), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1710) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1720), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1720) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1760), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1760) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1770), new DateTime(2025, 7, 20, 22, 58, 31, 848, DateTimeKind.Local).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 4, 7, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540), new DateTime(2022, 4, 7, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 10, 24, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540), new DateTime(2022, 10, 24, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 2, 1, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540), new DateTime(2023, 2, 1, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 6, 11, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540), new DateTime(2021, 6, 11, 15, 58, 31, 848, DateTimeKind.Utc).AddTicks(1540) });

            migrationBuilder.CreateIndex(
                name: "IX_Reels_UploaderId",
                table: "Reels",
                column: "UploaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reels");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "193b4458-4ea7-4dc0-9363-ca51d60f3040");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34d1e12e-bab3-4bb8-a9c6-e697e454a3cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e5592f94-cbee-4ef4-87aa-e2b9faabb9d4", null, "User", "USER" },
                    { "f9b7cef5-d120-4efd-8ce0-b82f1ed32dbc", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8930), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8930) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8950), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8950) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8980), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8980) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8990), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(8990) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9020), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9020) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9090), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9090) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9120), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9120) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9130), new DateTime(2025, 7, 15, 21, 26, 14, 338, DateTimeKind.Local).AddTicks(9130) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 4, 2, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740), new DateTime(2022, 4, 2, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 10, 19, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740), new DateTime(2022, 10, 19, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 1, 27, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740), new DateTime(2023, 1, 27, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 6, 6, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740), new DateTime(2021, 6, 6, 14, 26, 14, 338, DateTimeKind.Utc).AddTicks(8740) });
        }
    }
}
