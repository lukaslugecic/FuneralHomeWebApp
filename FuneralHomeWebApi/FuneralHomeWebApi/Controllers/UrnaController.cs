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
    public class UrnaController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public UrnaController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Urna
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urna>>> GetUrna()
        {
            return await _context.Urna.ToListAsync();
        }

        // GET: api/Urna/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urna>> GetUrna(int id)
        {
            var urna = await _context.Urna.FindAsync(id);

            if (urna == null)
            {
                return NotFound();
            }

            return urna;
        }

        // PUT: api/Urna/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUrna(int id, Urna urna)
        {
            if (id != urna.Id)
            {
                return BadRequest();
            }

            _context.Entry(urna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrnaExists(id))
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

        // POST: api/Urna
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Urna>> CreateUrna(Urna urna)
        {
            _context.Urna.Add(urna);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrna", new { id = urna.Id }, urna);
        }

        // DELETE: api/Urna/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrna(int id)
        {
            var urna = await _context.Urna.FindAsync(id);
            if (urna == null)
            {
                return NotFound();
            }

            _context.Urna.Remove(urna);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrnaExists(int id)
        {
            return _context.Urna.Any(e => e.Id == id);
        }
    }
}
