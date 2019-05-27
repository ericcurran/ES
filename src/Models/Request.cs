using System;

namespace Models
{
    public class Request
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public RequestTypeEnum Type { get; set; }
        public string ClaimNumber { get; set; }
        public string InsuredName { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfLoss { get; set; }
        public DateTime DateOfService { get; set; }
        public string Phase { get; set; }
        public string DeatilsFileName { get; set; }        
        public string PdfPackName { get; set; }
        public int? EsRef { get; set; }
        public RequestStatusEnum Status { get; set; }
        public bool Archived { get; set; }
        public bool Duplicated { get; set; }


        public string GetZipFileName() => $"{ClaimNumber}_{RequestId}.zip";
        
    }
}
