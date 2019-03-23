using System.Collections.Generic;

namespace RecordsManagmentWeb.Services
{
    public class PdfJsonData
    {
        public IEnumerable<string> LogTitle { get; set; }

        public IEnumerable<PdfItemData> LogItems { get; set; }
    }
}