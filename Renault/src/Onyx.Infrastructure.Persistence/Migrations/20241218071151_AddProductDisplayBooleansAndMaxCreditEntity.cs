using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyx.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDisplayBooleansAndMaxCreditEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBestSeller",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "پرفروش");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "ویژه");

            migrationBuilder.AddColumn<bool>(
                name: "IsLatest",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "آخرین");

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "جدید");

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "محبوب");

            migrationBuilder.AddColumn<bool>(
                name: "IsSale",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "حراج");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialOffer",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "پیشنهاد ویژه");

            migrationBuilder.AddColumn<bool>(
                name: "IsTopRated",
                table: "ProductDisplayVariants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "بالاترین امتیاز");

            migrationBuilder.CreateTable(
                name: "MaxCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifierUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifierUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaxCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaxCredits_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaxCredits_CustomerId",
                table: "MaxCredits",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaxCredits");

            migrationBuilder.DropColumn(
                name: "IsBestSeller",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsLatest",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsSale",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsSpecialOffer",
                table: "ProductDisplayVariants");

            migrationBuilder.DropColumn(
                name: "IsTopRated",
                table: "ProductDisplayVariants");
        }
    }
}
