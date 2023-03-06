using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuneralHomeWebApi.Data.DbModels;

namespace FuneralHomeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsmrtnicaController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public OsmrtnicaController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Osmrtnica
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Osmrtnica>>> GetOsmrtnica()
        {
            return await _context.Osmrtnica.ToListAsync();
        }

        // GET: api/Osmrtnica/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Osmrtnica>> GetOsmrtnica(int id)
        {
            var osmrtnica = await _context.Osmrtnica.FindAsync(id);

            if (osmrtnica == null)
            {
                return NotFound();
            }

            return osmrtnica;
        }

        // PUT: api/Osmrtnica/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditOsmrtnica(int id, Osmrtnica osmrtnica)
        {
            if (id != osmrtnica.Id)
            {
                return BadRequest();
            }

            _context.Entry(osmrtnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OsmrtnicaExists(id))
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

        // POST: api/Osmrtnica
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Osmrtnica>> CreateOsmrtnica(Osmrtnica osmrtnica)
        {
            _context.Osmrtnica.Add(osmrtnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOsmrtnica", new { id = osmrtnica.Id }, osmrtnica);
        }

        // DELETE: api/Osmrtnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOsmrtnica(int id)
        {
            var osmrtnica = await _context.Osmrtnica.FindAsync(id);
            if (osmrtnica == null)
            {
                return NotFound();
            }

            _context.Osmrtnica.Remove(osmrtnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OsmrtnicaExists(int id)
        {
            return _context.Osmrtnica.Any(e => e.Id == id);
        }
    }
}
