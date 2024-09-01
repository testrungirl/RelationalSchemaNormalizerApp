using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelationalSchemaNormalizerUI.Migrations
{
    /// <inheritdoc />
    public partial class DBInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatabaseDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataBaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatabaseDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableDetails_DatabaseDetails_DatabaseDetailId",
                        column: x => x.DatabaseDetailId,
                        principalTable: "DatabaseDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedTables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Is2ndNF = table.Column<bool>(type: "bit", nullable: false),
                    Is3rdNF = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedTables_TableDetails_TableDetailId",
                        column: x => x.TableDetailId,
                        principalTable: "TableDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributeDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyAttribute = table.Column<bool>(type: "bit", nullable: false),
                    TableDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeneratedTableId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeDetails_GeneratedTables_GeneratedTableId",
                        column: x => x.GeneratedTableId,
                        principalTable: "GeneratedTables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttributeDetails_TableDetails_TableDetailId",
                        column: x => x.TableDetailId,
                        principalTable: "TableDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDetails_GeneratedTableId",
                table: "AttributeDetails",
                column: "GeneratedTableId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDetails_TableDetailId",
                table: "AttributeDetails",
                column: "TableDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedTables_TableDetailId",
                table: "GeneratedTables",
                column: "TableDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TableDetails_DatabaseDetailId",
                table: "TableDetails",
                column: "DatabaseDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeDetails");

            migrationBuilder.DropTable(
                name: "GeneratedTables");

            migrationBuilder.DropTable(
                name: "TableDetails");

            migrationBuilder.DropTable(
                name: "DatabaseDetails");
        }
    }
}
