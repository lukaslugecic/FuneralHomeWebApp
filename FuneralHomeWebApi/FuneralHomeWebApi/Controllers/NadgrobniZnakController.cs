using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NadgrobniZnakController : ControllerBase
{
    private readonly INadgrobniZnakRepository<int, DbModels.NadgrobniZnak> _nadgrobniZnakRepository;

    public NadgrobniZnakController(INadgrobniZnakRepository<int, DbModels.NadgrobniZnak> nadgrobniZnakRepository)
    {
        _nadgrobniZnakRepository = nadgrobniZnakRepository;
    }

    // GET: api/NadgrobniZnak
    [HttpGet]
    public ActionResult<IEnumerable<NadgrobniZnak>> GetAllNadgrobniZnak()
    {
        return Ok(_nadgrobniZnakRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/NadgrobniZnak/5
    [HttpGet("{id}")]
    public ActionResult<NadgrobniZnak> GetNadgrobniZnak(int id)
    {
        var nadgrobniZnakOption = _nadgrobniZnakRepository.Get(id).Map(DtoMapping.ToDto);

        return nadgrobniZnakOption
            ? Ok(nadgrobniZnakOption.Data)
            : NotFound();
    }

    // PUT: api/NadgrobniZnak/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditNadgrobniZnak(int id, NadgrobniZnak nadgrobni)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != nadgrobni.Id)
        {
            return BadRequest();
        }

        if (!_nadgrobniZnakRepository.Exists(id))
        {
            return NotFound();
        }

        return _nadgrobniZnakRepository.Update(nadgrobni.ToDbModel())
            ? AcceptedAtAction("EditNadgrobniZnak", nadgrobni)
            : StatusCode(500);
    }

    // POST: api/NadgrobniZnak
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<NadgrobniZnak> CreateNadgrobniZnak(NadgrobniZnak nadgrobni)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _nadgrobniZnakRepository.Insert(nadgrobni.ToDbModel())
            ? CreatedAtAction("GetLijes", new { id = nadgrobni.Id }, nadgrobni)
            : StatusCode(500);
    }

    // DELETE: api/NadgrobniZnak/5
    [HttpDelete("{id}")]
    public IActionResult DeleteNadgrobniZnak(int id)
    {
        if (!_nadgrobniZnakRepository.Exists(id))
            return NotFound();

        return _nadgrobniZnakRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}