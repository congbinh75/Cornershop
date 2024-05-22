using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cornershop.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveListImagesUrlsFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesUrls",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagesUrls",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
