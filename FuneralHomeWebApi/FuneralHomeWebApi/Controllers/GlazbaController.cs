using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlazbaController : ControllerBase
    {
        private readonly IGlazbaRepository _glazbaRepository;

        public GlazbaController(IGlazbaRepository repository)
        {
            _glazbaRepository = repository;
        }

        // GET: api/Glazba
        [HttpGet]
        public ActionResult<IEnumerable<Glazba>> GetAllGlazba()
        {
            var glazbaResult = _glazbaRepository.GetAll()
                .Map(g => g.Select(DtoMapping.ToDto));

            return glazbaResult
                ? Ok(glazbaResult.Data)
                : Problem(glazbaResult.Message, statusCode: 500);
        }

        // GET: api/Glazba/5
        [HttpGet("{id}")]
        public ActionResult<Glazba> GetGlazba(int id)
        {
            var glazbaResult = _glazbaRepository.Get(id).Map(DtoMapping.ToDto);

            return glazbaResult switch
            {
                { IsSuccess: true } => Ok(glazbaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(glazbaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Glazba/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditGlazba(int id, Glazba glazba)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != glazba.Id)
            {
                return BadRequest();
            }

            if (!_glazbaRepository.Exists(id))
            {
                return NotFound();
            }

            var domainGlazba = glazba.ToDomain();

            var result =
                domainGlazba.IsValid()
                .Bind(() => _glazbaRepository.Update(domainGlazba));

            return result
                ? AcceptedAtAction("EditGlazba", glazba)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/Glazba
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Glazba> CreateGlazba(Glazba glazba)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainGlazba = glazba.ToDomain();

            var result =
                domainGlazba.IsValid()
                .Bind(() => _glazbaRepository.Insert(domainGlazba));

            return result
                ? CreatedAtAction("GetGlazba", new { id = glazba.Id }, glazba)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/Glazba/5
        [HttpDelete("{id}")]
        public IActionResult DeleteGlazba(int id)
        {
            if (!_glazbaRepository.Exists(id))
                return NotFound();

            var deleteResult = _glazbaRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}