using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvijeceController : ControllerBase
    {
        private readonly ICvijeceRepository _cvijeceRepository;

        public CvijeceController(ICvijeceRepository repository)
        {
            _cvijeceRepository = repository;
        }

        // GET: api/Cvijece
        [HttpGet]
        public ActionResult<IEnumerable<Cvijece>> GetAllCvijece()
        {
            var cvijeceResults = _cvijeceRepository.GetAll()
                .Map(cv => cv.Select(DtoMapping.ToDto));

            return cvijeceResults
                ? Ok(cvijeceResults.Data)
                : Problem(cvijeceResults.Message, statusCode: 500);
        }

        // GET: api/Cvijece/5
        [HttpGet("{id}")]
        public ActionResult<Cvijece> GetCvijece(int id)
        {
            var cvijeceResult = _cvijeceRepository.Get(id).Map(DtoMapping.ToDto);

            return cvijeceResult switch
            {
                { IsSuccess: true } => Ok(cvijeceResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(cvijeceResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Cvijece/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditCvijece(int id, Cvijece cvijece)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != cvijece.Id)
            {
                return BadRequest();
            }

            if (!_cvijeceRepository.Exists(id))
            {
                return NotFound();
            }

            var domainCvijece = cvijece.ToDomain();

            var result =
                domainCvijece.IsValid()
                .Bind(() => _cvijeceRepository.Update(domainCvijece));

            return result
                ? AcceptedAtAction("EditCvijce", cvijece)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/Cvijece
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Cvijece> CreateCvijece(Cvijece cvijece)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainCvijece = cvijece.ToDomain();

            var result =
                domainCvijece.IsValid()
                .Bind(() => _cvijeceRepository.Insert(domainCvijece));

            return result
                ? CreatedAtAction("GetCvijece", new { id = cvijece.Id }, cvijece)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/Cvijece/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCvijece(int id)
        {
            if (!_cvijeceRepository.Exists(id))
                return NotFound();

            var deleteResult = _cvijeceRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}