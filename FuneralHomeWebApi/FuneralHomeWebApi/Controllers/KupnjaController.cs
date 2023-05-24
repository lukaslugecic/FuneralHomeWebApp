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
public class KupnjaController : ControllerBase
{
    private readonly IKupnjaRepository _kupnjaRepository;
    private readonly IOpremaUslugaRepository _opremaUslugaRepository;

    public KupnjaController(IKupnjaRepository repository,
        IOpremaUslugaRepository opremaRepository)
    {
        _kupnjaRepository = repository;
        _opremaUslugaRepository = opremaRepository;
    }

    // GET: api/Kupnja
    [HttpGet]
    public ActionResult<IEnumerable<Kupnja>> GetAllKupnja()
    {
        var kupnjaResults = _kupnjaRepository.GetAll()
            .Map(k => k.Select(DtoMapping.ToDto));

        return kupnjaResults
            ? Ok(kupnjaResults.Data)
            : Problem(kupnjaResults.Message, statusCode: 500);
    }

    // GET: api/Kupnja/Aggregates
    [HttpGet("Aggregates")]
    public ActionResult<IEnumerable<KupnjaAggregate>> GetAllKupnjaAggregate()
    {
        var kupnjaResults = _kupnjaRepository.GetAllAggregates()
            .Map(p => p.Select(DtoMapping.ToAggregateDto));
        return kupnjaResults
            ? Ok(kupnjaResults.Data)
            : Problem(kupnjaResults.Message, statusCode: 500);
    }

    // GET: api/Kupnja/Aggregates/Korisnik/5
    [HttpGet("Aggregates/Korisnik/{id}")]
    public ActionResult<IEnumerable<KupnjaAggregate>> GetAllKupnjaAggregatesByKorisnikId(int id)
    {
        var kupnjaResults = _kupnjaRepository.GetAllAggregatesByKorisnikId(id)
            .Map(p => p.Select(DtoMapping.ToAggregateDto));
        return kupnjaResults
            ? Ok(kupnjaResults.Data)
            : Problem(kupnjaResults.Message, statusCode: 500);
    }


    // GET : api/Kupanja/Korisnik/5
    [HttpGet("Korisnik/{id}")]
    public ActionResult<IEnumerable<PogrebSmrtniSlucaj>> GetAllKupnjaByKorisnikId(int id)
    {
        var kupnjaKorisnik = _kupnjaRepository.GetAllByKorisnikId(id)
            .Map(k => k.Select(DtoMapping.ToDto));
        return kupnjaKorisnik
            ? Ok(kupnjaKorisnik.Data)
            : Problem(kupnjaKorisnik.Message, statusCode: 500);
    }


    // GET: api/Kupnja/5
    [HttpGet("{id}")]
    public ActionResult<Kupnja> GetKupnja(int id)
    {
        var kupnjaResults = _kupnjaRepository.Get(id).Map(DtoMapping.ToDto);

        return kupnjaResults switch
        {
            { IsSuccess: true } => Ok(kupnjaResults.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(kupnjaResults.Message, statusCode: 500)
        };
    }


    [HttpGet("/api/[controller]/Aggregate/{id}")]
    public ActionResult<KupnjaAggregate> GetKupnjaAggregate(int id)
    {
        var kupnjaResult = _kupnjaRepository.GetAggregate(id).Map(DtoMapping.ToAggregateDto);

        return kupnjaResult switch
        {
            { IsSuccess: true } => Ok(kupnjaResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(kupnjaResult.Message, statusCode: 500)
        };
    }

    
    [HttpPost("AddOprema/{kupnjaId}")]
    public IActionResult AddOprema(int kupnjaId, KupnjaOpremaUsluge kupnjaOprema)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var kupnjaResult = _kupnjaRepository.GetAggregate(kupnjaId);
        if (kupnjaResult.IsFailure)
        {
            return NotFound();
        }
        if (kupnjaResult.IsException)
        {
            return Problem(kupnjaResult.Message, statusCode: 500);
        }

        var kupnja = kupnjaResult.Data;

        var domainKupnjaOprema = kupnjaOprema.ToDomain(kupnjaId);
        var validationResult = domainKupnjaOprema.IsValid();

        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        if (kupnja.AddOprema(domainKupnjaOprema))
        {
            var opremaResult = _opremaUslugaRepository.DecreaseZaliha(domainKupnjaOprema.OpremaUsluga, domainKupnjaOprema.Kolicina);
            if (opremaResult.IsFailure)
            {
                return Problem(opremaResult.Message, statusCode: 500);
            }
        };

        var updateResult =
            kupnja.IsValid()
            .Bind(() => _kupnjaRepository.UpdateAggregate(kupnja));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    [HttpPost("RemoveOprema/{kupnjaId}")]
    public IActionResult RemoveOprema(int kupnjaId, OpremaUsluga opremaUsluga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var kupnjaResult = _kupnjaRepository.GetAggregate(kupnjaId);
        if (kupnjaResult.IsFailure)
        {
            return NotFound();
        }
        if (kupnjaResult.IsException)
        {
            return Problem(kupnjaResult.Message, statusCode: 500);
        }

        var kupnja = kupnjaResult.Data;

        var domainOprema = opremaUsluga.ToDomain();

        var kolicina = kupnja.KupnjaOpremaUsluge
            .Where(o => o.OpremaUsluga.Id == opremaUsluga.Id)
            .Select(o => o.Kolicina).FirstOrDefault();

        if (kolicina == 0)
        {
            return NotFound($"Couldn't find equipment kolicina {opremaUsluga.Naziv}");
        }

        if (!kupnja.RemoveOpremaUsluga(domainOprema))
        {
            return NotFound($"Couldn't find equipment {opremaUsluga.Naziv}");
        }

        var opremaResult = _opremaUslugaRepository.IncreaseZaliha(domainOprema, kolicina);
        if (opremaResult.IsFailure)
        {
            return Problem(opremaResult.Message, statusCode: 500);
        }


        var updateResult =
            kupnja.IsValid()
            .Bind(() => _kupnjaRepository.UpdateAggregate(kupnja));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }

    [HttpPost("IncrementOprema/{kupnjaId}")]
    public IActionResult IncrementOprema(int kupnjaId, int opremaId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var kupnjaResult = _kupnjaRepository.GetAggregate(kupnjaId);
        if (kupnjaResult.IsFailure)
        {
            return NotFound();
        }
        if (kupnjaResult.IsException)
        {
            return Problem(kupnjaResult.Message, statusCode: 500);
        }

        var kupnja = kupnjaResult.Data;

        if (kupnja.IncrementOpremaUsluga(opremaId))
        {
            var oprema = _opremaUslugaRepository.Get(opremaId).Data;
            var opremaResult = _opremaUslugaRepository.DecreaseZaliha(oprema, 1);
            if (opremaResult.IsFailure)
            {
                return Problem(opremaResult.Message, statusCode: 500);
            }
        };

        var updateResult =
            kupnja.IsValid()
            .Bind(() => _kupnjaRepository.UpdateAggregate(kupnja));

        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }

    [HttpPost("DecrementOprema/{pogrebId}")]
    public IActionResult DecrementOprema(int kupnjaId, int opremaId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var kupnjaResult = _kupnjaRepository.GetAggregate(kupnjaId);
        if (kupnjaResult.IsFailure)
        {
            return NotFound();
        }
        if (kupnjaResult.IsException)
        {
            return Problem(kupnjaResult.Message, statusCode: 500);
        }
        var kupnja = kupnjaResult.Data;

        if (kupnja.DecrementOpremaUsluga(opremaId))
        {
            var oprema = _opremaUslugaRepository.Get(opremaId).Data;
            var opremaResult = _opremaUslugaRepository.IncreaseZaliha(oprema, 1);
            if (opremaResult.IsFailure)
            {
                return Problem(opremaResult.Message, statusCode: 500);
            }
        };

        var updateResult =
            kupnja.IsValid()
            .Bind(() => _kupnjaRepository.UpdateAggregate(kupnja));
        return updateResult
            ? Accepted()
            : Problem(updateResult.Message, statusCode: 500);
    }


    // PUT: api/Kupnja/5
    [HttpPut("{id}")]
    public IActionResult EditKupnja(int id, Kupnja kupnja)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != kupnja.Id)
        {
            return BadRequest();
        }

        if (!_kupnjaRepository.Exists(id))
        {
            return NotFound();
        }

        var domainKupnja = kupnja.ToDomain();

        var result =
            domainKupnja.IsValid()
            .Bind(() => _kupnjaRepository.Update(domainKupnja));

        return result
            ? AcceptedAtAction("EditKupnja", kupnja)
            : Problem(result.Message, statusCode: 500);
    }


    // POST: api/Kupnja
    [HttpPost]
    public ActionResult<Kupnja> CreateKupnja(Kupnja kupnja)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainKupnja = kupnja.ToDomain();

        var validationResult = domainKupnja.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainKupnja.IsValid()
            .Bind(() => _kupnjaRepository.Insert(domainKupnja));

        return result
            ? CreatedAtAction("GetKupnja", new { id = kupnja.Id }, kupnja)
            : Problem(result.Message, statusCode: 500);
    }

    
    // DELETE: api/Kupnja/5
    [HttpDelete("{id}")]
    public IActionResult DeleteKupnja(int id)
    {
        if (!_kupnjaRepository.Exists(id))
            return NotFound();

        var deleteResult = _kupnjaRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }


    [HttpPut("AddKupnja")]
    public IActionResult AddKupnja(AddKupnja addKupnja)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainKupnja = addKupnja.Kupnja.ToDomain();

        var validationResult = domainKupnja.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        var result =
            domainKupnja.IsValid()
            .Bind(() => _kupnjaRepository.Insert(domainKupnja));

        if (!result)
        {
            return Problem(result.Message, statusCode: 500);
        }

        var idResult = _kupnjaRepository.GetLatestByKorisnikId(addKupnja.Kupnja.KorisnikId);
        if (!idResult)
        {
            return Problem(idResult.Message, statusCode: 500);
        }
        int id = idResult.Data.Id;

        addKupnja.Kupnja.Id = id;

        var domainKupnjaWithId = addKupnja.Kupnja.ToDomain();

        var validationResulWithId = domainKupnjaWithId.IsValid(); // domainKupnjaWithId
        if (!validationResulWithId)
        {
            var deleteResult = _kupnjaRepository.Remove(id);
            return Problem(validationResulWithId.Message, statusCode: 500);
        }

        foreach (KupnjaOpremaUsluge kupnjaOprema in addKupnja.Oprema)
        {
            var domainKupnjaOprema = kupnjaOprema.ToDomain(id);
            var kupnjaOpremavalidationResult = domainKupnjaOprema.IsValid();
            if (!kupnjaOpremavalidationResult)
            {
                return Problem(kupnjaOpremavalidationResult.Message, statusCode: 500);
            }

            if (domainKupnjaWithId.AddOprema(domainKupnjaOprema))
            {
                var opremaResult = _opremaUslugaRepository.DecreaseZaliha(domainKupnjaOprema.OpremaUsluga, domainKupnjaOprema.Kolicina);
                if (!opremaResult)
                {
                    var deleteResult = _kupnjaRepository.Remove(id);
                    return Problem(opremaResult.Message, statusCode: 500);
                }
            };
        }

        var updateResult = domainKupnjaWithId.IsValid()
                .Bind(() => _kupnjaRepository.UpdateAggregate(domainKupnjaWithId));

        if (!updateResult)
        {
            // obriši pogreb
            var deleteResult = _kupnjaRepository.Remove(id);
            return Problem(updateResult.Message, statusCode: 500);
        }

        return CreatedAtAction("GetKupnja", new { id = id }, addKupnja);
    }

}
