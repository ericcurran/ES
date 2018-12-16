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
    }
}
