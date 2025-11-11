using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 242, DateTimeKind.Utc).AddTicks(8913), "$2a$11$lj4Pr6rMq4bXTcyvvpJDg.JGVhW5D3zbMuPyzQ1P6fm7AkyU92yP2", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 379, DateTimeKind.Utc).AddTicks(8434), "$2a$11$gd2b7qwyuv0egoblD8.yauCMIcpJChQq2dtwSFXATbGAS.XX6fshy", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 530, DateTimeKind.Utc).AddTicks(2360), "$2a$11$Wf8v6Zxf5AiofUktm2q1fusVIe1GYssP8yNxY4BHOWq5lgDy0R2aW", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 679, DateTimeKind.Utc).AddTicks(291), "$2a$11$H/l4eBlk1DpitGnKuq/WEuhwyC5odphi8LKmDZf7MHjdNjw0.mHYK", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Users");

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 9, 46, 34, 847, DateTimeKind.Utc).AddTicks(3029), "$2a$11$YVaq9BU2qbK41ZI4M7OHR.rEYl7Ylx8zLpMImSglsRrxJUtQwCDD6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 9, 46, 35, 28, DateTimeKind.Utc).AddTicks(7656), "$2a$11$Bz1Ud8kTak3v5t4Rk1WL2OeyY7jqCHka1QHDOvNoHCJyagGlOnpqK" });
        }
    }
}
