using DbService;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly string _tempRecordPath;
        private readonly string _nodePath;

        public PdfGenearatorService(INodeServices node, ApplicationDbContext db, BlobStorageService blobService, IOptions<NodeOptions> nodeOPtions)
        {
            _node = node;
            _db = db;
            _blobService = blobService;
            _tempRecordPath = nodeOPtions.Value.RecordsTempPath;
            _nodePath = nodeOPtions.Value.NodeAppFile;
        }
        public async Task GeneratePdf(CancellationToken stoppingToken, int id)  
        {
            var tempDir = $"{_tempRecordPath}\\{Guid.NewGuid().ToString()}";
            IEnumerable<string> savedFiles = await DownloadPackFiles(id, tempDir);
            var fileName = await _node.InvokeExportAsync<string>(_nodePath, "callFromAsp", tempDir);
            await SavePdfReport(fileName, tempDir, id);            
            DeletePackFiles(tempDir);
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

        private async Task<IEnumerable<string>> DownloadPackFiles(int id, string tempDir)
        {
            var files = await GetFileNames(id);
            if (files == null)
            {
                return null;
            }
            await DownloadFileToLocalFolder(files, tempDir);

            return files;
        }

        private async Task DownloadFileToLocalFolder(IEnumerable<string> files, string tempDir)
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
            Directory.CreateDirectory(tempDir);

            var savedTasks = new Dictionary<string,Task<Stream>>();
            foreach (var file in files)
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
        }

        private async  Task SaveRecordOnDisk(string fileName, Stream result, string tempDir)
        {
            using (var fileStream = new FileStream($"{tempDir}\\{fileName}", FileMode.CreateNew))
            {
                result.Position = 0;
                await result.CopyToAsync(fileStream);       
            }
        }

        private async Task<IEnumerable<string>> GetFileNames(int id)
        {
            Directory.Exists(_tempRecordPath);
            var requestPack = await _db.RequestPackages.AsNoTracking().FirstOrDefaultAsync(rp => rp.Id == id);
            if (requestPack.DeatilsFileName == null)
            {
                return null;
            }
            var files = new List<string>(new[] { requestPack.DeatilsFileName });
            IEnumerable<string> recordFiles = await _db.RecordFiles
                                                       .Where(f => f.RequestPackageId == id && f.InScope 
                                                                && f.Id!=requestPack.DetailsRecordId)
                                                       .Select(f => f.FileName)
                                                       .ToListAsync();
            files.AddRange(recordFiles);
            return files;
        }
    }
}
