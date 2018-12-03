using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace FtpService
{
    public class ZipServcie
    {
        public string[] UnzipFile(Stream stream)
        {
           var zipArchive =  new ZipArchive(stream);
           
            foreach (var entry in zipArchive.Entries)
            {
                var entryStream = entry.Open();
            }
            return zipArchive.Entries.Select(e => e.FullName).ToArray();
        }
    }
}
