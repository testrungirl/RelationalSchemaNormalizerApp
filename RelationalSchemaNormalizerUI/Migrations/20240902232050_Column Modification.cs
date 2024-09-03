using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelationalSchemaNormalizerUI.Migrations
{
    /// <inheritdoc />
    public partial class ColumnModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is2ndNF",
                table: "GeneratedTables");

            migrationBuilder.DropColumn(
                name: "Is3rdNF",
                table: "GeneratedTables");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "TableDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LevelOfNF",
                table: "TableDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelOfNF",
                table: "GeneratedTables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelOfNF",
                table: "TableDetails");

            migrationBuilder.DropColumn(
                name: "LevelOfNF",
                table: "GeneratedTables");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "TableDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Is2ndNF",
                table: "GeneratedTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is3rdNF",
                table: "GeneratedTables",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
