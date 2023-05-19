using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OpremaUslugaController : ControllerBase
{
    private readonly IOpremaUslugaRepository _opremaUslugaRepository;

    public OpremaUslugaController(IOpremaUslugaRepository repository)
    {
        _opremaUslugaRepository = repository;
    }

    // GET: api/Oprema
    [HttpGet]
    public ActionResult<IEnumerable<OpremaUsluga>> GetAllOpremaUsluga()
    {
        var opremaUslugaResults = _opremaUslugaRepository.GetAll()
            .Map(o => o.Select(DtoMapping.ToDto));

        return opremaUslugaResults
            ? Ok(opremaUslugaResults.Data)
            : Problem(opremaUslugaResults.Message, statusCode: 500);
    }

    // Get: api/OpremaUsluga/Vrste/1
    [HttpGet("/api/[controller]/Vrste/{id}")]
    public ActionResult<IEnumerable<OpremaUsluga>> GetAllByType(int id)
    {
        var uslugaResults = _opremaUslugaRepository.GetAllByType(id)
            .Map(o => o.Select(DtoMapping.ToDto));

        return uslugaResults
           ? Ok(uslugaResults.Data)
           : Problem(uslugaResults.Message, statusCode: 500);
    }

    // GET: api/OpremaUsluga/5
    [HttpGet("{id}")]
    public ActionResult<OpremaUsluga> GetOpremaUsluga(int id)
    {
        var opremaResult = _opremaUslugaRepository.Get(id).Map(DtoMapping.ToDto);

        return opremaResult switch
        {
            { IsSuccess: true } => Ok(opremaResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(opremaResult.Message, statusCode: 500)
        };
    }


    // PUT: api/OpremaUsluga/5
    [HttpPut("{id}")]
    public IActionResult EditOpremaUsluga(int id, OpremaUsluga opremaUsluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != opremaUsluga.Id)
        {
            return BadRequest();
        }

        if (!_opremaUslugaRepository.Exists(id))
        {
            return NotFound();
        }

        var domainOpremaUsluga = opremaUsluga.ToDomain();

        var result =
            domainOpremaUsluga.IsValid()
            .Bind(() => _opremaUslugaRepository.Update(domainOpremaUsluga));

        return result
            ? AcceptedAtAction("EditOprema", opremaUsluga)
            : Problem(result.Message, statusCode: 500);
    }

    // PUT: api/OpremaUsluga/IncreaseZaliha/5
    [HttpPut("IncreaseZaliha/{id}")]
    public IActionResult IncreaseZaliha(int id, OpremaUsluga opremaUsluga, int kolicina)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (id != opremaUsluga.Id)
        {
            return BadRequest();
        }
        if (!_opremaUslugaRepository.Exists(id))
        {
            return NotFound();
        }
        var domainOprema = opremaUsluga.ToDomain();
        var result =
            domainOprema.IsValid()
            .Bind(() => _opremaUslugaRepository.IncreaseZaliha(domainOprema, kolicina));
        return result
            ? AcceptedAtAction("IncreaseOpremaUsluga", opremaUsluga)
            : Problem(result.Message, statusCode: 500);
    }


    // PUT: api/OpremaUsluga/DecreaseZaliha/5
    [HttpPut("DecreaseZaliha/{id}")]
    public IActionResult DecreaseZaliha(int id, OpremaUsluga opremaUsluga, int kolicina)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != opremaUsluga.Id)
        {
            return BadRequest();
        }

        if (!_opremaUslugaRepository.Exists(id))
        {
            return NotFound();
        }

        var domainOpremaUsluga = opremaUsluga.ToDomain();

        var result =
            domainOpremaUsluga.IsValid()
            .Bind(() => _opremaUslugaRepository.DecreaseZaliha(domainOpremaUsluga, kolicina));

        return result
            ? AcceptedAtAction("DecreaseOpremaUsluga", opremaUsluga)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/OpremaUsluga
    [HttpPost]
    public ActionResult<OpremaUsluga> CreateOpremaUsluga(OpremaUsluga opremaUsluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainOpremaUsluga = opremaUsluga.ToDomain();

        var validationResult = domainOpremaUsluga.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainOpremaUsluga.IsValid()
            .Bind(() => _opremaUslugaRepository.Insert(domainOpremaUsluga));

        return result
            ? CreatedAtAction("GetOpremaUsluga", new { id = opremaUsluga.Id }, opremaUsluga)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/OpremaUsluga/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOpremaUsluga(int id)
    {
        if (!_opremaUslugaRepository.Exists(id))
            return NotFound();

        var deleteResult = _opremaUslugaRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
