using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrnaController : ControllerBase
    {
        private readonly IUrnaRepository _urnaRepository;

        public UrnaController(IUrnaRepository repository)
        {
            _urnaRepository = repository;
        }

        // GET: api/Urna
        [HttpGet]
        public ActionResult<IEnumerable<Urna>> GetAllUrna()
        {
            var urnaResult = _urnaRepository.GetAll()
                .Map(u => u.Select(DtoMapping.ToDto));

            return urnaResult
                ? Ok(urnaResult.Data)
                : Problem(urnaResult.Message, statusCode: 500);
        }

        // GET: api/Urna/5
        [HttpGet("{id}")]
        public ActionResult<Urna> GetUrna(int id)
        {
            var urnaResult = _urnaRepository.Get(id).Map(DtoMapping.ToDto);

            return urnaResult switch
            {
                { IsSuccess: true } => Ok(urnaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(urnaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Urna/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditUrna(int id, Urna urna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != urna.Id)
            {
                return BadRequest();
            }

            if (!_urnaRepository.Exists(id))
            {
                return NotFound();
            }

            var domainUrna = urna.ToDomain();

            var result =
                domainUrna.IsValid()
                .Bind(() => _urnaRepository.Update(domainUrna));

            return result
                ? AcceptedAtAction("EditUrna", urna)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/Urna
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Urna> CreateUrna(Urna urna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainUrna = urna.ToDomain();

            var result =
                domainUrna.IsValid()
                .Bind(() => _urnaRepository.Insert(domainUrna));

            return result
                ? CreatedAtAction("GetUrna", new { id = urna.Id }, urna)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/Unra/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUrna(int id)
        {
            if (!_urnaRepository.Exists(id))
                return NotFound();

            var deleteResult = _urnaRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}