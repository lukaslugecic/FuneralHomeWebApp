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
    public class OsiguranjeController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public OsiguranjeController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Osiguranje
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Osiguranje>>> GetOsiguranje()
        {
            return await _context.Osiguranje.ToListAsync();
        }

        // GET: api/Osiguranje/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Osiguranje>> GetOsiguranje(int id)
        {
            var osiguranje = await _context.Osiguranje.FindAsync(id);

            if (osiguranje == null)
            {
                return NotFound();
            }

            return osiguranje;
        }

        // PUT: api/Osiguranje/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditOsiguranje(int id, Osiguranje osiguranje)
        {
            if (id != osiguranje.Id)
            {
                return BadRequest();
            }

            _context.Entry(osiguranje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OsiguranjeExists(id))
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

        // POST: api/Osiguranje
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Osiguranje>> CreateOsiguranje(Osiguranje osiguranje)
        {
            _context.Osiguranje.Add(osiguranje);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OsiguranjeExists(osiguranje.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOsiguranje", new { id = osiguranje.Id }, osiguranje);
        }

        // DELETE: api/Osiguranje/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOsiguranje(int id)
        {
            var osiguranje = await _context.Osiguranje.FindAsync(id);
            if (osiguranje == null)
            {
                return NotFound();
            }

            _context.Osiguranje.Remove(osiguranje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OsiguranjeExists(int id)
        {
            return _context.Osiguranje.Any(e => e.Id == id);
        }
    }
}
