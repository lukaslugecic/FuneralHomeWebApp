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
    public class LijesController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public LijesController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Lijes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lijes>>> GetLijes()
        {
            return await _context.Lijes.ToListAsync();
        }

        // GET: api/Lijes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lijes>> GetLijes(int id)
        {
            var lijes = await _context.Lijes.FindAsync(id);

            if (lijes == null)
            {
                return NotFound();
            }

            return lijes;
        }

        // PUT: api/Lijes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLijes(int id, Lijes lijes)
        {
            if (id != lijes.Id)
            {
                return BadRequest();
            }

            _context.Entry(lijes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LijesExists(id))
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

        // POST: api/Lijes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lijes>> CreateLijes(Lijes lijes)
        {
            _context.Lijes.Add(lijes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLijes", new { id = lijes.Id }, lijes);
        }

        // DELETE: api/Lijes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLijes(int id)
        {
            var lijes = await _context.Lijes.FindAsync(id);
            if (lijes == null)
            {
                return NotFound();
            }

            _context.Lijes.Remove(lijes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LijesExists(int id)
        {
            return _context.Lijes.Any(e => e.Id == id);
        }
    }
}
