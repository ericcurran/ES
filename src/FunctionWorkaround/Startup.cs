using FtpFunction;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using FtpService;

[assembly: WebJobsStartup(typeof(Startup))]
namespace FtpFunction
{
    internal class Startup : IWebJobsStartup
    {
        
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<FtpMonitoringService, FtpMonitoringService>((s) =>
            {
                string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
                string login = @"renderhubserver\RenderHubAdmin";
                string psw = "7j9u*@jyB29729S9";
                return new FtpMonitoringService(url, login, psw);
            });
        }
    }
}
