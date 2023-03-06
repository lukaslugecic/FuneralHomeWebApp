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
    public class OglasController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public OglasController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Oglas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Oglas>>> GetOglas()
        {
            return await _context.Oglas.ToListAsync();
        }

        // GET: api/Oglas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Oglas>> GetOglas(int id)
        {
            var oglas = await _context.Oglas.FindAsync(id);

            if (oglas == null)
            {
                return NotFound();
            }

            return oglas;
        }

        // PUT: api/Oglas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditOglas(int id, Oglas oglas)
        {
            if (id != oglas.Id)
            {
                return BadRequest();
            }

            _context.Entry(oglas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OglasExists(id))
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

        // POST: api/Oglas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Oglas>> CreateOglas(Oglas oglas)
        {
            _context.Oglas.Add(oglas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOglas", new { id = oglas.Id }, oglas);
        }

        // DELETE: api/Oglas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOglas(int id)
        {
            var oglas = await _context.Oglas.FindAsync(id);
            if (oglas == null)
            {
                return NotFound();
            }

            _context.Oglas.Remove(oglas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OglasExists(int id)
        {
            return _context.Oglas.Any(e => e.Id == id);
        }
    }
}
