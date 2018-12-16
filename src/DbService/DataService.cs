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
        private readonly ApplicationDbContext _db;

        public DataService(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<RequestPackage>> SaveNewRequest(IEnumerable<string> fileNames)
        {
            var requests = fileNames.Select(fileName => new RequestPackage()
            {
                ZipFileName = fileName,
                Status = RequestStatusEnum.New
            }).ToList();
            foreach (var r in requests)
            {
                var e = _db.RequestPackages.Add(r);
                await _db.SaveChangesAsync();
                r.Id = e.Entity.Id;
            }

            return requests;
        }

        public async Task SaveNewRecord(RecordFile record)
        {
            _db.RecordFiles.Add(record);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRequestPackage(RequestPackage request)
        {
            _db.RequestPackages.Update(request);
            await _db.SaveChangesAsync();
        }
    }
}
