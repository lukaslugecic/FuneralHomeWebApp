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
    public class PogrebController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public PogrebController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Pogreb
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pogreb>>> GetPogreb()
        {
            return await _context.Pogreb.ToListAsync();
        }

        // GET: api/Pogreb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pogreb>> GetPogreb(int id)
        {
            var pogreb = await _context.Pogreb.FindAsync(id);

            if (pogreb == null)
            {
                return NotFound();
            }

            return pogreb;
        }

        // PUT: api/Pogreb/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPogreb(int id, Pogreb pogreb)
        {
            if (id != pogreb.Id)
            {
                return BadRequest();
            }

            _context.Entry(pogreb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PogrebExists(id))
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

        // POST: api/Pogreb
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pogreb>> CreatePogreb(Pogreb pogreb)
        {
            _context.Pogreb.Add(pogreb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPogreb", new { id = pogreb.Id }, pogreb);
        }

        // DELETE: api/Pogreb/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePogreb(int id)
        {
            var pogreb = await _context.Pogreb.FindAsync(id);
            if (pogreb == null)
            {
                return NotFound();
            }

            _context.Pogreb.Remove(pogreb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PogrebExists(int id)
        {
            return _context.Pogreb.Any(e => e.Id == id);
        }
    }
}
