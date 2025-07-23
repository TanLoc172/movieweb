using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsAndUserRegistrationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "193b4458-4ea7-4dc0-9363-ca51d60f3040");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34d1e12e-bab3-4bb8-a9c6-e697e454a3cf");

            migrationBuilder.CreateTable(
                name: "AnalyticsCounters",
                columns: table => new
                {
                    CounterName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CountValue = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsCounters", x => x.CounterName);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "246a67a3-dcc2-4401-9d0d-b2a3306114bc", null, "User", "USER" },
                    { "d120506c-27cf-45c2-9b1f-24402b17484e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8590), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8590) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8600), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8600) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8620), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8630) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8630), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8630) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8660), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8670), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8670) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8690), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8690) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8690), new DateTime(2025, 7, 23, 7, 27, 22, 146, DateTimeKind.Local).AddTicks(8690) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 4, 10, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480), new DateTime(2022, 4, 10, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 10, 27, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480), new DateTime(2022, 10, 27, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 2, 4, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480), new DateTime(2023, 2, 4, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 6, 14, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480), new DateTime(2021, 6, 14, 0, 27, 22, 146, DateTimeKind.Utc).AddTicks(8480) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsCounters");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "246a67a3-dcc2-4401-9d0d-b2a3306114bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d120506c-27cf-45c2-9b1f-24402b17484e");

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
        }
    }
}
