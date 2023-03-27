using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LijesController : ControllerBase
    {
        private readonly ILijesRepository _lijesRepository;

        public LijesController(ILijesRepository repository)
        {
            _lijesRepository = repository;
        }

        // GET: api/Lijes
        [HttpGet]
        public ActionResult<IEnumerable<Lijes>> GetAllLijes()
        {
            var lijesResult = _lijesRepository.GetAll()
                .Map(l => l.Select(DtoMapping.ToDto));

            return lijesResult
                ? Ok(lijesResult.Data)
                : Problem(lijesResult.Message, statusCode: 500);
        }

        // GET: api/Lijes/5
        [HttpGet("{id}")]
        public ActionResult<Lijes> GetLijes(int id)
        {
            var lijesResult = _lijesRepository.Get(id).Map(DtoMapping.ToDto);

            return lijesResult switch
            {
                { IsSuccess: true } => Ok(lijesResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(lijesResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Lijes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditLijes(int id, Lijes lijes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != lijes.Id)
            {
                return BadRequest();
            }

            if (!_lijesRepository.Exists(id))
            {
                return NotFound();
            }

            var domainLijes = lijes.ToDomain();

            var result =
                domainLijes.IsValid()
                .Bind(() => _lijesRepository.Update(domainLijes));

            return result
                ? AcceptedAtAction("EditLijes", lijes)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/Lijes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Lijes> CreateLijes(Lijes lijes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainLijes = lijes.ToDomain();

            var result =
                domainLijes.IsValid()
                .Bind(() => _lijesRepository.Insert(domainLijes));

            return result
                ? CreatedAtAction("GetLijes", new { id = lijes.Id }, lijes)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/Lijes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteLijes(int id)
        {
            if (!_lijesRepository.Exists(id))
                return NotFound();

            var deleteResult = _lijesRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}