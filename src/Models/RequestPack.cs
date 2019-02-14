using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class PdfPack
    {
        public  int Id { get; set; }
        public string FileName { get; set; }
        public PackStatus Status { get; set; }
        public virtual RequestPackage RequestPackage { get; set; }
    }
}
