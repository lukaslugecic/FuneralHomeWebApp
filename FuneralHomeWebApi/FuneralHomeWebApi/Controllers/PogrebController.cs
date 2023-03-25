using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PogrebController : ControllerBase
{
    private readonly IPogrebRepository<int, DbModels.Pogreb> _pogrebRepository;

    public PogrebController(IPogrebRepository<int, DbModels.Pogreb> pogrebRepository)
    {
        _pogrebRepository = pogrebRepository;
    }

    // GET: api/Pogreb
    [HttpGet]
    public ActionResult<IEnumerable<Pogreb>> GetAllPogreb()
    {
        return Ok(_pogrebRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Pogreb/5
    [HttpGet("{id}")]
    public ActionResult<Pogreb> GetPogreb(int id)
    {
        var pogrebOption = _pogrebRepository.Get(id).Map(DtoMapping.ToDto);

        return pogrebOption
            ? Ok(pogrebOption.Data)
            : NotFound();
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

        return _pogrebRepository.Update(pogreb.ToDbModel())
            ? AcceptedAtAction("EditPogreb", pogreb)
            : StatusCode(500);
    }

    // POST: api/Pogreb
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Oglas> CreatePogreb(Pogreb pogreb)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _pogrebRepository.Insert(pogreb.ToDbModel())
            ? CreatedAtAction("GetPogreb", new { id = pogreb.Id }, pogreb)
            : StatusCode(500);
    }

    // DELETE: api/Pogreb/5
    [HttpDelete("{id}")]
    public IActionResult DeletePogreb(int id)
    {
        if (!_pogrebRepository.Exists(id))
            return NotFound();

        return _pogrebRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}