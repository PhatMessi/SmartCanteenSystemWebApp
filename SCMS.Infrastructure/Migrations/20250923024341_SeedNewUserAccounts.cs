using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewUserAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 40, 535, DateTimeKind.Utc).AddTicks(6485), "$2a$11$LGZAvXkGcsB3OIQ9CdI0s.s266QOwV2J2VGCQGUhL1IMb6UwtfJP." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 40, 678, DateTimeKind.Utc).AddTicks(9964), "$2a$11$RO6BmxvWG9UAmk0BtGwEVOH35c6bgcOm56UPJpWpYdSM3MgRsfsh2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 40, 824, DateTimeKind.Utc).AddTicks(3612), "$2a$11$4HCASYqUCASqcHYMfyyD1O0Qs5NzAKT0Ssj93ijEsinU6u4QsuUCe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 40, 985, DateTimeKind.Utc).AddTicks(1815), "$2a$11$QlPrWnCETSh1NxEYoYKX7.4IAM4tdJY7uOLg1j/TPhfuPvBBLQIwK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 41, 150, DateTimeKind.Utc).AddTicks(6093), "$2a$11$Rv5szvF3VXMJ3NW2GbAYIuFVjcHMOtrlaprmQuB5R4XJdgzncWG.i" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 41, 308, DateTimeKind.Utc).AddTicks(2649), "$2a$11$WZ/xP8rfLft4lnYxhbGkn.hEbObbLY5OOkPl.ccvSivSTiS3A6LYi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 41, 448, DateTimeKind.Utc).AddTicks(9499), "$2a$11$oFxwTYwTTLzhxkexjAEfieUaXfGHJBg1rCdxuH0wkFApAVVgFcV7q" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 20, 518, DateTimeKind.Utc).AddTicks(3568), "$2a$11$dWg.91rdTRYexs7zc40PduWl0PQG91QRir/xTkLbZl2hXF6qQIB4m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 20, 678, DateTimeKind.Utc).AddTicks(6234), "$2a$11$DY7KTpQrZqssG0QFgtp4xOXFWg9sDoE3vIUVKVPN8Tc94C15G/ZuS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 20, 830, DateTimeKind.Utc).AddTicks(1937), "$2a$11$4CNifz1JvU0JD4Y1z4EVXe0k6qUeUEomc5nodun4z4EictkpYemsC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 20, 984, DateTimeKind.Utc).AddTicks(9520), "$2a$11$fBFPCHo8d2FZR6K3XZOGp.v5MAqeEiHbBnS3hcB2AGyMURat8/zJq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 21, 137, DateTimeKind.Utc).AddTicks(8296), "$2a$11$9JrsOCTfEueoTjymy.G/jOdRkaJUFlcLP1pZx6i/cPCf.RKHXSwfq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 21, 274, DateTimeKind.Utc).AddTicks(1409), "$2a$11$zUUWsdg3ERIG.WLy3HAVdefaMzdipHBMDliRHS3CXGFx5dZqTxMXu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 23, 2, 43, 21, 411, DateTimeKind.Utc).AddTicks(4693), "$2a$11$RCpsBHVAMT.dMgVk46031.Rqth7Y4XVMmiyenJ6THKdzMYbg1O9LS" });
        }
    }
}
