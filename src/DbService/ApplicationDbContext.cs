using Microsoft.EntityFrameworkCore;
using Models;
using System;

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
    
}
