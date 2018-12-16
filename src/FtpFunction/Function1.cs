using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FtpService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtpFunction
{
    public class RequestProcessing
    {
        private readonly FtpMonitoringService _ftpService;
        
        public RequestProcessing(FtpMonitoringService ftpService)
        {
            _ftpService = ftpService;            
        }

        [FunctionName("FtpNotification")]
        public static async Task<HttpResponseMessage> Post([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req, ILogger log)
        {

            string content = await req.Content.ReadAsStringAsync();
            
            log.LogInformation(content);
            return req.CreateResponse(HttpStatusCode.OK);
           
            //var files = await _ftpService.GetFileList();

            //foreach (var f in files)
            //{
            //    log.LogInformation(f);
            //}
        }

        [FunctionName("HttpFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}