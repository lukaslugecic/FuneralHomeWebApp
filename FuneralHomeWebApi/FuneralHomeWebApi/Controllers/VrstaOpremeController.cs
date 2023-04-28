using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VrstaOpremeController : ControllerBase
    {
        private readonly IVrstaOpremeRepository _vrstaOpremeRepository;

        public VrstaOpremeController(IVrstaOpremeRepository repository)
        {
            _vrstaOpremeRepository = repository;
        }

        // GET: api/VrstaOpreme
        [HttpGet]
        public ActionResult<IEnumerable<VrstaOpreme>> GetAllVrstaOpreme()
        {
            var vrstaResult = _vrstaOpremeRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return vrstaResult
                ? Ok(vrstaResult.Data)
                : Problem(vrstaResult.Message, statusCode: 500);
        }

        // GET: api/VrstaOpreme/5
        [HttpGet("{id}")]
        public ActionResult<VrstaOpreme> GetVrstaOpreme(int id)
        {
            var vrstaResult = _vrstaOpremeRepository.Get(id).Map(DtoMapping.ToDto);

            return vrstaResult switch
            {
                { IsSuccess: true } => Ok(vrstaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(vrstaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/VrstaOpreme/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditVrstaOpreme(int id, VrstaOpreme vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != vrsta.Id)
            {
                return BadRequest();
            }

            if (!_vrstaOpremeRepository.Exists(id))
            {
                return NotFound();
            }

            var domainVrsta = vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaOpremeRepository.Update(domainVrsta));

            return result
                ? AcceptedAtAction("EditVrstaOpreme", vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/VrstaOpreme
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<VrstaOpreme> CreateVrstaOpreme(VrstaOpreme vrsta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainVrsta = vrsta.ToDomain();

            var result =
                domainVrsta.IsValid()
                .Bind(() => _vrstaOpremeRepository.Insert(domainVrsta));

            return result
                ? CreatedAtAction("GetVrstaOpreme", new { id = vrsta.Id }, vrsta)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/VrstaOpreme/5
        [HttpDelete("{id}")]
        public IActionResult DeleteVrstaOpreme(int id)
        {
            if (!_vrstaOpremeRepository.Exists(id))
                return NotFound();

            var deleteResult = _vrstaOpremeRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}