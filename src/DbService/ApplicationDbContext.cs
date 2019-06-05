using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models;

namespace DbService
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
      

        public DbSet<Request> Requests { get; set; }

        public DbSet<Record> Records { get; set; }        
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                       
            //var connectionString = "Server=tcp:recordscenter.database.windows.net,1433;Initial Catalog=RequestManagmentDb;Persist Security Info=False;User ID=ServerAdmin;Password=7c%cB^z^T5&L6E8Z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RequestManagmentDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
