using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class RanemedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPacks_RequestPackages_Id",
                table: "RequestPacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestPacks",
                table: "RequestPacks");

            migrationBuilder.RenameTable(
                name: "RequestPacks",
                newName: "PdfPacks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PdfPacks",
                table: "PdfPacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PdfPacks_RequestPackages_Id",
                table: "PdfPacks",
                column: "Id",
                principalTable: "RequestPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfPacks_RequestPackages_Id",
                table: "PdfPacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PdfPacks",
                table: "PdfPacks");

            migrationBuilder.RenameTable(
                name: "PdfPacks",
                newName: "RequestPacks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestPacks",
                table: "RequestPacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPacks_RequestPackages_Id",
                table: "RequestPacks",
                column: "Id",
                principalTable: "RequestPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
