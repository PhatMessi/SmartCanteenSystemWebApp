using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCanteenManagerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 3, 8, 22, 422, DateTimeKind.Utc).AddTicks(9683), "$2a$11$dnVIUupXx9uvATJHmbo/GuMz0rQNkd93W.ewsOT.kKKhWq1eY5PDq" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "RoleId" },
                values: new object[] { 2, new DateTime(2025, 9, 15, 3, 8, 22, 559, DateTimeKind.Utc).AddTicks(2060), "canteenmanager@scms.com", "Canteen Manager", "$2a$11$1VAhVAY86HX.8g0yWEfRIOGwJVo01ArXja9FYS74do669Uj7sg5BG", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 2, 56, 18, 131, DateTimeKind.Utc).AddTicks(6216), "$2a$11$ziHj7STjO6EU8CaEJUxzM.YTqwHikHEDqTyFXAmel052jdvDGcwSS" });
        }
    }
}
