using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OpremaController : ControllerBase
{
    private readonly IOpremaRepository _opremaRepository;

    public OpremaController(IOpremaRepository repository)
    {
        _opremaRepository = repository;
    }

    // GET: api/Oprema
    [HttpGet]
    public ActionResult<IEnumerable<Oprema>> GetAllOprema()
    {
        var opremaResults = _opremaRepository.GetAll()
            .Map(o => o.Select(DtoMapping.ToDto));

        return opremaResults
            ? Ok(opremaResults.Data)
            : Problem(opremaResults.Message, statusCode: 500);
    }

    // Get: api/Oprema/Vrste/1
    [HttpGet("/api/[controller]/Vrste/{id}")]
    public ActionResult<IEnumerable<Usluga>> GetAllByType(int id)
    {
        var uslugaResults = _opremaRepository.GetAllByType(id)
            .Map(o => o.Select(DtoMapping.ToDto));

        return uslugaResults
           ? Ok(uslugaResults.Data)
           : Problem(uslugaResults.Message, statusCode: 500);
    }

    // GET: api/Oprema/5
    [HttpGet("{id}")]
    public ActionResult<Oprema> GetOprema(int id)
    {
        var opremaResult = _opremaRepository.Get(id).Map(DtoMapping.ToDto);

        return opremaResult switch
        {
            { IsSuccess: true } => Ok(opremaResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(opremaResult.Message, statusCode: 500)
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



    // PUT: api/Oprema/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditOprema(int id, Oprema oprema)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != oprema.Id)
        {
            return BadRequest();
        }

        if (!_opremaRepository.Exists(id))
        {
            return NotFound();
        }

        var domainOprema = oprema.ToDomain();

        var result =
            domainOprema.IsValid()
            .Bind(() => _opremaRepository.Update(domainOprema));

        return result
            ? AcceptedAtAction("EditOprema", oprema)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/Oprema
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Oprema> CreateOprema(Oprema oprema)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainOprema = oprema.ToDomain();

        var validationResult = domainOprema.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainOprema.IsValid()
            .Bind(() => _opremaRepository.Insert(domainOprema));

        return result
            ? CreatedAtAction("GetOprema", new { id = oprema.Id }, oprema)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/Oprema/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOprema(int id)
    {
        if (!_opremaRepository.Exists(id))
            return NotFound();

        var deleteResult = _opremaRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
