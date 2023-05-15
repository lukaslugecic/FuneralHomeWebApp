using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SmrtniSlucajController : ControllerBase
{
    private readonly ISmrtniSlucajRepository _smrtniSlucajRepository;

    public SmrtniSlucajController(ISmrtniSlucajRepository repository)
    {
        _smrtniSlucajRepository = repository;
    }

    // GET: api/SmrtniSlucaj
    [HttpGet]
    public ActionResult<IEnumerable<SmrtniSlucaj>> GetAllSmrtniSlucaj()
    {
        var slucajResults = _smrtniSlucajRepository.GetAll()
            .Map(ss => ss.Select(DtoMapping.ToDto));

        return slucajResults
            ? Ok(slucajResults.Data)
            : Problem(slucajResults.Message, statusCode: 500);
    }

    // GET: api/SmrtniSlucaj/WithoutFuneral
    [HttpGet("/api/[controller]/WithoutFuneral")]
    public ActionResult<IEnumerable<SmrtniSlucaj>> GetAllWithoutFuneral()
    {
        var slucajResults = _smrtniSlucajRepository.GetAllWithoutFuneral()
            .Map(ss => ss.Select(DtoMapping.ToDto));

        return slucajResults
            ? Ok(slucajResults.Data)
            : Problem(slucajResults.Message, statusCode: 500);
    }

    // GET: api/SmrtniSlucaj/WithoutFuneral/5
    [HttpGet("/api/[controller]/WithoutFuneral/{id}")]
    public ActionResult<IEnumerable<SmrtniSlucaj>> GetAllWithoutFuneralByKorisnikId(int id)
    {
        var slucajResults = _smrtniSlucajRepository.GetAllWithoutFuneralByKorisnikId(id)
            .Map(ss => ss.Select(DtoMapping.ToDto));

        return slucajResults
            ? Ok(slucajResults.Data)
            : Problem(slucajResults.Message, statusCode: 500);
    }

    // GET: api/SmrtniSlucajl/5
    [HttpGet("/api/[controller]/Korisnik/{id}")]
    public ActionResult<IEnumerable<SmrtniSlucaj>> GetAllByKorisnikId(int id)
    {
        var slucajResults = _smrtniSlucajRepository.GetAllByKorisnikId(id)
            .Map(ss => ss.Select(DtoMapping.ToDto));

        return slucajResults
            ? Ok(slucajResults.Data)
            : Problem(slucajResults.Message, statusCode: 500);
    }

    // GET: api/SmrtniSlucaj/5
    [HttpGet("{id}")]
    public ActionResult<SmrtniSlucaj> GetSmrtniSlucaj(int id)
    {
        var slucajResult = _smrtniSlucajRepository.Get(id).Map(DtoMapping.ToDto);

        return slucajResult switch
        {
            { IsSuccess: true } => Ok(slucajResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(slucajResult.Message, statusCode: 500)
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



    // PUT: api/SmrtniSlucaj/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditSmrtniSlucaj(int id, SmrtniSlucaj smrtni)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != smrtni.Id)
        {
            return BadRequest();
        }

        if (!_smrtniSlucajRepository.Exists(id))
        {
            return NotFound();
        }

        var domainSmrtni = smrtni.ToDomain();

        var result =
            domainSmrtni.IsValid()
            .Bind(() => _smrtniSlucajRepository.Update(domainSmrtni));

        return result
            ? AcceptedAtAction("EditSmrtniSlucaj", smrtni)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/SmrtniSlucaj
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<SmrtniSlucaj> CreateSmrtniSlucaj(SmrtniSlucaj slucaj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainSlucaj = slucaj.ToDomain();

        var validationResult = domainSlucaj.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainSlucaj.IsValid()
            .Bind(() => _smrtniSlucajRepository.Insert(domainSlucaj));

        return result
            ? CreatedAtAction("GetSmrtniSlucaj", new { id = slucaj.Id }, slucaj)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/SmrtniSlucaj/5
    [HttpDelete("{id}")]
    public IActionResult DeleteSmrtniSlucaj(int id)
    {
        if (!_smrtniSlucajRepository.Exists(id))
            return NotFound();

        var deleteResult = _smrtniSlucajRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
