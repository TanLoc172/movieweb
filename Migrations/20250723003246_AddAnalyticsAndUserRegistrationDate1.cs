using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsAndUserRegistrationDate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "246a67a3-dcc2-4401-9d0d-b2a3306114bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d120506c-27cf-45c2-9b1f-24402b17484e");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "891788f9-876d-4e84-afb6-1cc2f011f674", null, "User", "USER" },
                    { "e1a2a227-d8d7-48d8-996f-dadcfa86b5ff", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8120), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8120) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8130), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8130) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8160), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8160) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8160), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8160) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8190), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8190) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8190), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8200) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8220), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8220) });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8220), new DateTime(2025, 7, 23, 7, 32, 45, 809, DateTimeKind.Local).AddTicks(8220) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 4, 10, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030), new DateTime(2022, 4, 10, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 10, 27, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030), new DateTime(2022, 10, 27, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 2, 4, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030), new DateTime(2023, 2, 4, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030) });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 6, 14, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030), new DateTime(2021, 6, 14, 0, 32, 45, 809, DateTimeKind.Utc).AddTicks(8030) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "891788f9-876d-4e84-afb6-1cc2f011f674");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1a2a227-d8d7-48d8-996f-dadcfa86b5ff");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "AspNetUsers");

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
    }
}
