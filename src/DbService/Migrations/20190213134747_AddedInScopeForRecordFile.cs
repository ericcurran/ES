using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class AddedInScopeForRecordFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InScope",
                table: "RecordFiles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InScope",
                table: "RecordFiles");
        }
    }
}
