using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsiguranjeController : ControllerBase
    {
        private readonly IOsiguranjeRepository _osiguranjeRepository;

        public OsiguranjeController(IOsiguranjeRepository repository)
        {
            _osiguranjeRepository = repository;
        }

        // GET: api/Osiguranje
        [HttpGet]
        public ActionResult<IEnumerable<Osiguranje>> GetAllOsiguranje()
        {
            var osiguranjeResult = _osiguranjeRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return osiguranjeResult
                ? Ok(osiguranjeResult.Data)
                : Problem(osiguranjeResult.Message, statusCode: 500);
        }

        // GET: api/Osiguranje/5
        [HttpGet("{id}")]
        public ActionResult<Osiguranje> GetOsiguranje(int id)
        {
            var osiguranjeResult = _osiguranjeRepository.Get(id).Map(DtoMapping.ToDto);

            return osiguranjeResult switch
            {
                { IsSuccess: true } => Ok(osiguranjeResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(osiguranjeResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Osiguranje/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditOsiguranje(int id, Osiguranje osiguranje)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != osiguranje.Id)
            {
                return BadRequest();
            }

            if (!_osiguranjeRepository.Exists(id))
            {
                return NotFound();
            }

            var domainOsiguranje = osiguranje.ToDomain();

            var result =
                domainOsiguranje.IsValid()
                .Bind(() => _osiguranjeRepository.Update(domainOsiguranje));

            return result
                ? AcceptedAtAction("EditOsiguranje", osiguranje)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/Osiguranje
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Osiguranje> CreateOsiguranje(Osiguranje osiguranje)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainOsiguranje = osiguranje.ToDomain();

            var result =
                domainOsiguranje.IsValid()
                .Bind(() => _osiguranjeRepository.Insert(domainOsiguranje));

            return result
                ? CreatedAtAction("GetOsiguranje", new { id = osiguranje.Id }, osiguranje)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/Osiguranje/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOsiguranje(int id)
        {
            if (!_osiguranjeRepository.Exists(id))
                return NotFound();

            var deleteResult = _osiguranjeRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}