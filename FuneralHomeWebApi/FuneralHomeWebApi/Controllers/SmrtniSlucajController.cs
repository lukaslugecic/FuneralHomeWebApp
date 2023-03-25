using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmrtniSlucajController : ControllerBase
{
    private readonly ISmrtniSlucajRepository<int, DbModels.SmrtniSlucaj> _smrtniSlucajRepository;

    public SmrtniSlucajController(ISmrtniSlucajRepository<int, DbModels.SmrtniSlucaj> smrtniSlucajRepository)
    {
        _smrtniSlucajRepository = smrtniSlucajRepository;
    }

    // GET: api/SmrtniSlucaj
    [HttpGet]
    public ActionResult<IEnumerable<SmrtniSlucaj>> GetAllSmrtniSlucaj()
    {
        return Ok(_smrtniSlucajRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/SmrtniSlucaj/5
    [HttpGet("{id}")]
    public ActionResult<SmrtniSlucaj> GetSmrtniSlucaj(int id)
    {
        var slucajOption = _smrtniSlucajRepository.Get(id).Map(DtoMapping.ToDto);

        return slucajOption
            ? Ok(slucajOption.Data)
            : NotFound();
    }

    // PUT: api/SmrtniSlucaj/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditSmrtniSlucaj(int id, SmrtniSlucaj slucaj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != slucaj.Id)
        {
            return BadRequest();
        }

        if (!_smrtniSlucajRepository.Exists(id))
        {
            return NotFound();
        }

        return _smrtniSlucajRepository.Update(slucaj.ToDbModel())
            ? AcceptedAtAction("EditSmrtniSlucaj", slucaj)
            : StatusCode(500);
    }

    // POST: api/SmrtniSlucaj
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<SmrtniSlucaj> CreateSmrtniSlucaj(SmrtniSlucaj smrtniSlucaj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _smrtniSlucajRepository.Insert(smrtniSlucaj.ToDbModel())
            ? CreatedAtAction("GetSmrtniSlucaj", new { id = smrtniSlucaj.Id }, smrtniSlucaj)
            : StatusCode(500);
    }

    // DELETE: api/SmrtniSlucaj/5
    [HttpDelete("{id}")]
    public IActionResult DeleteSmrtniSlucaj(int id)
    {
        if (!_smrtniSlucajRepository.Exists(id))
            return NotFound();

        return _smrtniSlucajRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}