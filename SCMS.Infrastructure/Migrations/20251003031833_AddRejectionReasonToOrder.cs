using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectionReasonToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 31, 442, DateTimeKind.Utc).AddTicks(3130), "$2a$11$zKacyCxHheQYiSr4pcsMv.OnstgFnRd.w4tHDaLz5.ZZY56SJTUae" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 31, 616, DateTimeKind.Utc).AddTicks(7666), "$2a$11$O1rLyZTmLu1jbBSoCxCwUOATQS0j9IvIl1gMB0T2NMRT0PsQdjA2u" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 31, 792, DateTimeKind.Utc).AddTicks(4342), "$2a$11$misfm0cMtDY65p7dlK1iHO.4OyLbiMIUNp/ha3eWklP07Fwr2hWlS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 31, 974, DateTimeKind.Utc).AddTicks(5768), "$2a$11$0FPlu5/RacQtzQHO3qmfCuoJvUYiuL7rJ4i4Dfn8N1zvSk.ucimja" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 32, 183, DateTimeKind.Utc).AddTicks(3829), "$2a$11$D79lu8cIWGnygv1OEQF/5uJC8si9zkn3NbGl48qc/qMxD8CxfEBKy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 32, 376, DateTimeKind.Utc).AddTicks(6890), "$2a$11$hx89v03lnJ3SAJ4aQwWxgOMMHgXkbjCZD1pAbd2yEQY5BSassstC6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 3, 3, 18, 32, 562, DateTimeKind.Utc).AddTicks(1109), "$2a$11$PEpV/2Weug.S8AC0L8BH0ewjzp8UO6S39Z.5vYhxhDqQJVGXYqpW." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Orders");

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
    }
}
