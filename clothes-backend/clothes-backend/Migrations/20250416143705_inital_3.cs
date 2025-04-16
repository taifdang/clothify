using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clothes_backend.Migrations
{
    /// <inheritdoc />
    public partial class inital_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "passwordSalt",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passwordSalt",
                table: "users");
        }
    }
}
