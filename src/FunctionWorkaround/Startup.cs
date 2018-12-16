using FtpFunction;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FtpService;
using Microsoft.Extensions.Configuration;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace FtpFunction
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

            var connectionString = config.GetConnectionString("RequestManagmentDb");
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<FtpMonitoringService, FtpMonitoringService>((s) =>
            {
                string url = config["FtpUrl"];
                string login = config["FtpLogin"];
                string psw = config["FtpPasword"];
                return new FtpMonitoringService(url, login, psw);
            });
        }
    }
}
