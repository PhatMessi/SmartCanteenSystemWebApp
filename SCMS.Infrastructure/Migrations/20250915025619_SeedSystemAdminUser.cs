using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSystemAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "RoleId" },
                values: new object[] { 1, new DateTime(2025, 9, 15, 2, 56, 18, 131, DateTimeKind.Utc).AddTicks(6216), "admin@scms.com", "System Administrator", "$2a$11$ziHj7STjO6EU8CaEJUxzM.YTqwHikHEDqTyFXAmel052jdvDGcwSS", 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
