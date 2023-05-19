using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VrstaOpremeUslugeController : ControllerBase
    {
        private readonly IVrstaOpremeUslugeRepository _vrstaOpremeUslugeRepository;

        public VrstaOpremeUslugeController(IVrstaOpremeUslugeRepository repository)
        {
            _vrstaOpremeUslugeRepository = repository;
        }

        // GET: api/VrstaOpremeUsluge
        [HttpGet]
        public ActionResult<IEnumerable<VrstaOpremeUsluge>> GetAllVrstaOpremeUsluge()
        {
            var vrstaResult = _vrstaOpremeUslugeRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return vrstaResult
                ? Ok(vrstaResult.Data)
                : Problem(vrstaResult.Message, statusCode: 500);
        }

        // GET: api/VrstaOpremeUsluge/5
        [HttpGet("{id}")]
        public ActionResult<VrstaOpremeUsluge> GetVrstaOpremeUsluge(int id)
        {
            var vrstaResult = _vrstaOpremeUslugeRepository.Get(id).Map(DtoMapping.ToDto);

            return vrstaResult switch
            {
                { IsSuccess: true } => Ok(vrstaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(vrstaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/VrstaOpremeUsluge/5
        [HttpPut("{id}")]
        public IActionResult EditVrstaOpremeUsluge(int id, VrstaOpremeUsluge vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != vrsta.Id)
            {
                return BadRequest();
            }

            if (!_vrstaOpremeUslugeRepository.Exists(id))
            {
                return NotFound();
            }

            var domainVrsta = vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaOpremeUslugeRepository.Update(domainVrsta));

            return result
                ? AcceptedAtAction("EditVrstaOpreme", vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/VrstaOpremeUsluge
        [HttpPost]
        public ActionResult<VrstaOpremeUsluge> CreateVrstaOpremeUsluge(VrstaOpremeUsluge vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainVrsta = vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaOpremeUslugeRepository.Insert(domainVrsta));

            return result
                ? CreatedAtAction("GetVrstaOpreme", new { id = vrsta.Id }, vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/VrstaOpremeUsluge/5
        [HttpDelete("{id}")]
        public IActionResult DeleteVrstaOpremeUsluge(int id)
        {
            if (!_vrstaOpremeUslugeRepository.Exists(id))
                return NotFound();

            var deleteResult = _vrstaOpremeUslugeRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}