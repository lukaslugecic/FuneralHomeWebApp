using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OglasController : ControllerBase
{
    private readonly IOglasRepository<int, DbModels.Oglas> _oglasRepository;

    public OglasController(IOglasRepository<int, DbModels.Oglas> oglasRepository)
    {
        _oglasRepository = oglasRepository;
    }

    // GET: api/Oglas
    [HttpGet]
    public ActionResult<IEnumerable<Oglas>> GetAllOglas()
    {
        return Ok(_oglasRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Oglas/5
    [HttpGet("{id}")]
    public ActionResult<Oglas> GetOglas(int id)
    {
        var oglasOption = _oglasRepository.Get(id).Map(DtoMapping.ToDto);

        return oglasOption
            ? Ok(oglasOption.Data)
            : NotFound();
    }

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

        return _oglasRepository.Update(oglas.ToDbModel())
            ? AcceptedAtAction("EditOglas", oglas)
            : StatusCode(500);
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

        return _oglasRepository.Insert(oglas.ToDbModel())
            ? CreatedAtAction("GetOglas", new { id = oglas.Id }, oglas)
            : StatusCode(500);
    }

    // DELETE: api/Oglas/5
    [HttpDelete("{id}")]
    public IActionResult DeleteOglas(int id)
    {
        if (!_oglasRepository.Exists(id))
            return NotFound();

        return _oglasRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}