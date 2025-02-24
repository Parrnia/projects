using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyx.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPayedToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPayment_Orders_OrderId",
                table: "OrderPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPayment",
                table: "OrderPayment");

            migrationBuilder.DropIndex(
                name: "IX_OrderPayment_OrderId",
                table: "OrderPayment");

            migrationBuilder.RenameTable(
                name: "OrderPayment",
                newName: "OrderPayments");

            migrationBuilder.AddColumn<bool>(
                name: "IsPayed",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "پرداخت شده");

            migrationBuilder.AddColumn<int>(
                name: "OrderPaymentType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "شیوه پرداخت");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "OrderPayments",
                type: "bigint",
                nullable: true,
                comment: "مبلغ پراختی",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPayments",
                table: "OrderPayments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_OrderId",
                table: "OrderPayments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPayments_Orders_OrderId",
                table: "OrderPayments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPayments_Orders_OrderId",
                table: "OrderPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPayments",
                table: "OrderPayments");

            migrationBuilder.DropIndex(
                name: "IX_OrderPayments_OrderId",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderPaymentType",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderPayments",
                newName: "OrderPayment");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "OrderPayment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "مبلغ پراختی");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPayment",
                table: "OrderPayment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayment_OrderId",
                table: "OrderPayment",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPayment_Orders_OrderId",
                table: "OrderPayment",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
