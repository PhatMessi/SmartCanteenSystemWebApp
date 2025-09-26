using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 17, 541, DateTimeKind.Utc).AddTicks(7552), "$2a$11$w/CZgByTuWDxGfuj5IFd3uIUVfsFQ.j6paTUXS/ZCqyM09BOmiRdG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 17, 719, DateTimeKind.Utc).AddTicks(3097), "$2a$11$sILGQLS01kR6HQj7x09Z8OCkcR7zNscmi0NSXGabrkotOl/Da/l6e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 17, 886, DateTimeKind.Utc).AddTicks(935), "$2a$11$UT3FvqyXCgHgQLzauwide.y2EoHaVLvXPhIEekKYBhxY.EHLupay2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 18, 65, DateTimeKind.Utc).AddTicks(5745), "$2a$11$WqX89X6JeoB3KZPARU0K8u2Foy7.0eBy1hkYopV3p20cZg2nyUtl." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 18, 256, DateTimeKind.Utc).AddTicks(7821), "$2a$11$7oDNiQyA/l7XXQY9v6UiWuE4k2GLkJR9nMzNZmQhjp6dfOnYKGtAm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 18, 431, DateTimeKind.Utc).AddTicks(23), "$2a$11$2zRgLYjKJR9/QWrX.YcdmuoO5TRYgm50BHw..J9DBhCyyv/aToNK." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 26, 3, 6, 18, 589, DateTimeKind.Utc).AddTicks(7271), "$2a$11$GpccuaBqca1.5bJGaxHr9uhjcTkdMba/ojPbTWMR3PgL0IVb6Vyxq" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

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
    }
}
