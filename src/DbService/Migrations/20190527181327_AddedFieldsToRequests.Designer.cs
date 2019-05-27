﻿// <auto-generated />
using System;
using DbService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DbService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190527181327_AddedFieldsToRequests")]
    partial class AddedFieldsToRequests
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived");

                    b.Property<string>("BundleNumber");

                    b.Property<string>("ClaimNumber");

                    b.Property<bool>("Duplicated");

                    b.Property<int?>("EsRef");

                    b.Property<string>("FileName");

                    b.Property<bool>("InLog");

                    b.Property<bool>("InScope");

                    b.Property<string>("Log");

                    b.Property<int>("OrderNumber");

                    b.Property<int>("PageNumber");

                    b.Property<string>("Phase");

                    b.Property<int>("RequestId");

                    b.Property<string>("RequestNumber");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<bool>("Archived");

                    b.Property<string>("ClaimNumber");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfLoss");

                    b.Property<DateTime>("DateOfService");

                    b.Property<string>("DeatilsFileName");

                    b.Property<bool>("Duplicated");

                    b.Property<int?>("EsRef");

                    b.Property<string>("InsuredName");

                    b.Property<string>("PdfPackName");

                    b.Property<string>("Phase");

                    b.Property<string>("RequestId");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Models.Record", b =>
                {
                    b.HasOne("Models.Request", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
