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
    public class GlazbaController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public GlazbaController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Glazba
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Glazba>>> GetGlazba()
        {
            return await _context.Glazba.ToListAsync();
        }

        // GET: api/Glazba/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Glazba>> GetGlazba(int id)
        {
            var glazba = await _context.Glazba.FindAsync(id);

            if (glazba == null)
            {
                return NotFound();
            }

            return glazba;
        }

        // PUT: api/Glazba/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditGlazba(int id, Glazba glazba)
        {
            if (id != glazba.Id)
            {
                return BadRequest();
            }

            _context.Entry(glazba).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlazbaExists(id))
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

        // POST: api/Glazba
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Glazba>> CreateGlazba(Glazba glazba)
        {
            _context.Glazba.Add(glazba);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGlazba", new { id = glazba.Id }, glazba);
        }

        // DELETE: api/Glazba/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGlazba(int id)
        {
            var glazba = await _context.Glazba.FindAsync(id);
            if (glazba == null)
            {
                return NotFound();
            }

            _context.Glazba.Remove(glazba);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GlazbaExists(int id)
        {
            return _context.Glazba.Any(e => e.Id == id);
        }
    }
}
