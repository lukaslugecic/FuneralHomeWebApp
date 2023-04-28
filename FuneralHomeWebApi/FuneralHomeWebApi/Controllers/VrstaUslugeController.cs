using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VrstaUslugeController : ControllerBase
    {
        private readonly IVrstaUslugeRepository _vrstaUslugeRepository;

        public VrstaUslugeController(IVrstaUslugeRepository repository)
        {
            _vrstaUslugeRepository = repository;
        }

        // GET: api/VrstaUsluge
        [HttpGet]
        public ActionResult<IEnumerable<VrstaUsluge>> GetAllVrstaUsluge()
        {
            var vrstaResult = _vrstaUslugeRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return vrstaResult
                ? Ok(vrstaResult.Data)
                : Problem(vrstaResult.Message, statusCode: 500);
        }

        // GET: api/VrstaUsluge/5
        [HttpGet("{id}")]
        public ActionResult<VrstaUsluge> GetVrstaUsluge(int id)
        {
            var vrstaResult = _vrstaUslugeRepository.Get(id).Map(DtoMapping.ToDto);

            return vrstaResult switch
            {
                { IsSuccess: true } => Ok(vrstaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(vrstaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/VrstaUsluge/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditVrstaUsluge(int id, VrstaUsluge vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != vrsta.Id)
            {
                return BadRequest();
            }

            if (!_vrstaUslugeRepository.Exists(id))
            {
                return NotFound();
            }

            var domainVrsta= vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaUslugeRepository.Update(domainVrsta));

            return result
                ? AcceptedAtAction("EditVrstaUsluge", vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/VrstaUsluge
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<VrstaUsluge> CreateVrstaUsluge(VrstaUsluge vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainVrsta = vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaUslugeRepository.Insert(domainVrsta));

            return result
                ? CreatedAtAction("GetVrstaUsluge", new { id = vrsta.Id }, vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/VrstaUsluge/5
        [HttpDelete("{id}")]
        public IActionResult DeleteVrstaUsluge(int id)
        {
            if (!_vrstaUslugeRepository.Exists(id))
                return NotFound();

            var deleteResult = _vrstaUslugeRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}