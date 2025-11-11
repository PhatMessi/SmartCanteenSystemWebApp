using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentAndStaffUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 9, 46, 34, 539, DateTimeKind.Utc).AddTicks(4823), "$2a$11$UaalioCFtH1SthNjyA3L4eCypj3ifCTWAYUaw6lF2k7mJk72m/pYO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 9, 46, 34, 683, DateTimeKind.Utc).AddTicks(5470), "$2a$11$WWDOdoK9AvbrQPk/lecgiOL16xSoYHL/6XO11xKAk85D88XD5Qa.q" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 9, 15, 9, 46, 34, 847, DateTimeKind.Utc).AddTicks(3029), "student@scms.com", "Nhat Dung", "$2a$11$YVaq9BU2qbK41ZI4M7OHR.rEYl7Ylx8zLpMImSglsRrxJUtQwCDD6", 1 },
                    { 7, new DateTime(2025, 9, 15, 9, 46, 35, 28, DateTimeKind.Utc).AddTicks(7656), "staff@scms.com", "Thien Truong", "$2a$11$Bz1Ud8kTak3v5t4Rk1WL2OeyY7jqCHka1QHDOvNoHCJyagGlOnpqK", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 4, 5, 48, 101, DateTimeKind.Utc).AddTicks(9791), "$2a$11$qELJdg.7P6essKYbo2eW4.KWnA0S.Bl9Yeom.9VLejyoL/0hA8gDW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 4, 5, 48, 333, DateTimeKind.Utc).AddTicks(4586), "$2a$11$AyLcVnQy1Y1jz1olmFvMuedruxeEWszmDmA5Kl.p7ANnWbP11ZLBm" });
        }
    }
}
