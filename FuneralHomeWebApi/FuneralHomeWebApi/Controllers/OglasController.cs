using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OglasController : ControllerBase
{
    private readonly IOglasRepository _oglasRepository;

    public OglasController(IOglasRepository repository)
    {
        _oglasRepository = repository;
    }

    // GET: api/Oglas
    [HttpGet]
    public ActionResult<IEnumerable<Oglas>> GetAllOglas()
    {
        var oglasResults = _oglasRepository.GetAll()
            .Map(o => o.Select(DtoMapping.ToDto));

        return oglasResults
            ? Ok(oglasResults.Data)
            : Problem(oglasResults.Message, statusCode: 500);
    }

    // GET: api/Oglas/5
    [HttpGet("{id}")]
    public ActionResult<Oglas> GetOglas(int id)
    {
        var oglasResult = _oglasRepository.Get(id).Map(DtoMapping.ToDto);

        return oglasResult switch
        {
            { IsSuccess: true } => Ok(oglasResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(oglasResult.Message, statusCode: 500)
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



    // PUT: api/Oglas/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditOglas(int id, Oglas oglas)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != oglas.Id)
        {
            return BadRequest();
        }

        if (!_oglasRepository.Exists(id))
        {
            return NotFound();
        }

        var domainOglas = oglas.ToDomain();

        var result =
            domainOglas.IsValid()
            .Bind(() => _oglasRepository.Update(domainOglas));

        return result
            ? AcceptedAtAction("EditOglas", oglas)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/Oglas
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Oglas> CreateOglas(Oglas oglas)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainOglas = oglas.ToDomain();

        var validationResult = domainOglas.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainOglas.IsValid()
            .Bind(() => _oglasRepository.Insert(domainOglas));

        return result
            ? CreatedAtAction("GetOglas", new { id = oglas.Id }, oglas)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/Oglas/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOglas(int id)
    {
        if (!_oglasRepository.Exists(id))
            return NotFound();

        var deleteResult = _oglasRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
