using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbService;
using Models;

namespace RecordsManagmentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Record
        [HttpGet]
        public async Task<ActionResult> GetRecordFiles(int requestId, 
            string sortDir, int page, int size)
        {
            var data = _context.Records.AsNoTracking();
            if (requestId > 0)
            {
                data = data.Where(r => r.RequestId == requestId);
            }
            int count = await data.CountAsync();
            switch (sortDir)
            {
                case "asc":
                    data = data.OrderBy(r => r.FileName);
                    break;
                case "desc":
                    data = data.OrderByDescending(r => r.FileName);
                    break;
                default:
                    break;
            }

            return Ok(new
            {
                Data = await data.Skip(page * size).Take(size).ToListAsync(),
                Count = count
            });
        }

        // GET: api/Record/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecordFile(int id)
        {
            var recordFile = await _context.Records.FindAsync(id);

            if (recordFile == null)
            {
                return NotFound();
            }

            return recordFile;
        }

        // PUT: api/Record/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecordFile(int id, Record recordFile)
        {
            if (id != recordFile.Id)
            {
                return BadRequest();
            }

            _context.Entry(recordFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordFileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Record
        [HttpPost]
        public async Task<ActionResult<Record>> PostRecordFile(Record recordFile)
        {
            _context.Records.Add(recordFile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecordFile", new { id = recordFile.Id }, recordFile);
        }

        // DELETE: api/Record/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Record>> DeleteRecordFile(int id)
        {
            var recordFile = await _context.Records.FindAsync(id);
            if (recordFile == null)
            {
                return NotFound();
            }

            _context.Records.Remove(recordFile);
            await _context.SaveChangesAsync();

            return recordFile;
        }

        private bool RecordFileExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
