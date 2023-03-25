using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrnaController : ControllerBase
{
    private readonly IUrnaRepository<int, DbModels.Urna> _urnaRepository;

    public UrnaController(IUrnaRepository<int, DbModels.Urna> urnaRepository)
    {
        _urnaRepository = urnaRepository;
    }

    // GET: api/Urna
    [HttpGet]
    public ActionResult<IEnumerable<Urna>> GetAllUrna()
    {
        return Ok(_urnaRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Urna/5
    [HttpGet("{id}")]
    public ActionResult<Urna> GetUrna(int id)
    {
        var urnaOption = _urnaRepository.Get(id).Map(DtoMapping.ToDto);

        return urnaOption
            ? Ok(urnaOption.Data)
            : NotFound();
    }

    // PUT: api/Urna/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditUrna(int id, Urna urna)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != urna.Id)
        {
            return BadRequest();
        }

        if (!_urnaRepository.Exists(id))
        {
            return NotFound();
        }

        return _urnaRepository.Update(urna.ToDbModel())
            ? AcceptedAtAction("EditOglas", urna)
            : StatusCode(500);
    }

    // POST: api/Urna
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Urna> CreateUrna(Urna urna)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _urnaRepository.Insert(urna.ToDbModel())
            ? CreatedAtAction("GetUrna", new { id = urna.Id }, urna)
            : StatusCode(500);
    }

    // DELETE: api/Urna/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUrna(int id)
    {
        if (!_urnaRepository.Exists(id))
            return NotFound();

        return _urnaRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}