using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class RecordFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public RecordStatusEnum Status { get; set; }
        public bool InScope { get; set; }
        public int RequestPackageId { get; set; }
        public int? EsRef { get; set; }

        public virtual RequestPackage RequestPackage { get; set; }        
    }
}
