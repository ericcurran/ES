using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class UpdatedRequestsAndRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "RequestPackages",
                newName: "ZipFileName");

            migrationBuilder.AddColumn<string>(
                name: "DeatilsFileName",
                table: "RequestPackages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetailsRecordId",
                table: "RequestPackages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestPackageId",
                table: "RecordFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecordFiles_RequestPackageId",
                table: "RecordFiles",
                column: "RequestPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordFiles_RequestPackages_RequestPackageId",
                table: "RecordFiles",
                column: "RequestPackageId",
                principalTable: "RequestPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordFiles_RequestPackages_RequestPackageId",
                table: "RecordFiles");

            migrationBuilder.DropIndex(
                name: "IX_RecordFiles_RequestPackageId",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "DeatilsFileName",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "DetailsRecordId",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "RequestPackageId",
                table: "RecordFiles");

            migrationBuilder.RenameColumn(
                name: "ZipFileName",
                table: "RequestPackages",
                newName: "FileName");
        }
    }
}
