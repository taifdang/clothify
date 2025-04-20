using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clothes_backend.Migrations
{
    /// <inheritdoc />
    public partial class inital_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_checkout",
                table: "cart_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_checkout",
                table: "cart_items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
