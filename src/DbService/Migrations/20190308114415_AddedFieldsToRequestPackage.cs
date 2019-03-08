using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class AddedFieldsToRequestPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaimNumber",
                table: "RequestPackages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfLoss",
                table: "RequestPackages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfService",
                table: "RequestPackages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InsuredName",
                table: "RequestPackages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "RequestPackages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "RequestPackages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimNumber",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "DateOfLoss",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "DateOfService",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "InsuredName",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "Phase",
                table: "RequestPackages");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "RequestPackages");
        }
    }
}
