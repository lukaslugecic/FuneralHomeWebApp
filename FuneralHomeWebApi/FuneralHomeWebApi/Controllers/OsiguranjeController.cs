using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OsiguranjeController : ControllerBase
{
    private readonly IOsiguranjeRepository<int, DbModels.Osiguranje> _osiguranjeRepository;

    public OsiguranjeController(IOsiguranjeRepository<int, DbModels.Osiguranje> osiguranjeRepository)
    {
        _osiguranjeRepository = osiguranjeRepository;
    }

    // GET: api/Osiguranje
    [HttpGet]
    public ActionResult<IEnumerable<Osiguranje>> GetAllOsiguranje()
    {
        return Ok(_osiguranjeRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Osiguranje/5
    [HttpGet("{id}")]
    public ActionResult<Osiguranje> GetOsiguranje(int id)
    {
        var osiguranjeOption = _osiguranjeRepository.Get(id).Map(DtoMapping.ToDto);

        return osiguranjeOption
            ? Ok(osiguranjeOption.Data)
            : NotFound();
    }

    // PUT: api/Osiguranje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditOsiguranje(int id, Osiguranje osiguranje)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != osiguranje.Id)
        {
            return BadRequest();
        }

        if (!_osiguranjeRepository.Exists(id))
        {
            return NotFound();
        }

        return _osiguranjeRepository.Update(osiguranje.ToDbModel())
            ? AcceptedAtAction("EditOglas", osiguranje)
            : StatusCode(500);
    }

    // POST: api/Osiguranje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Oglas> CreateOsiguranje(Osiguranje osiguranje)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _osiguranjeRepository.Insert(osiguranje.ToDbModel())
            ? CreatedAtAction("GetOglas", new { id = osiguranje.Id }, osiguranje)
            : StatusCode(500);
    }

    // DELETE: api/Osiguranje/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOglas(int id)
    {
        if (!_osiguranjeRepository.Exists(id))
            return NotFound();

        return _osiguranjeRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}