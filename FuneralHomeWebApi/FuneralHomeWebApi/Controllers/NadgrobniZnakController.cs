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
    public class NadgrobniZnakController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public NadgrobniZnakController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/NadgrobniZnak
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NadgrobniZnak>>> GetNadgrobniZnak()
        {
            return await _context.NadgrobniZnak.ToListAsync();
        }

        // GET: api/NadgrobniZnak/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NadgrobniZnak>> GetNadgrobniZnak(int id)
        {
            var nadgrobniZnak = await _context.NadgrobniZnak.FindAsync(id);

            if (nadgrobniZnak == null)
            {
                return NotFound();
            }

            return nadgrobniZnak;
        }

        // PUT: api/NadgrobniZnak/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditNadgrobniZnak(int id, NadgrobniZnak nadgrobniZnak)
        {
            if (id != nadgrobniZnak.Id)
            {
                return BadRequest();
            }

            _context.Entry(nadgrobniZnak).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NadgrobniZnakExists(id))
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

        // POST: api/NadgrobniZnak
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NadgrobniZnak>> CreateNadgrobniZnak(NadgrobniZnak nadgrobniZnak)
        {
            _context.NadgrobniZnak.Add(nadgrobniZnak);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNadgrobniZnak", new { id = nadgrobniZnak.Id }, nadgrobniZnak);
        }

        // DELETE: api/NadgrobniZnak/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNadgrobniZnak(int id)
        {
            var nadgrobniZnak = await _context.NadgrobniZnak.FindAsync(id);
            if (nadgrobniZnak == null)
            {
                return NotFound();
            }

            _context.NadgrobniZnak.Remove(nadgrobniZnak);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NadgrobniZnakExists(int id)
        {
            return _context.NadgrobniZnak.Any(e => e.Id == id);
        }
    }
}
