using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class AddPdfFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfPackName",
                table: "RequestPackages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPackName",
                table: "RequestPackages");
        }
    }
}
