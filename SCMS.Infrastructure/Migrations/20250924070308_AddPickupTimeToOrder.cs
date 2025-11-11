using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPickupTimeToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PickupTime",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 6, 858, DateTimeKind.Utc).AddTicks(3540), "$2a$11$9PjuDrKG15wYIaGE1W6sIOCkOlhGjIpaHQMuJoiMhUSZ90FyreLOa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 31, DateTimeKind.Utc).AddTicks(64), "$2a$11$GDWd9lGFpdwLdkk0JmLWpOnlnCkEZaGiivL0uk9odSpBZLvjEZn6C" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 199, DateTimeKind.Utc).AddTicks(8369), "$2a$11$6BaBRoycaIoQ9jEOZ2v0FuFov0qabUY2XXpg8YcAVf.IvNExu26C." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 389, DateTimeKind.Utc).AddTicks(6666), "$2a$11$j4MR1VLRmGQDq3s.jUD9TOpDn4iFdeW2yQxm34JOrpv1at79QuKNu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 573, DateTimeKind.Utc).AddTicks(4525), "$2a$11$HB4rSIo4m5tsipPJhdWD2eZJYb.AJuWQzZoN6GAdr7vk0rJUWu.eS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 737, DateTimeKind.Utc).AddTicks(2780), "$2a$11$rF5y6y.5FbHnAWC2fcyjmeNvR2PMuqyY4Gd1jW58hIA6NxfTYViSy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 24, 7, 3, 7, 896, DateTimeKind.Utc).AddTicks(8670), "$2a$11$GjV4AVe.nyO/sXX4dZH7HuQ7okN4YDTRw7aFzPunSPYKNqNoMpvJa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "Orders");

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
    }
}
