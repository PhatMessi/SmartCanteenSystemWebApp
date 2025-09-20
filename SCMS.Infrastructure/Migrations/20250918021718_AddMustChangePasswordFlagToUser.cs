using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMustChangePasswordFlagToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MustChangePassword", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 516, DateTimeKind.Utc).AddTicks(1135), false, "$2a$11$ZX5973CxeTZzoOKqlIpRouGs0qvcqH1nwnj1mp.R6fFfHCXZ8b7p." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "MustChangePassword", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 654, DateTimeKind.Utc).AddTicks(6530), false, "$2a$11$L6p5Dt3wtaXm9WyQtgtR1OQEp0JLow40fbVt.kONJxpNntCN5yxwS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "MustChangePassword", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 807, DateTimeKind.Utc).AddTicks(959), false, "$2a$11$lZkZyyhUk9VL1YFee3s4mOvA6I2vbv3mEidXnqwJ7KIij7lJWA7mW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "MustChangePassword", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 966, DateTimeKind.Utc).AddTicks(6858), false, "$2a$11$htVnarQWU0rsYkX5L1AlMeyy3icYz.iKFh4bfYbrWjxmrZyQUn4D2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 242, DateTimeKind.Utc).AddTicks(8913), "$2a$11$lj4Pr6rMq4bXTcyvvpJDg.JGVhW5D3zbMuPyzQ1P6fm7AkyU92yP2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 379, DateTimeKind.Utc).AddTicks(8434), "$2a$11$gd2b7qwyuv0egoblD8.yauCMIcpJChQq2dtwSFXATbGAS.XX6fshy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 530, DateTimeKind.Utc).AddTicks(2360), "$2a$11$Wf8v6Zxf5AiofUktm2q1fusVIe1GYssP8yNxY4BHOWq5lgDy0R2aW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 5, 49, 679, DateTimeKind.Utc).AddTicks(291), "$2a$11$H/l4eBlk1DpitGnKuq/WEuhwyC5odphi8LKmDZf7MHjdNjw0.mHYK" });
        }
    }
}
