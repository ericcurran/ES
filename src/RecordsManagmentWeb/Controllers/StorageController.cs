using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageService;

namespace RecordsManagmentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StorageController : ControllerBase
    {
        private readonly BlobStorageService _blob;

        public StorageController(BlobStorageService blob)
        {
            _blob = blob;
        }

        [Route("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var fileStream = await _blob.DownloadFile(id);
            fileStream.Position = 0;
            string contentHeader = GetContentHeaderFromFileName(id);
            byte[] blob = new byte[fileStream.Length];
            fileStream.Read(blob, 0, (int)fileStream.Length);
            return new FileContentResult(blob, contentHeader);
        }

        private string GetContentHeaderFromFileName(string f)
        {
            var ext = f?.Split('.');
            if (ext != null && ext.Length > 1)
            {
                switch (ext[ext.Length - 1])
                {
                    case "pdf":
                        return "application/pdf";

                    case "tiff":
                    case "ttf":
                        return "image/tiff";

                    case "png":
                        return "image/png";

                    case "jpg":
                    case "jpeg":
                        return "image/jpeg";
                }
            }
            return "application/octet-stream";
        }
    }
}