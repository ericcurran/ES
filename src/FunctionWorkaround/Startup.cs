using FtpFunction;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using FtpService;
using Microsoft.Extensions.Configuration;

[assembly: WebJobsStartup(typeof(Startup))]
namespace FtpFunction
{
    internal class Startup : IWebJobsStartup
    {
       
        public void Configure(IWebJobsBuilder builder)
        {
            var config = new ConfigurationBuilder()
                         .AddEnvironmentVariables()
                         .Build();
            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddTransient<FtpMonitoringService, FtpMonitoringService>((s) =>
            {
                string url = config["FtpUrl"];
                string login = config["FtpLogin"];
                string psw = config["FtpPasword"];
                return new FtpMonitoringService(url, login, psw);
            });
        }
    }
}
