using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class ChangePageNumType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "PageNumber",
               table: "RecordFiles");

            migrationBuilder.AddColumn<int>(
                name: "PageNumber",
                table: "RecordFiles",
                nullable: false,
                defaultValue:0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "PageNumber",
             table: "RecordFiles");

            migrationBuilder.AddColumn<string>(
                name: "PageNumber",
                table: "RecordFiles",
                nullable: true);
        }
    }
}
