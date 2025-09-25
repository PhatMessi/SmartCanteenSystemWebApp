using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletAndUserLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Transactions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Transactions",
                newName: "TransactionDate");

            migrationBuilder.AddColumn<int>(
                name: "HeadTeacherId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HeadTeacherId", "ParentId", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 57, 148, DateTimeKind.Utc).AddTicks(8570), null, null, "$2a$11$OJGn4D4P9yc68FDLoGMSSeQv5RBDhtDtBgw14cXyfTKI2fjni/FIm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "HeadTeacherId", "ParentId", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 57, 556, DateTimeKind.Utc).AddTicks(2302), null, null, "$2a$11$oMZ7rSGqHgsO1hrg2Iwjv.SWsH8mEOVg5GQRLtpNwwXHcI.YMqkNS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "HeadTeacherId", "ParentId", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 58, 162, DateTimeKind.Utc).AddTicks(1616), null, null, "$2a$11$lHp4KY.HLzQ.ZOqpZ4BzQu5EJPg7gtoOlSB5WeholDBeTalWXXM2W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "HeadTeacherId", "ParentId", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 22, 13, 34, 58, 652, DateTimeKind.Utc).AddTicks(43), null, null, "$2a$11$h9VC0bqbOWnX4Xp4N5u0Iehtg4rErnPitR9M9LsLhdmaUF18J7Jva" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_HeadTeacherId",
                table: "Users",
                column: "HeadTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParentId",
                table: "Users",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_HeadTeacherId",
                table: "Users",
                column: "HeadTeacherId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_HeadTeacherId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Users_HeadTeacherId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ParentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "HeadTeacherId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Transactions",
                newName: "PaymentStatus");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Transactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 516, DateTimeKind.Utc).AddTicks(1135), "$2a$11$ZX5973CxeTZzoOKqlIpRouGs0qvcqH1nwnj1mp.R6fFfHCXZ8b7p." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 654, DateTimeKind.Utc).AddTicks(6530), "$2a$11$L6p5Dt3wtaXm9WyQtgtR1OQEp0JLow40fbVt.kONJxpNntCN5yxwS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 807, DateTimeKind.Utc).AddTicks(959), "$2a$11$lZkZyyhUk9VL1YFee3s4mOvA6I2vbv3mEidXnqwJ7KIij7lJWA7mW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 18, 2, 17, 17, 966, DateTimeKind.Utc).AddTicks(6858), "$2a$11$htVnarQWU0rsYkX5L1AlMeyy3icYz.iKFh4bfYbrWjxmrZyQUn4D2" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderId",
                table: "Transactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
