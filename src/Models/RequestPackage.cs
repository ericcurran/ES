using System;

namespace Models
{
    public class RequestPackage
    {
        public int Id { get; set; }
        public string ZipFileName { get; set; }
        public string DeatilsFileName { get; set; }
        public int? DetailsRecordId { get; set; }
        public RequestStatusEnum Status { get; set; }

    }
}
