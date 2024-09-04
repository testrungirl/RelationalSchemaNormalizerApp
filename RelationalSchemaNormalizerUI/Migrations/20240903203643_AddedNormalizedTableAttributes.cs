using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelationalSchemaNormalizerUI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNormalizedTableAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeDetails_GeneratedTables_GeneratedTableId",
                table: "AttributeDetails");

            migrationBuilder.DropIndex(
                name: "IX_AttributeDetails_GeneratedTableId",
                table: "AttributeDetails");

            migrationBuilder.DropColumn(
                name: "GeneratedTableId",
                table: "AttributeDetails");

            migrationBuilder.CreateTable(
                name: "GenTableAttributeDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyAttribute = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedTableId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenTableAttributeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenTableAttributeDetails_GeneratedTables_GeneratedTableId",
                        column: x => x.GeneratedTableId,
                        principalTable: "GeneratedTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenTableAttributeDetails_GeneratedTableId",
                table: "GenTableAttributeDetails",
                column: "GeneratedTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenTableAttributeDetails");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedTableId",
                table: "AttributeDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDetails_GeneratedTableId",
                table: "AttributeDetails",
                column: "GeneratedTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeDetails_GeneratedTables_GeneratedTableId",
                table: "AttributeDetails",
                column: "GeneratedTableId",
                principalTable: "GeneratedTables",
                principalColumn: "Id");
        }
    }
}
