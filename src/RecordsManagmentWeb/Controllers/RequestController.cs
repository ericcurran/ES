using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbService;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace RecordsManagmentWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestPackages()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Request/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequestPackage(int id)
        {
            var requestPackage = await _context.Requests.FindAsync(id);

            if (requestPackage == null)
            {
                return NotFound();
            }

            return requestPackage;
        }

        // PUT: api/Request/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestPackage(int id, Request requestPackage)
        {
            if (id != requestPackage.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestPackageExists(id))
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

        // POST: api/Request
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequestPackage(Request requestPackage)
        {
            _context.Requests.Add(requestPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestPackage", new { id = requestPackage.Id }, requestPackage);
        }

        // DELETE: api/Request/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequestPackage(int id)
        {
            var requestPackage = await _context.Requests.FindAsync(id);
            if (requestPackage == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(requestPackage);
            await _context.SaveChangesAsync();

            return requestPackage;
        }

        private bool RequestPackageExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
