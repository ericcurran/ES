using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService
{
    public static class DataService
    {
        private static readonly string _connectionString =  Environment.GetEnvironmentVariable("ConnectionStrings:RequestManagmentDb");

        public static async Task<IEnumerable<RequestPackage>> SaveNewRequests(IEnumerable<string> fileNames)
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

        public static async Task SaveNewRecordAndUpdaterequest(RecordFile record)
        {
            using (var db = GetDbContext())
            {
                db.RecordFiles.Add(record);
                await db.SaveChangesAsync();
                if (record.FileName.EndsWith(".pdf"))
                {
                    var request = await db.RequestPackages.FindAsync(record.RequestPackageId);
                    request.DeatilsFileName = record.FileName;
                    request.DetailsRecordId = record.RequestPackageId;
                    await db.SaveChangesAsync();
                }
            }
        }

        public static async Task UpdateRequestPackagetoStatus(int requestId, RequestStatusEnum status)
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

        private static ApplicationDbContext GetDbContext()
        {
            var opt = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseSqlServer(_connectionString)
                         .Options;
            return new ApplicationDbContext(opt);
        }
    }
}
