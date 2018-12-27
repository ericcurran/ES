using System;
using System.Threading.Tasks;
using BusinessLogic;
using FtpService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtpFunction
{
    public static class RequestProcessing
    {
        //[FunctionName("FtpProcessing")]
        public static async Task Run([TimerTrigger("* */5 * * * *")]TimerInfo myTimer,
                                                                      ILogger log)
        {
            log.LogInformation($"Service started at {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}.");
            await FtpLogic.GetZipFilesFromFtp();
            log.LogInformation($"Service finished at {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}.");

        }

        //[FunctionName("HttpFunction")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    return name != null
        //        ? (ActionResult)new OkObjectResult($"Hello, {name}")
        //        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        //}
    }
}