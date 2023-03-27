using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NadgrobniZnakController : ControllerBase
    {
        private readonly INadgrobniZnakRepository _nadgrobniZnakRepository;

        public NadgrobniZnakController(INadgrobniZnakRepository repository)
        {
            _nadgrobniZnakRepository = repository;
        }

        // GET: api/NadgrobniZnak
        [HttpGet]
        public ActionResult<IEnumerable<NadgrobniZnak>> GetAllNadgrobniZnak()
        {
            var znakResult = _nadgrobniZnakRepository.GetAll()
                .Map(nz => nz.Select(DtoMapping.ToDto));

            return znakResult
                ? Ok(znakResult.Data)
                : Problem(znakResult.Message, statusCode: 500);
        }

        // GET: api/NadgrobniZnak/5
        [HttpGet("{id}")]
        public ActionResult<NadgrobniZnak> GetNadgrobniZnak(int id)
        {
            var znakResult = _nadgrobniZnakRepository.Get(id).Map(DtoMapping.ToDto);

            return znakResult switch
            {
                { IsSuccess: true } => Ok(znakResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(znakResult.Message, statusCode: 500)
            };
        }

        // PUT: api/NadgrobniZnak/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditNadgrobniZnak(int id, NadgrobniZnak znak)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != znak.Id)
            {
                return BadRequest();
            }

            if (!_nadgrobniZnakRepository.Exists(id))
            {
                return NotFound();
            }

            var domainZnak = znak.ToDomain();

            var result =
                domainZnak.IsValid()
                .Bind(() => _nadgrobniZnakRepository.Update(domainZnak));

            return result
                ? AcceptedAtAction("EditNadgrobniZnak", znak)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/NadgrobniZnak
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<NadgrobniZnak> CreateNadgrobniZnak(NadgrobniZnak znak)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainZnak = znak.ToDomain();

            var result =
                domainZnak.IsValid()
                .Bind(() => _nadgrobniZnakRepository.Insert(domainZnak));

            return result
                ? CreatedAtAction("GetNadgrobniZnak", new { id = znak.Id }, znak)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/NadgrobniZnak/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNadgrobniZnak(int id)
        {
            if (!_nadgrobniZnakRepository.Exists(id))
                return NotFound();

            var deleteResult = _nadgrobniZnakRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}