using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cornershop.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthorOfProductToSingular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorProduct");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuthorId",
                table: "Products",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AuthorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "AuthorProduct",
                columns: table => new
                {
                    AuthorsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorProduct", x => new { x.AuthorsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_AuthorProduct_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorProduct_ProductsId",
                table: "AuthorProduct",
                column: "ProductsId");
        }
    }
}
