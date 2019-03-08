using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class RecordFile
    {
        public int Id { get; set; }
        public int RequestPackageId { get; set; }
        public string FileName { get; set; }
        public RecordStatusEnum Status { get; set; }
        public bool InScope { get; set; }
        public bool InLog { get; set; }
        public int? EsRef { get; set; }
        public string ClaimNumber { get; set; }
        public string BundleNumber { get; set; }
        public int PageNumber { get; set; }
        public int OrderNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string Log { get; set; }
        public string Phase { get; set; }

        public virtual RequestPackage RequestPackage { get; set; }        
    }
}
