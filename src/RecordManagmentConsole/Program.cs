using BusinessLogic;
using DbService;
using FtpService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StorageService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RecordManagmentConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
             .ConfigureHostConfiguration(configHost =>
             {
                 configHost.SetBasePath(Directory.GetCurrentDirectory());                 
             })
             .ConfigureAppConfiguration((hostContext, configApp) =>
             {
                 configApp.AddJsonFile("appsettings.json", optional: true);
                 configApp.AddJsonFile("appsettings.Development.json", optional: true);
             })
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddLogging();

                 AddAppServices(hostContext.Configuration, services);

             })
             .ConfigureLogging((hostContext, configLogging) =>
             {
                 configLogging.AddConsole();

             })
             .UseConsoleLifetime()
             .Build();

            await host.RunAsync();
        }

        private static void AddAppServices(IConfiguration config, IServiceCollection services)
        {
            var connectionString = config.GetConnectionString("RequestManagmentDb");
            var storageConnectionString = config.GetConnectionString("BlobStorage");
            var blobContainer = config["BlobContainer"];
            string ftpUrl = config["FtpUrl"];
            string ftpLogin = config["FtpLogin"];
            string ftpPassword = config["FtpPasword"];
            string processedDir = config["ProcessedDir"];

            services.AddTransient((s) => new DataService(connectionString));
            services.AddTransient(s =>
            {
                var logger = s.GetService<ILogger<BlobStorageService>>();
                return new BlobStorageService(storageConnectionString, blobContainer, logger);
            });
            services.AddTransient(s =>
            {
                var logger = s.GetService<ILogger<FtpClient>>();
                return new FtpClient(ftpUrl, ftpLogin, ftpPassword, processedDir, logger);
            });
            services.AddTransient<AppLogic>();
            services.AddHostedService<HostedAppService>();
        }


    }
}
