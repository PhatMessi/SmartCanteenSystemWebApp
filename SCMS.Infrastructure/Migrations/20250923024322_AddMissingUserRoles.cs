using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 5, "Parent" },
                    { 6, "HeadTeacher" },
                    { 7, "SchoolAdministration" }
                });

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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "HeadTeacherId", "MustChangePassword", "ParentId", "PasswordHash", "PasswordResetToken", "ResetTokenExpires", "RoleId" },
                values: new object[,]
                {
                    { 15, new DateTime(2025, 9, 23, 2, 43, 21, 137, DateTimeKind.Utc).AddTicks(8296), "parent@scms.com", "Default Parent", null, false, null, "$2a$11$9JrsOCTfEueoTjymy.G/jOdRkaJUFlcLP1pZx6i/cPCf.RKHXSwfq", null, null, 5 },
                    { 16, new DateTime(2025, 9, 23, 2, 43, 21, 274, DateTimeKind.Utc).AddTicks(1409), "headteacher@scms.com", "Default Head Teacher", null, false, null, "$2a$11$zUUWsdg3ERIG.WLy3HAVdefaMzdipHBMDliRHS3CXGFx5dZqTxMXu", null, null, 6 },
                    { 17, new DateTime(2025, 9, 23, 2, 43, 21, 411, DateTimeKind.Utc).AddTicks(4693), "schooladmin@scms.com", "School Admin User", null, false, null, "$2a$11$RCpsBHVAMT.dMgVk46031.Rqth7Y4XVMmiyenJ6THKdzMYbg1O9LS", null, null, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 57, 148, DateTimeKind.Utc).AddTicks(8570), "$2a$11$OJGn4D4P9yc68FDLoGMSSeQv5RBDhtDtBgw14cXyfTKI2fjni/FIm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 57, 556, DateTimeKind.Utc).AddTicks(2302), "$2a$11$oMZ7rSGqHgsO1hrg2Iwjv.SWsH8mEOVg5GQRLtpNwwXHcI.YMqkNS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 58, 162, DateTimeKind.Utc).AddTicks(1616), "$2a$11$lHp4KY.HLzQ.ZOqpZ4BzQu5EJPg7gtoOlSB5WeholDBeTalWXXM2W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 58, 652, DateTimeKind.Utc).AddTicks(43), "$2a$11$h9VC0bqbOWnX4Xp4N5u0Iehtg4rErnPitR9M9LsLhdmaUF18J7Jva" });
        }
    }
}
