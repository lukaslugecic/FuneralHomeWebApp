using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
using System.Data;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PogrebController : ControllerBase
{
    private readonly IPogrebRepository _pogrebRepository;

    public PogrebController(IPogrebRepository repository)
    {
        _pogrebRepository = repository;
    }

    // GET: api/Pogreb
    [HttpGet]
    public ActionResult<IEnumerable<Pogreb>> GetAllPogreb()
    {
        var pogrebResults = _pogrebRepository.GetAll()
            .Map(p => p.Select(DtoMapping.ToDto));

        return pogrebResults
            ? Ok(pogrebResults.Data)
            : Problem(pogrebResults.Message, statusCode: 500);
    }

    // GET: api/Pogreb/PogrebSmrtniSlucaj
    [HttpGet("PogrebSmrtniSlucaj")]
    public ActionResult<IEnumerable<PogrebSmrtniSlucaj>> GetAllPogrebSmrtniSlucaj()
    {
        var pogrebResults = _pogrebRepository.GetAllPogrebSmrtniSlucaj()
            .Map(p => p.Select(DtoMapping.ToDto));
        return pogrebResults
            ? Ok(pogrebResults.Data)
            : Problem(pogrebResults.Message, statusCode: 500);
    }

    // GET: api/Pogreb/5
    [HttpGet("{id}")]
    public ActionResult<Pogreb> GetPogreb(int id)
    {
        var pogrebResult = _pogrebRepository.Get(id).Map(DtoMapping.ToDto);

        return pogrebResult switch
        {
            { IsSuccess: true } => Ok(pogrebResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(pogrebResult.Message, statusCode: 500)
        };
    }

    
    [HttpGet("/api/[controller]/Aggregate/{id}")]
    public ActionResult<PogrebAggregate> GetPogrebAggregate(int id)
    {
        var pogrebResult = _pogrebRepository.GetAggregate(id).Map(DtoMapping.ToAggregateDto);

        return pogrebResult switch
        {
            { IsSuccess: true } => Ok(pogrebResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(pogrebResult.Message, statusCode: 500)
        };
    }


    [HttpPost("AddOprema/{pogrebId}")]
    public IActionResult AddOprema(int pogrebId, PogrebOprema pogrebOprema)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pogrebResult = _pogrebRepository.GetAggregate(pogrebId);
        if (pogrebResult.IsFailure)
        {
            return NotFound();
        }
        if (pogrebResult.IsException)
        {
            return Problem(pogrebResult.Message, statusCode: 500);
        }

        var pogreb = pogrebResult.Data;

        var domainPogrebOprema = pogrebOprema.ToDomain(pogrebId);
        var validationResult = domainPogrebOprema.IsValid();

        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        pogreb.AddOprema(domainPogrebOprema);

        var updateResult =
            pogreb.IsValid()
            .Bind(() => _pogrebRepository.UpdateAggregate(pogreb));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    [HttpPost("RemoveOprema/{pogrebId}")]
    public IActionResult RemoveOprema(int pogrebId, Oprema oprema)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pogrebResult = _pogrebRepository.GetAggregate(pogrebId);
        if (pogrebResult.IsFailure)
        {
            return NotFound();
        }
        if (pogrebResult.IsException)
        {
            return Problem(pogrebResult.Message, statusCode: 500);
        }

        var pogreb = pogrebResult.Data;

        var domainOprema = oprema.ToDomain();

        if (!pogreb.RemoveOprema(domainOprema))
        {
            return NotFound($"Couldn't find equipment {oprema.Naziv}");
        }

        var updateResult =
            pogreb.IsValid()
            .Bind(() => _pogrebRepository.UpdateAggregate(pogreb));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    [HttpPost("AddUsluga/{pogrebId}")]
    public IActionResult AddUsluga(int pogrebId, Usluga usluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pogrebResult = _pogrebRepository.GetAggregate(pogrebId);
        if (pogrebResult.IsFailure)
        {
            return NotFound();
        }
        if (pogrebResult.IsException)
        {
            return Problem(pogrebResult.Message, statusCode: 500);
        }

        var pogreb = pogrebResult.Data;

        var domainPogrebUsluga = usluga.ToDomain();
        var validationResult = domainPogrebUsluga.IsValid();

        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        pogreb.AddUsluga(domainPogrebUsluga);

        var updateResult =
            pogreb.IsValid()
            .Bind(() => _pogrebRepository.UpdateAggregate(pogreb));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    [HttpPost("RemoveUsluga/{pogrebId}")]
    public IActionResult RemoveUsluga(int pogrebId, Usluga usluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pogrebResult = _pogrebRepository.GetAggregate(pogrebId);
        if (pogrebResult.IsFailure)
        {
            return NotFound();
        }
        if (pogrebResult.IsException)
        {
            return Problem(pogrebResult.Message, statusCode: 500);
        }

        var pogreb = pogrebResult.Data;

        var domainUsluga = usluga.ToDomain();

        if (!pogreb.RemoveUsluga(domainUsluga))
        {
            return NotFound($"Couldn't find service {usluga.Naziv}");
        }

        var updateResult =
            pogreb.IsValid()
            .Bind(() => _pogrebRepository.UpdateAggregate(pogreb));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    // PUT: api/Pogreb/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditPogreb(int id, Pogreb pogreb)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != pogreb.Id)
        {
            return BadRequest();
        }

        if (!_pogrebRepository.Exists(id))
        {
            return NotFound();
        }

        var domainPogreb = pogreb.ToDomain();

        var result =
            domainPogreb.IsValid()
            .Bind(() => _pogrebRepository.Update(domainPogreb));

        return result
            ? AcceptedAtAction("EditPogreb", pogreb)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/Pogreb
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Pogreb> CreatePogreb(Pogreb pogreb)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainPogreb = pogreb.ToDomain();

        var validationResult = domainPogreb.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainPogreb.IsValid()
            .Bind(() => _pogrebRepository.Insert(domainPogreb));

        return result
            ? CreatedAtAction("GetPogreb", new { id = pogreb.Id }, pogreb)
            : Problem(result.Message, statusCode: 500);
    }

    // DELETE: api/Pogreb/5
    [HttpDelete("{id}")]
    public IActionResult DeletePogreb(int id)
    {
        if (!_pogrebRepository.Exists(id))
            return NotFound();

        var deleteResult = _pogrebRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
