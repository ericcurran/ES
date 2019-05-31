using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecordsManagmentWeb.Services;

namespace RecordsManagmentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]

    public class PackController : ControllerBase
    {
        [HttpPost("{id}")]   
        public async Task<IActionResult> Post([FromRoute]int id, [FromServices] PdfGenearatorService pdfService )
        {
            CancellationToken t = new CancellationToken();
            string fileName = await pdfService.GeneratePdf(t, id);

            return Ok(fileName);
            
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestNodeService([FromServices] PdfGenearatorService pdfService)
        {
            CancellationToken t = new CancellationToken();
            string okResult = await pdfService.TestServcie(t);
            return Ok(okResult);            
        }

    }
}