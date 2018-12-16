using FunctionWorkaround;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FtpService;
using Microsoft.Extensions.Configuration;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using DbService;
using Microsoft.EntityFrameworkCore;
using System;
using BusinessLogic;
using StorageService;

[assembly: WebJobsStartup(typeof(Startup))]
namespace FunctionWorkaround
{
    internal class Startup : IWebJobsStartup
    {

        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection(ConfigDi);          
        }

        private void ConfigDi(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .Build();
            services.AddSingleton<IConfiguration>(config);

            AddDbContext(services, config);
            AddDatabaseService(services);
            AddFtpService(services, config);
            AddZipSerive(services);
            AddBlobStorage(services, config);
            AddBusinessLogic(services);
        }

        private void AddBlobStorage(IServiceCollection services, IConfigurationRoot config)
        {
            services.AddScoped<BlobStorageService, BlobStorageService>((s) =>
            {
                string containerName = config["BlobContainer"];
                string connectionStrimg = config.GetConnectionString("BlobStorage");
                return new BlobStorageService(connectionStrimg, containerName);
            });
        }

        private void AddBusinessLogic(IServiceCollection services)
        {
            services.AddScoped<FtpLogic, FtpLogic>();
        }

        private void AddZipSerive(IServiceCollection services)
        {
            services.AddScoped<ZipServcie, ZipServcie>();
        }

        private void AddFtpService(IServiceCollection services, IConfigurationRoot config)
        {
            services.AddScoped<FtpMonitoringService, FtpMonitoringService>((s) =>
            {
                string url = config["FtpUrl"];
                string login = config["FtpLogin"];
                string psw = config["FtpPasword"];
                string processedDir = config["ProcessedDir"];
                return new FtpMonitoringService(url, login, psw, processedDir);
            });
        }

        private void AddDatabaseService(IServiceCollection services)
        {
            services.AddScoped<DataService, DataService>();
        }

        private void AddDbContext(IServiceCollection services, IConfigurationRoot config)
        {
            var connectionString = config.GetConnectionString("RequestManagmentDb");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
