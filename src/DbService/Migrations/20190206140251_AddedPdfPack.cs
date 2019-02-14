using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class AddedPdfPack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestPacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestPacks_RequestPackages_Id",
                        column: x => x.Id,
                        principalTable: "RequestPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestPacks");
        }
    }
}
