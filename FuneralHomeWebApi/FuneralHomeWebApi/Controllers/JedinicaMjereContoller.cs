using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;

namespace FuneralHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JedinicaMjereController : ControllerBase
    {
        private readonly IJedinicaMjereRepository _jedinicaMjereRepository;

        public JedinicaMjereController(IJedinicaMjereRepository repository)
        {
            _jedinicaMjereRepository = repository;
        }

        // GET: api/JedinicaMjere
        [HttpGet]
        public ActionResult<IEnumerable<JedinicaMjere>> GetAllJedinicaMjere()
        {
            var vrstaResult = _jedinicaMjereRepository.GetAll()
                .Map(o => o.Select(DtoMapping.ToDto));

            return vrstaResult
                ? Ok(vrstaResult.Data)
                : Problem(vrstaResult.Message, statusCode: 500);
        }


        // GET: api/JedinicaMjere/5
        [HttpGet("{id}")]
        public ActionResult<JedinicaMjere> GetJedinicaMjere(int id)
        {
            var vrstaResult = _jedinicaMjereRepository.Get(id).Map(DtoMapping.ToDto);

            return vrstaResult switch
            {
                { IsSuccess: true } => Ok(vrstaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(vrstaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/JedinicaMjere/5
        [HttpPut("{id}")]
        public IActionResult EditJedinicaMjere(int id, JedinicaMjere jedMj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != jedMj.Id)
            {
                return BadRequest();
            }

            if (!_jedinicaMjereRepository.Exists(id))
            {
                return NotFound();
            }

            var domainJedMj= jedMj.ToDomain();

            var result =
                domainJedMj.IsValid()
                .Bind(() => _jedinicaMjereRepository.Update(domainJedMj));

            return result
                ? AcceptedAtAction("EditJedinicaMjere", jedMj)
                : Problem(result.Message, statusCode: 500);
        }

        // POST: api/JedinicaMjere
        [HttpPost]
        public ActionResult<JedinicaMjere> CreateJedinicaMjere(JedinicaMjere jedMj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainJedMj = jedMj.ToDomain();

            var result =
                domainJedMj.IsValid()
                .Bind(() => _jedinicaMjereRepository.Insert(domainJedMj));

            return result
                ? CreatedAtAction("GetJedinicaMjere", new { id = jedMj.Id }, jedMj)
                : Problem(result.Message, statusCode: 500);
        }

        // DELETE: api/JedinicaMjere/5
        [HttpDelete("{id}")]
        public IActionResult DeleteJedinicaMjere(int id)
        {
            if (!_jedinicaMjereRepository.Exists(id))
                return NotFound();

            var deleteResult = _jedinicaMjereRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}