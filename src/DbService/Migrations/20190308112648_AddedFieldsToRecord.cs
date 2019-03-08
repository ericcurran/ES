using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class AddedFieldsToRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BundleNumber",
                table: "RecordFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimNumber",
                table: "RecordFiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InLog",
                table: "RecordFiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Log",
                table: "RecordFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "RecordFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PageNumber",
                table: "RecordFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "RecordFiles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RecordFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BundleNumber",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "ClaimNumber",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "InLog",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "Log",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "PageNumber",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "Phase",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RecordFiles");
        }
    }
}
