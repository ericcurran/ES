﻿// <auto-generated />
using System;
using DbService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DbService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.PdfPack", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("FileName");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("PdfPacks");
                });

            modelBuilder.Entity("Models.RecordFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BundleNumber");

                    b.Property<string>("ClaimNumber");

                    b.Property<int?>("EsRef");

                    b.Property<string>("FileName");

                    b.Property<bool>("InLog");

                    b.Property<bool>("InScope");

                    b.Property<string>("Log");

                    b.Property<int>("OrderNumber");

                    b.Property<int>("PageNumber");

                    b.Property<string>("Phase");

                    b.Property<int>("RequestPackageId");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("RequestPackageId");

                    b.ToTable("RecordFiles");
                });

            modelBuilder.Entity("Models.RequestPackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimNumber");

                    b.Property<DateTime>("DateOfLoss");

                    b.Property<DateTime>("DateOfService");

                    b.Property<string>("DeatilsFileName");

                    b.Property<int?>("DetailsRecordId");

                    b.Property<int?>("EsRef");

                    b.Property<string>("InsuredName");

                    b.Property<string>("PdfPackName");

                    b.Property<string>("Phase");

                    b.Property<string>("RequestId");

                    b.Property<int>("Status");

                    b.Property<string>("ZipFileName");

                    b.HasKey("Id");

                    b.ToTable("RequestPackages");
                });

            modelBuilder.Entity("Models.PdfPack", b =>
                {
                    b.HasOne("Models.RequestPackage", "RequestPackage")
                        .WithOne("PdfPack")
                        .HasForeignKey("Models.PdfPack", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Models.RecordFile", b =>
                {
                    b.HasOne("Models.RequestPackage", "RequestPackage")
                        .WithMany()
                        .HasForeignKey("RequestPackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
