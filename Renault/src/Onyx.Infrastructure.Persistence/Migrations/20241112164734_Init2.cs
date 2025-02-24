using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyx.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Models_LocalizedName",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_Name",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Kinds_LocalizedName",
                table: "Kinds");

            migrationBuilder.DropIndex(
                name: "IX_Kinds_Name",
                table: "Kinds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Models_LocalizedName",
                table: "Models",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_LocalizedName",
                table: "Kinds",
                column: "LocalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_Name",
                table: "Kinds",
                column: "Name",
                unique: true);
        }
    }
}
