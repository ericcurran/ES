using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.IO;

namespace DbService
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
        }

        public DbSet<RequestPackage> RequestPackages { get; set; }

        public DbSet<RecordFile> RecordFiles { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RequestManagmentDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
