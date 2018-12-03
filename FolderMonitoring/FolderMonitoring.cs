using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace FolderMonitoring
{
    public static class FolderMonitoring
    {
        [FunctionName("FolderMonitoring")]
        public static void Run([TimerTrigger("* */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
