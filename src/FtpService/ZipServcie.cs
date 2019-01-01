using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpService
{
    public static class ZipServcie
    {
        public static async Task<IEnumerable<ZipEntryModel>> UnzipFile(Stream stream)
        {
            var zipEntries = new List<ZipEntryModel>();
            var zipArchive = new ZipArchive(stream);

            foreach (var entry in zipArchive.Entries)
            {
                var m = new MemoryStream();
                var z = entry.Open();
                await z.CopyToAsync(m);
                z.Close();
                m.Position = 0;
                var f = new ZipEntryModel
                {
                    FileName = entry.Name,
                    FileStrem = m
                };
                zipEntries.Add(f);
            }
            return zipEntries;
        }
    }
}
