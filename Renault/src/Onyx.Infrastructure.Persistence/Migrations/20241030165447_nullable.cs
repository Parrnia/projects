using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyx.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrganizationType",
                table: "ReturnOrderReasons",
                type: "int",
                nullable: true,
                comment: "نوع دلیل سمت سازمان",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "نوع دلیل سمت سازمان");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerType",
                table: "ReturnOrderReasons",
                type: "int",
                nullable: true,
                comment: "نوع دلیل سمت مشتری",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "نوع دلیل سمت مشتری");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrganizationType",
                table: "ReturnOrderReasons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "نوع دلیل سمت سازمان",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "نوع دلیل سمت سازمان");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerType",
                table: "ReturnOrderReasons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "نوع دلیل سمت مشتری",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "نوع دلیل سمت مشتری");
        }
    }
}
