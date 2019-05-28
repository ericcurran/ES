using DbService;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecordsManagmentWeb.NodeJs;
using StorageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecordsManagmentWeb.Services
{
    public class PdfGenearatorService
    {
        private readonly INodeServices _node;
        private readonly ApplicationDbContext _db;
        private readonly BlobStorageService _blobService;
        private readonly ILogger<PdfGenearatorService> _logger;
        private readonly string _tempRecordPath;
        private readonly string _nodePath;
        private readonly string _pdfFont;

        public PdfGenearatorService(INodeServices node, 
            ApplicationDbContext db, 
            BlobStorageService blobService, 
            IOptions<NodeOptions> nodeOptions,
            ILogger<PdfGenearatorService> logger)
        {
            _node = node;
            _db = db;
            _blobService = blobService;
            _logger = logger;
            _tempRecordPath = nodeOptions.Value.RecordsTempPath;
            _nodePath = nodeOptions.Value.NodeAppFile;
            _pdfFont = nodeOptions.Value.PdfFont;
        }

        public async Task<string> TestServcie(CancellationToken t)
        {
            _logger.LogInformation(_nodePath);
            var ok = await _node.InvokeExportAsync<string>(_nodePath, "testCall");
            return ok;
        }

        public async Task<string> GeneratePdf(CancellationToken stoppingToken, int id)
        {
            var tempDir = $"{_tempRecordPath}\\{Guid.NewGuid().ToString()}";
            await DownloadPackFiles(id, tempDir);
            var fileName = await _node.InvokeExportAsync<string>(_nodePath, "callFromAsp", tempDir, _pdfFont);
            await SavePdfReport(fileName, tempDir, id);
            DeletePackFiles(tempDir);
            return fileName;
        }

        private async Task SavePdfReport(string fileName, string tempDir, int requestId)
        {
            using (var file = File.OpenRead($"{tempDir}\\{fileName}"))
            {
                await _blobService.SaveFileToBlob(fileName, file);
            }
            var request = await _db.Requests.FindAsync(requestId);
            request.PdfPackName = fileName;
            await _db.SaveChangesAsync();
        }

        private void DeletePackFiles(string tempDir)
        {
            Directory.Delete(tempDir, true);
        }

        private async Task DownloadPackFiles(int id, string tempDir)
        {
            var jsonData = await GetFileNames(id);
            if (jsonData == null)
            {
                return;
            }
            await DownloadFileToLocalFolder(jsonData, tempDir);          
        }

        private async Task DownloadFileToLocalFolder(PdfJsonData jsonData, string tempDir)
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
            Directory.CreateDirectory(tempDir);

            var savedTasks = new Dictionary<string,Task<Stream>>();
            foreach (var file in jsonData.LogItems.Select(f=>f.FileName))
            {
                var t = _blobService.DownloadFile(file);
                savedTasks.Add(file, t);
                if (savedTasks.Count == 50)
                {
                    await Task.WhenAll(savedTasks.Values);                     
                    foreach (var kv in savedTasks)
                    {
                        await SaveRecordOnDisk(kv.Key, kv.Value.Result, tempDir);
                    }
                    savedTasks.Clear();
                }
            }
            if (savedTasks.Count > 0)
            {
                await Task.WhenAll(savedTasks.Values);
                foreach (var kv in savedTasks)
                {
                    await SaveRecordOnDisk(kv.Key, kv.Value.Result, tempDir);
                }
            }

            await SaveJsonOnDisk(jsonData, tempDir);
        }

        private async Task SaveJsonOnDisk(PdfJsonData jsonData, string tempDir)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            string json = JsonConvert.SerializeObject(jsonData, jsonSettings);
            using (var jsonFile = new StreamWriter(Path.Combine(tempDir, "pdf-data.json")))
            {
                await jsonFile.WriteLineAsync(json);
            }
        }

        private async  Task SaveRecordOnDisk(string fileName, Stream result, string tempDir)
        {
            using (var fileStream = new FileStream($"{tempDir}\\{fileName}", FileMode.CreateNew))
            {
                result.Position = 0;
                await result.CopyToAsync(fileStream);       
            }
        }

        private async Task<PdfJsonData> GetFileNames(int id)
        {
            Directory.Exists(_tempRecordPath);
            var requestPack = await _db.Requests.AsNoTracking().FirstOrDefaultAsync(rp => rp.Id == id);
            if (requestPack.DeatilsFileName == null)
            {
                return null;
            }
            var files = new List<PdfItemData>(new[] {
                new PdfItemData(){FileName = requestPack.DeatilsFileName, InLog=false}
            });
            var recordFiles = await _db.Records.Where(f => f.RequestId == id && f.InScope)
                                               .Select(f => new PdfItemData
                                               {
                                                   FileName = f.FileName,
                                                   InLog = f.InLog,
                                                   Log = f.InLog ? f.Log : null
                                               })
                                               .ToListAsync();
            files.AddRange(recordFiles);
            var titleData = GetLogTitle(requestPack);
            return new PdfJsonData() { LogItems = files, LogTitle = titleData };
        }

        private IEnumerable<string> GetLogTitle(Request requestPack)
        {
            var titleLines = new List<string>();
            titleLines.Add($"Today's Date: {DateTime.Now.ToString("MM/dd/yyyy")}");
            titleLines.Add($"Claimant's Date: {requestPack.InsuredName ?? "Not Defiend"}");
            titleLines.Add($"Claim Number: {requestPack.ClaimNumber ?? "Not Defiend"}");
            titleLines.Add($"Date of Accident: {requestPack.DateOfLoss?.ToString("MM/dd/yyyy") ?? ""}");
            titleLines.Add($"Date(s) of Service: {requestPack.DateOfService?.ToString("MM/dd/yyyy") ?? ""}");
            titleLines.Add("Undated, Procedure consent formsigned by claimant");
            return titleLines;            
        }
    }
}
