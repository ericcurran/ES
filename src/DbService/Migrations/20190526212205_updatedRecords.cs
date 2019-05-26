using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbService.Migrations
{
    public partial class updatedRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfPacks");

            migrationBuilder.DropTable(
                name: "RecordFiles");

            migrationBuilder.DropTable(
                name: "RequestPackages");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ClaimNumber = table.Column<string>(nullable: true),
                    InsuredName = table.Column<string>(nullable: true),
                    DateOfLoss = table.Column<DateTime>(nullable: false),
                    DateOfService = table.Column<DateTime>(nullable: false),
                    Phase = table.Column<string>(nullable: true),
                    DeatilsFileName = table.Column<string>(nullable: true),
                    PdfPackName = table.Column<string>(nullable: true),
                    EsRef = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Archived = table.Column<bool>(nullable: false),
                    Duplicated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestId = table.Column<int>(nullable: false),
                    RequestNumber = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    InScope = table.Column<bool>(nullable: false),
                    InLog = table.Column<bool>(nullable: false),
                    EsRef = table.Column<int>(nullable: true),
                    ClaimNumber = table.Column<string>(nullable: true),
                    BundleNumber = table.Column<string>(nullable: true),
                    PageNumber = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Log = table.Column<string>(nullable: true),
                    Phase = table.Column<string>(nullable: true),
                    Archived = table.Column<bool>(nullable: false),
                    Duplicated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_RequestId",
                table: "Records",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.CreateTable(
                name: "RequestPackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimNumber = table.Column<string>(nullable: true),
                    DateOfLoss = table.Column<DateTime>(nullable: false),
                    DateOfService = table.Column<DateTime>(nullable: false),
                    DeatilsFileName = table.Column<string>(nullable: true),
                    DetailsRecordId = table.Column<int>(nullable: true),
                    EsRef = table.Column<int>(nullable: true),
                    InsuredName = table.Column<string>(nullable: true),
                    PdfPackName = table.Column<string>(nullable: true),
                    Phase = table.Column<string>(nullable: true),
                    RequestId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ZipFileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdfPacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfPacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfPacks_RequestPackages_Id",
                        column: x => x.Id,
                        principalTable: "RequestPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecordFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BundleNumber = table.Column<string>(nullable: true),
                    ClaimNumber = table.Column<string>(nullable: true),
                    EsRef = table.Column<int>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    InLog = table.Column<bool>(nullable: false),
                    InScope = table.Column<bool>(nullable: false),
                    Log = table.Column<string>(nullable: true),
                    OrderNumber = table.Column<int>(nullable: false),
                    PageNumber = table.Column<int>(nullable: false),
                    Phase = table.Column<string>(nullable: true),
                    RequestPackageId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordFiles_RequestPackages_RequestPackageId",
                        column: x => x.RequestPackageId,
                        principalTable: "RequestPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordFiles_RequestPackageId",
                table: "RecordFiles",
                column: "RequestPackageId");
        }
    }
}
