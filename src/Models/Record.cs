using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Record
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
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
        public bool Archived { get; set; }
        public bool Duplicated { get; set; }

        public virtual Request Request { get; set; }
        
    }
}
