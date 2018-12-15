using System;
using System.Threading;
using System.Threading.Tasks;
using FtpService;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FtpFunction
{
    public class Function1
    {
        private readonly FtpMonitoringService _ftpService;

        public Function1(FtpMonitoringService ftpService)
        {
            _ftpService = ftpService;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("*/1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var files = await _ftpService.GetFileList();

            foreach (var f in files)
            {
                log.LogInformation(f);
            }
        }

        
    }
}
