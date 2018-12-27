using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Models;

namespace FtpFunction
{
    public static class MonitoringOrchestration
    {
        [FunctionName("MonitoringOrchestration")]
        public static async Task<List<RequestPackage>> RunOrchestrator(
             [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var newZipFileName = await context.CallActivityAsync<IEnumerable<RequestPackage>>("GetZipFileNames",null);

            var outputs = new List<RequestPackage>();
            foreach (var zip in newZipFileName)
            {
                string instanceId = $"{Guid.NewGuid().ToString()}_{zip}";
                await context.CallSubOrchestratorAsync("PrcessZipFile", instanceId, zip);
            }           
            return outputs;
        }

        [FunctionName("GetZipFileNames")]
        public static async Task<IEnumerable<RequestPackage>> GetZipFiles([ActivityTrigger] object o, ILogger log)
        {
            var newZipFileName = await FtpLogic.GetZipFilesFromFtp();
            return newZipFileName;
        }

        [FunctionName("PrcessZipFile")]
        public static async Task ProcessZipFile(
                     [OrchestrationTrigger] DurableOrchestrationContext context,                                                       
                                                                ILogger log)
        {
            var zipPackage = context.GetInput<RequestPackage>();

            var tempZipFile = await context.CallActivityAsync<string>("DownloadFile", zipPackage);

            var downloadTask = FtpLogic.DownloadfromFtp(zipPackage);
            downloadTask.Wait();

            var zipEntires = FtpLogic.GetZipEntries(downloadTask.Result);

            foreach (var record in zipEntires)
            {
               
                await context.CallActivityAsync("SaveRecordToBlob", record);
                await context.CallActivityAsync("SaveRecordToDb", new RecordFile()
                {
                    FileName = record.Name,
                    RequestPackageId = zipPackage.Id,
                    Status = RecordStatusEnum.SavedToBlobStorage                    
                });
            }
            await context.CallActivityAsync("CleanFtp", zipPackage);
        }

        //[FunctionName("DownloadFile")]
        //public static async Task<Stream> DownloadZipPackage([ActivityTrigger] RequestPackage zipPackage, ILogger log)
        //{
        //    var downloadedFile = await FtpLogic.DownloadfromFtp(zipPackage);
        //    return downloadedFile;
        //}

        [FunctionName("CleanFtp")]
        public static async Task CleanFtp([ActivityTrigger] RequestPackage zipPackage, ILogger log)
        {
            await FtpLogic.CleanFtp(zipPackage);
        }

        [FunctionName("SaveRecordToDb")]
        public static async Task SaveRecordToDb([ActivityTrigger] RecordFile record)
        {
            await FtpLogic.SaveRecordAndUpdateRequest(record);
        }

        [FunctionName("SaveRecordToBlob")]
        public static void SaveRecordToBlob([ActivityTrigger] ZipArchiveEntry zipRecord,
             [Blob("documents-test/{zipRecord.Name}", FileAccess.Write, Connection = "BlobStorage")]
                                                                            Stream recordToSave,
                                                                           ILogger log)
        {
            recordToSave = zipRecord.Open();
            log.LogInformation($"file {zipRecord.Name} started to save in blob");
        }




        [FunctionName("MonitoringOrchestration_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("MonitoringOrchestration", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}