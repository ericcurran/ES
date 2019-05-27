using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DataService> _log;

        public DataService(string connectionString, ILogger<DataService> logger)
        {
            _connectionString = connectionString;
            _log = logger;
        }

        public async Task<IEnumerable<Request>> SaveNewRequests(IEnumerable<Request> requests)
        {
            try
            {
                using (var db = GetDbContext())
                {
                    await db.Requests.AddRangeAsync(requests);
                    await db.SaveChangesAsync();
                }

                return requests;
            }
            catch (Exception e)
            {
                _log.LogError("Error during saving new zip file list.");
                _log.LogError(e.Message);
                throw e;
            }
        }

        public async Task SaveNewRecordAndUpdaterequest(IEnumerable<Record> records)
        {
            try
            {
                var detailsFile = records.FirstOrDefault(r=> r.FileName.EndsWith("_1-0.pdf"));
                var recordsList = records.ToList();
                var removed = recordsList.Remove(detailsFile);
                using (var db = GetDbContext())
                {
                    await db.Records.AddRangeAsync(recordsList);
                    var request = await db.Requests.FindAsync(detailsFile.RequestId);
                    request.DeatilsFileName = detailsFile.FileName;                            
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _log.LogError("Error during saving record files.");
                _log.LogError(e.Message);
                throw e;
            }
        }

        public async Task<IEnumerable<string>> GetFileNamesByClaimNumber(string claimNumber)
        {
            using (var db = GetDbContext())
            {
                return await db.Records.AsNoTracking()
                    .Where(r => r.ClaimNumber == claimNumber)
                    .Select(r => r.FileName)
                    .ToListAsync();

            }
        }

        public async Task UpdateRequestPackagetoStatus(int requestId, RequestStatusEnum status)
        {
            using (var db = GetDbContext())
            {
                var request = await db.Requests.FindAsync(requestId);
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
