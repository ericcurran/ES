using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using DbService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using RecordsManagmentWeb.Services;

namespace RecordsManagmentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackController : ControllerBase
    {
        [HttpPost("{id}")]        
        public async Task<IActionResult> Post([FromRoute]int id, [FromServices] PdfGenearatorService pdfService )
        {
            CancellationToken t = new CancellationToken();
            await pdfService.GeneratePdf(t, id);

            return Ok();
            
        }
    }
}