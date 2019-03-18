using DbService;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var request = await _db.RequestPackages.FindAsync(requestId);
            request.PdfPackName = fileName;
            await _db.SaveChangesAsync();
        }

        private void DeletePackFiles(string tempDir)
        {
            Directory.Delete(tempDir, true);
        }

        private async Task DownloadPackFiles(int id, string tempDir)
        {
            var files = await GetFileNames(id);
            if (files == null)
            {
                return;
            }
            await DownloadFileToLocalFolder(files, tempDir);          
        }

        private async Task DownloadFileToLocalFolder(IEnumerable<PdfItemData> files, string tempDir)
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
            Directory.CreateDirectory(tempDir);

            var savedTasks = new Dictionary<string,Task<Stream>>();
            foreach (var file in files.Select(f=>f.FileName))
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

            await SaveJsonOnDisk(files, tempDir);
        }

        private async Task SaveJsonOnDisk(IEnumerable<PdfItemData> files, string tempDir)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            string json = JsonConvert.SerializeObject(files, jsonSettings );
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

        private async Task<IEnumerable<PdfItemData>> GetFileNames(int id)
        {
            Directory.Exists(_tempRecordPath);
            var requestPack = await _db.RequestPackages.AsNoTracking().FirstOrDefaultAsync(rp => rp.Id == id);
            if (requestPack.DeatilsFileName == null)
            {
                return null;
            }
            var files = new List<PdfItemData>(new[] {
                new PdfItemData(){FileName = requestPack.DeatilsFileName, InLog=false}
            });
            IEnumerable<PdfItemData> recordFiles = await _db.RecordFiles
                                                       .Where(f => f.RequestPackageId == id && f.InScope 
                                                                && f.Id!=requestPack.DetailsRecordId)
                                                       .Select(f => new PdfItemData()
                                                       {
                                                           FileName = f.FileName,
                                                           InLog    = f.InLog,
                                                           Log      = f.InLog ? f.Log : null
                                                       })
                                                       .ToListAsync();
            files.AddRange(recordFiles);
            return files;
        }
    }
}
