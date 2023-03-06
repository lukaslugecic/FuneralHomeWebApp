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
    public class CvijeceController : ControllerBase
    {
        private readonly FuneralHomeContext _context;

        public CvijeceController(FuneralHomeContext context)
        {
            _context = context;
        }

        // GET: api/Cvijece
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cvijece>>> GetCvijece()
        {
            return await _context.Cvijece.ToListAsync();
        }

        // GET: api/Cvijece/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cvijece>> GetCvijece(int id)
        {
            var cvijece = await _context.Cvijece.FindAsync(id);

            if (cvijece == null)
            {
                return NotFound();
            }

            return cvijece;
        }

        // PUT: api/Cvijece/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCvijece(int id, Cvijece cvijece)
        {
            if (id != cvijece.Id)
            {
                return BadRequest();
            }

            _context.Entry(cvijece).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CvijeceExists(id))
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

        // POST: api/Cvijece
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cvijece>> CreateCvijece(Cvijece cvijece)
        {
            _context.Cvijece.Add(cvijece);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCvijece", new { id = cvijece.Id }, cvijece);
        }

        // DELETE: api/Cvijece/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCvijece(int id)
        {
            var cvijece = await _context.Cvijece.FindAsync(id);
            if (cvijece == null)
            {
                return NotFound();
            }

            _context.Cvijece.Remove(cvijece);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CvijeceExists(int id)
        {
            return _context.Cvijece.Any(e => e.Id == id);
        }
    }
}
