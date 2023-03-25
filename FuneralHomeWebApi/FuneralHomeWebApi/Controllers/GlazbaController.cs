using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GlazbaController : ControllerBase
{
    private readonly IGlazbaRepository<int, DbModels.Glazba> _glazbaRepository;

    public GlazbaController(IGlazbaRepository<int, DbModels.Glazba> glazbaRepository)
    {
        _glazbaRepository = glazbaRepository;
    }

    // GET: api/Glazba
    [HttpGet]
    public ActionResult<IEnumerable<Glazba>> GetAllGlazba()
    {
        return Ok(_glazbaRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Glazba/5
    [HttpGet("{id}")]
    public ActionResult<Glazba> GetGlazba(int id)
    {
        var glazbaOption = _glazbaRepository.Get(id).Map(DtoMapping.ToDto);

        return glazbaOption
            ? Ok(glazbaOption.Data)
            : NotFound();
    }

    // PUT: api/Glazba/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditGlazba(int id, Glazba glazba)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != glazba.Id)
        {
            return BadRequest();
        }

        if (!_glazbaRepository.Exists(id))
        {
            return NotFound();
        }

        return _glazbaRepository.Update(glazba.ToDbModel())
            ? AcceptedAtAction("EditGlazba", glazba)
            : StatusCode(500);
    }

    // POST: api/Glazba
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Glazba> CreateGlazba(Glazba glazba)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _glazbaRepository.Insert(glazba.ToDbModel())
            ? CreatedAtAction("GetCvijece", new { id = glazba.Id }, glazba)
            : StatusCode(500);
    }

    // DELETE: api/Glazba/5
    [HttpDelete("{id}")]
    public IActionResult DeleteGlazba(int id)
    {
        if (!_glazbaRepository.Exists(id))
            return NotFound();

        return _glazbaRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}