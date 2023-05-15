using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaketOsiguranjaController : ControllerBase
    {
        private readonly IPaketOsiguranjaRepository _paketOsiguranjaRepository;

        public PaketOsiguranjaController(IPaketOsiguranjaRepository repository)
        {
            _paketOsiguranjaRepository = repository;
        }

        // GET: api/PaketOsiguranja
        [HttpGet]
        public ActionResult<IEnumerable<PaketOsiguranja>> GetAllPaketOsiguranja()
        {
            var paketResult = _paketOsiguranjaRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return paketResult
                ? Ok(paketResult.Data)
                : Problem(paketResult.Message, statusCode: 500);
        }

        // GET: api/PaketOsiguranja/5
        [HttpGet("{id}")]
        public ActionResult<PaketOsiguranja> GetPaketOsiguranja(int id)
        {
            var paketResult = _paketOsiguranjaRepository.Get(id).Map(DtoMapping.ToDto);

            return paketResult switch
            {
                { IsSuccess: true } => Ok(paketResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(paketResult.Message, statusCode: 500)
            };
        }

        // PUT: api/PaketOsiguranja/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditPaketOsiguranja(int id, PaketOsiguranja paket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != paket.Id)
            {
                return BadRequest();
            }

            if (!_paketOsiguranjaRepository.Exists(id))
            {
                return NotFound();
            }

            var domainPaket = paket.ToDomain();

            var result =
                domainPaket.IsValid()
                .Bind(() => _paketOsiguranjaRepository.Update(domainPaket));

            return result
                ? AcceptedAtAction("EditPaketOsiguranja", paket)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/PaketOsiguranja
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<PaketOsiguranja> CreatePaketOsiguranja(PaketOsiguranja paket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainPaket = paket.ToDomain();

            var result =
                domainPaket.IsValid()
                .Bind(() => _paketOsiguranjaRepository.Insert(domainPaket));

            return result
                ? CreatedAtAction("GetPaketOsiguranja", new { id = paket.Id }, paket)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/PaketOsiguranja/5
        [HttpDelete("{id}")]
        public IActionResult DeletePaketOsiguranja(int id)
        {
            if (!_paketOsiguranjaRepository.Exists(id))
                return NotFound();

            var deleteResult = _paketOsiguranjaRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}