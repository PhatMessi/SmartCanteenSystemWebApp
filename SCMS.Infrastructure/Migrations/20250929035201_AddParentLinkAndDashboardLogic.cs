using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParentLinkAndDashboardLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 51, 59, 609, DateTimeKind.Utc).AddTicks(6730), "$2a$11$DU35zXPv1WnkaaUwcLqgou5blSbJ1OWquMkWgziKoub3VfChILbay" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 51, 59, 769, DateTimeKind.Utc).AddTicks(6390), "$2a$11$NM/AzwziHfi2RTzZYoloi.kiTPdoNEuAvcxKLLz5230CAueiXyBg6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 51, 59, 944, DateTimeKind.Utc).AddTicks(6120), "$2a$11$bExJyvBSNAlBVTNvc.MVQehQgrXLuFSEP4./T.PeCoKDbWWAasV/G" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 52, 0, 154, DateTimeKind.Utc).AddTicks(5634), "$2a$11$bQxwsalMgJJSkNqKglJNC.GTmNqKdiJIZdCPuCEGmUh7jP8RZYMsi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 52, 0, 366, DateTimeKind.Utc).AddTicks(8013), "$2a$11$NWWQ1REoltK9JQIKRN5EvO3K6jKgM1w9Kzrf76EXimFR9BtPXCccK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 52, 0, 561, DateTimeKind.Utc).AddTicks(4313), "$2a$11$TTDOi.L95PEt9WVvlxTqSuS.7gDZP6HMadZDT40bvtlmuoQ4Qqg3S" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 29, 3, 52, 0, 717, DateTimeKind.Utc).AddTicks(2597), "$2a$11$8G.6ScW0yw.UWycEci225eVHjxV1lX8xqpW4pCAikocG3QEkas7qK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
