using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService
{
    public class DataService
    {
        private readonly string _connectionString;

        public DataService(string connectionString) => _connectionString = connectionString;


        public async Task<IEnumerable<RequestPackage>> SaveNewRequests(IEnumerable<string> fileNames)
        {
            var requests = fileNames.Select(fileName => new RequestPackage()
            {
                ZipFileName = fileName,
                Status = RequestStatusEnum.New
            }).ToList();

            using (var db = GetDbContext())
            {
                foreach (var r in requests)
                {
                    var e = db.RequestPackages.Add(r);
                    await db.SaveChangesAsync();                    
                }
            }

            return requests;
        }

        public async Task SaveNewRecordAndUpdaterequest(IEnumerable<RecordFile> records)
        {
            using (var db = GetDbContext())
            {
                db.RecordFiles.AddRange(records);
                await db.SaveChangesAsync();
                foreach (var record in records)
                {
                    if (record.FileName.EndsWith(".pdf"))
                    {
                        var request = await db.RequestPackages.FindAsync(record.RequestPackageId);
                        request.DeatilsFileName = record.FileName;
                        request.DetailsRecordId = record.RequestPackageId;
                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task UpdateRequestPackagetoStatus(int requestId, RequestStatusEnum status)
        {
            using (var db = GetDbContext())
            {
                var request = await db.RequestPackages.FindAsync(requestId);
                if (request != null)
                {
                    request.Status = status;
                    await db.SaveChangesAsync();
                }
            }
        }

        private ApplicationDbContext GetDbContext()
        {
            var opt = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseSqlServer(_connectionString)
                         .Options;
            return new ApplicationDbContext(opt);
        }
    }
}
