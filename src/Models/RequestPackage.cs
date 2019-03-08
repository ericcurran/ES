using System;

namespace Models
{
    public class RequestPackage
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; }
        public string RequestId { get; set; }
        public string InsuredName { get; set; }
        public DateTime DateOfLoss { get; set; }
        public DateTime DateOfService { get; set; }
        public string Phase { get; set; }
        public string ZipFileName { get; set; }
        public string DeatilsFileName { get; set; }
        public int? DetailsRecordId { get; set; }
        public string PdfPackName { get; set; }
        public int? EsRef { get; set; }
        public RequestStatusEnum Status { get; set; }
        public virtual PdfPack PdfPack { get; set; }
    }
}
