using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clothes_backend.Migrations
{
    /// <inheritdoc />
    public partial class initial_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "row_version",
                table: "product_variants");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "cart_items",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "row_version",
                table: "cart_items");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "product_variants",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
