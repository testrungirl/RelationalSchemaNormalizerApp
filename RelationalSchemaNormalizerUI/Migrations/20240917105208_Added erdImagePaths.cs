using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelationalSchemaNormalizerUI.Migrations
{
    /// <inheritdoc />
    public partial class AddederdImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPathFor2NF",
                table: "TableDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgPathFor3NF",
                table: "TableDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPathFor2NF",
                table: "TableDetails");

            migrationBuilder.DropColumn(
                name: "ImgPathFor3NF",
                table: "TableDetails");
        }
    }
}
