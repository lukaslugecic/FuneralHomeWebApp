using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;

namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OsmrtnicaController : ControllerBase
{
    private readonly IOsmrtnicaRepository<int, DbModels.Osmrtnica> _osmrtnicaRepository;

    public OsmrtnicaController(IOsmrtnicaRepository<int, DbModels.Osmrtnica> osmrtnicaRespository)
    {
        _osmrtnicaRepository = osmrtnicaRespository;
    }

    // GET: api/Osmrtnica
    [HttpGet]
    public ActionResult<IEnumerable<Osmrtnica>> GetAllOsmrtnica()
    {
        return Ok(_osmrtnicaRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Osmrtnica/5
    [HttpGet("{id}")]
    public ActionResult<Osmrtnica> GetOsmrtnica(int id)
    {
        var osmrtnicaOption = _osmrtnicaRepository.Get(id).Map(DtoMapping.ToDto);

        return osmrtnicaOption
            ? Ok(osmrtnicaOption.Data)
            : NotFound();
    }

    // PUT: api/Osmrtnica/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditOsmrtnica(int id, Osmrtnica osmrtnica)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != osmrtnica.Id)
        {
            return BadRequest();
        }

        if (!_osmrtnicaRepository.Exists(id))
        {
            return NotFound();
        }

        return _osmrtnicaRepository.Update(osmrtnica.ToDbModel())
            ? AcceptedAtAction("EditOsmrtnica", osmrtnica)
            : StatusCode(500);
    }

    // POST: api/Osiguranje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Osmrtnica> CreateOsiguranje(Osmrtnica osmrtnica)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _osmrtnicaRepository.Insert(osmrtnica.ToDbModel())
            ? CreatedAtAction("GetOsmrtnica", new { id = osmrtnica.Id }, osmrtnica)
            : StatusCode(500);
    }

    // DELETE: api/Osiguranje/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOglas(int id)
    {
        if (!_osmrtnicaRepository.Exists(id))
            return NotFound();

        return _osmrtnicaRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}