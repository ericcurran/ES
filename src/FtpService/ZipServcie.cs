using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace FtpService
{
    public static class ZipServcie
    {
        public static IEnumerable<ZipArchiveEntry> UnzipFile(Stream stream)
        {
            var zipArchive = new ZipArchive(stream);

            foreach (var entry in zipArchive.Entries)
            {
                yield return entry;
            }
        }
    }
}
