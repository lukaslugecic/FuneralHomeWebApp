using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using FuneralHome.Repositories.SqlServer;
using BaseLibrary;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuneralHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class KorisnikController : ControllerBase
{
    private readonly IKorisnikRepository _korisnikRepository;
    public KorisnikController(IKorisnikRepository repository)
    {
        _korisnikRepository = repository;
    }

    // GET: api/Korisnik
   // [Authorize(Roles = "A")]
    [HttpGet]
    public ActionResult<IEnumerable<Korisnik>> GetAllKorisnik()
    {
        var korisnikResults = _korisnikRepository.GetAll()
            .Map(k => k.Select(DtoMapping.ToDto));

        return korisnikResults
            ? Ok(korisnikResults.Data)
            : Problem(korisnikResults.Message, statusCode: 500);
    }
    // GET: api/Korisnik/WithoutInsurance
    [HttpGet("/api/[controller]/WithoutInsurance")]
    public ActionResult<IEnumerable<Korisnik>> GetAllWithoutInsurance()
    {
        var korisnikResults = _korisnikRepository.GetAllWithoutInsurance()
            .Map(k => k.Select(DtoMapping.ToDto));
        return korisnikResults
            ? Ok(korisnikResults.Data)
            : Problem(korisnikResults.Message, statusCode: 500);
    }


    // GET: api/Korisnik/5
    [HttpGet("{id}")]
    public ActionResult<Korisnik> GetKorisnik(int id)
    {
        var korisnikResult = _korisnikRepository.Get(id).Map(DtoMapping.ToDto);

        return korisnikResult switch
        {
            { IsSuccess: true } => Ok(korisnikResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(korisnikResult.Message, statusCode: 500)
        };
    }


    // GET: api/Korisnik/GetByMail/example@mail.com
    [HttpGet("/api/[controller]/GetByMail")]
    public ActionResult<Korisnik> GetKorisnikByMail(string mail)
    {
        var korisnikResult = _korisnikRepository.GetByMail(mail).Map(DtoMapping.ToDto);

        return korisnikResult switch
        {
            { IsSuccess: true } => Ok(korisnikResult.Data),
            { IsFailure: true } => NotFound(),
            { IsException: true } or _ => Problem(korisnikResult.Message, statusCode: 500)
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



    // PUT: api/Korisnik/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditKorisnik(int id, Korisnik korisnik)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != korisnik.Id)
        {
            return BadRequest();
        }

        if (!_korisnikRepository.Exists(id))
        {
            return NotFound();
        }

        var domainKorisnik = korisnik.ToDomain();

        // Hash the password using BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);

        // Store the hashed password in the Korisnik object
        domainKorisnik.Lozinka = hashedPassword;

        var result =
            domainKorisnik.IsValid()
            .Bind(() => _korisnikRepository.Update(domainKorisnik));

        return result
            ? AcceptedAtAction("EditKorisnik", korisnik)
            : Problem(result.Message, statusCode: 500);
    }

    // POST: api/Korisnik
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Korisnik> CreateKorisnik(Korisnik korisnik)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var domainKorisnik = korisnik.ToDomain();

        var validationResult = domainKorisnik.IsValid();
        if (!validationResult)
        {
            return Problem(validationResult.Message, statusCode: 500);
        }

        
        // Hash the password using BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);

        // Store the hashed password in the Korisnik object
        domainKorisnik.Lozinka = hashedPassword;
       
        var result =
            domainKorisnik.IsValid()
            .Bind(() => _korisnikRepository.Insert(domainKorisnik));

        return result
            ? CreatedAtAction("GetKorisnik", new { id = korisnik.Id },  korisnik)
            : Problem(result.Message, statusCode: 500);
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        
        if (!_korisnikRepository.Exists(model.Mail))
            return Unauthorized();
        var korisnik = _korisnikRepository.GetByMail(model.Mail);

        if(!BCrypt.Net.BCrypt.Verify(model.Lozinka, korisnik.Data.Lozinka))
            return Unauthorized();

        string token = _korisnikRepository.CreateToken(korisnik.Data);

        var authKorisnik = korisnik.Data.ToDto(token);

        return Ok(authKorisnik);
    }

    

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }


    // DELETE: api/Korisnik/5
    [HttpDelete("{id}")]
    public IActionResult DeleteKorisnik(int id)
    {
        if (!_korisnikRepository.Exists(id))
            return NotFound();

        var deleteResult = _korisnikRepository.Remove(id);
        return deleteResult
            ? NoContent()
            : Problem(deleteResult.Message, statusCode: 500);
    }
}
