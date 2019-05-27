using System.IO;

namespace Models
{
    public class ZipEntryModel
    {
        public string FileName { get; set; }
        public Stream FileStrem { get; set; }        
    }
}