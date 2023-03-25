using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KorisnikController : ControllerBase
{
    private readonly IKorisnikRepository<int, DbModels.Korisnik> _korisnikRepository;

    public KorisnikController(IKorisnikRepository<int, DbModels.Korisnik> korisnikRepository)
    {
        _korisnikRepository = korisnikRepository;
    }

    // GET: api/Korisnik
    [HttpGet]
    public ActionResult<IEnumerable<Korisnik>> GetAllKorisnik()
    {
        return Ok(_korisnikRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Korisnik/5
    [HttpGet("{id}")]
    public ActionResult<Korisnik> GetKorisnik(int id)
    {
        var korisnikOption = _korisnikRepository.Get(id).Map(DtoMapping.ToDto);

        return korisnikOption
            ? Ok(korisnikOption.Data)
            : NotFound();
    }

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

        return _korisnikRepository.Update(korisnik.ToDbModel())
            ? AcceptedAtAction("EditKorisnik", korisnik)
            : StatusCode(500);
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

        return _korisnikRepository.Insert(korisnik.ToDbModel())
            ? CreatedAtAction("GetKorisnik", new { id = korisnik.Id }, korisnik)
            : StatusCode(500);
    }

    // DELETE: api/Korisnik/5
    [HttpDelete("{id}")]
    public IActionResult DeleteKorisnik(int id)
    {
        if (!_korisnikRepository.Exists(id))
            return NotFound();

        return _korisnikRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}