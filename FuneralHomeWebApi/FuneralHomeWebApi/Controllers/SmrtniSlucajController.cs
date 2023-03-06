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
    public class SmrtniSlucajController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public SmrtniSlucajController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/SmrtniSlucaj
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmrtniSlucaj>>> GetSmrtniSlucaj()
        {
            return await _context.SmrtniSlucaj.ToListAsync();
        }

        // GET: api/SmrtniSlucaj/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SmrtniSlucaj>> GetSmrtniSlucaj(int id)
        {
            var smrtniSlucaj = await _context.SmrtniSlucaj.FindAsync(id);

            if (smrtniSlucaj == null)
            {
                return NotFound();
            }

            return smrtniSlucaj;
        }

        // PUT: api/SmrtniSlucaj/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSmrtniSlucaj(int id, SmrtniSlucaj smrtniSlucaj)
        {
            if (id != smrtniSlucaj.Id)
            {
                return BadRequest();
            }

            _context.Entry(smrtniSlucaj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmrtniSlucajExists(id))
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

        // POST: api/SmrtniSlucaj
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SmrtniSlucaj>> CreateSmrtniSlucaj(SmrtniSlucaj smrtniSlucaj)
        {
            _context.SmrtniSlucaj.Add(smrtniSlucaj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSmrtniSlucaj", new { id = smrtniSlucaj.Id }, smrtniSlucaj);
        }

        // DELETE: api/SmrtniSlucaj/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSmrtniSlucaj(int id)
        {
            var smrtniSlucaj = await _context.SmrtniSlucaj.FindAsync(id);
            if (smrtniSlucaj == null)
            {
                return NotFound();
            }

            _context.SmrtniSlucaj.Remove(smrtniSlucaj);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SmrtniSlucajExists(int id)
        {
            return _context.SmrtniSlucaj.Any(e => e.Id == id);
        }
    }
}
