using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LijesController : ControllerBase
{
    private readonly ILijesRepository<int, DbModels.Lijes> _lijesRepository;

    public LijesController(ILijesRepository<int, DbModels.Lijes> lijesRepository)
    {
        _lijesRepository = lijesRepository;
    }

    // GET: api/Lijes
    [HttpGet]
    public ActionResult<IEnumerable<Lijes>> GetAllLijes()
    {
        return Ok(_lijesRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Lijes/5
    [HttpGet("{id}")]
    public ActionResult<Lijes> GetLijes(int id)
    {
        var lijesOption = _lijesRepository.Get(id).Map(DtoMapping.ToDto);

        return lijesOption
            ? Ok(lijesOption.Data)
            : NotFound();
    }

    // PUT: api/Lijes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditLijes(int id, Lijes lijes)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != lijes.Id)
        {
            return BadRequest();
        }

        if (!_lijesRepository.Exists(id))
        {
            return NotFound();
        }

        return _lijesRepository.Update(lijes.ToDbModel())
            ? AcceptedAtAction("EditLijes", lijes)
            : StatusCode(500);
    }

    // POST: api/Lijes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Lijes> CreateLijes(Lijes lijes)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _lijesRepository.Insert(lijes.ToDbModel())
            ? CreatedAtAction("GetLijes", new { id = lijes.Id }, lijes)
            : StatusCode(500);
    }

    // DELETE: api/Lijes/5
    [HttpDelete("{id}")]
    public IActionResult DeleteLijes(int id)
    {
        if (!_lijesRepository.Exists(id))
            return NotFound();

        return _lijesRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}