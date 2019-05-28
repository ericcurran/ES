using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordsManagmentWeb.Models
{
    public class UpdateRequestModel
    {
        public Request Request { get; set; }
        public bool UpdateEf { get; set; }
    }
}
