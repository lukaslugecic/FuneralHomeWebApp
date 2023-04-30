using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UslugaController : ControllerBase
{
    private readonly IUslugaRepository _uslugaRepository;

    public UslugaController(IUslugaRepository repository)
    {
        _uslugaRepository = repository;
    }

    // GET: api/Usluga
    [HttpGet]
    public ActionResult<IEnumerable<Usluga>> GetAllUsluga()
    {
        var uslugaResults = _uslugaRepository.GetAll()
            .Map(u => u.Select(DtoMapping.ToDto));

        return uslugaResults
            ? Ok(uslugaResults.Data)
            : Problem(uslugaResults.Message, statusCode: 500);
    }

    // GEt: api/Usluga/Vrste/1
    [HttpGet("/api/[controller]/Vrste/{id}")]
    public ActionResult<IEnumerable<Usluga>> GetAllByType(int id)
    {
        var uslugaResults = _uslugaRepository.GetAllByType(id)
            .Map(u => u.Select(DtoMapping.ToDto));

        return uslugaResults
           ? Ok(uslugaResults.Data)
           : Problem(uslugaResults.Message, statusCode: 500);
    }

    // GET: api/Usluga/5
    [HttpGet("{id}")]
    public ActionResult<Usluga> GetUsluga(int id)
    {
        var uslugaResult = _uslugaRepository.Get(id).Map(DtoMapping.ToDto);

        return uslugaResult switch
        {
            { IsSuccess: true } => Ok(uslugaResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(uslugaResult.Message, statusCode: 500)
        };
    }

    /*
    [HttpGet("/Aggregate/{id}")]
    public ActionResult<PersonAggregate> GetPersonAggregate(int id)
    {
        var personResult = _personRepository.GetAggregate(id).Map(DtoMapping.ToAggregateDto);

        return personResult switch
        {
            { IsSuccess: true } => Ok(personResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(personResult.Message, statusCode: 500)
        };
    }
    */



    // PUT: api/Usluga/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditUsluga(int id, Usluga usluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != usluga.Id)
        {
            return BadRequest();
        }

        if (!_uslugaRepository.Exists(id))
        {
            return NotFound();
        }

        var domainUsluga = usluga.ToDomain();

        var result =
            domainUsluga.IsValid()
            .Bind(() => _uslugaRepository.Update(domainUsluga));

        return result
            ? AcceptedAtAction("EditUsluga", usluga)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/Usluga
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Usluga> CreateUsluga(Usluga usluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainUsluga = usluga.ToDomain();

        var validationResult = domainUsluga.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainUsluga.IsValid()
            .Bind(() => _uslugaRepository.Insert(domainUsluga));

        return result
            ? CreatedAtAction("GetUsluga", new { id = usluga.Id }, usluga)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/Usluga/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUsluga(int id)
    {
        if (!_uslugaRepository.Exists(id))
            return NotFound();

        var deleteResult = _uslugaRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
