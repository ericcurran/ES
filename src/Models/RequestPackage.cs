using System;

namespace Models
{
    public class RequestPackage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public RecordStatusEnum Status { get; set; }

    }
}
