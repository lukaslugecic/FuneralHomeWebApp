using FuneralHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using FuneralHome.DTOs;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Commons;
using System;


namespace FuneralHomeWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CvijeceController : ControllerBase
{
    private readonly ICvijeceRepository<int, DbModels.Cvijece> _cvijeceRepository;

    public CvijeceController(ICvijeceRepository<int, DbModels.Cvijece> cvijeceRepository)
    {
        _cvijeceRepository = cvijeceRepository;
    }

    // GET: api/Cvijece
    [HttpGet]
    public ActionResult<IEnumerable<Cvijece>> GetAllCvijece()
    {
        return Ok(_cvijeceRepository.GetAll().Select(DtoMapping.ToDto));
    }

    // GET: api/Cvijece/5
    [HttpGet("{id}")]
    public ActionResult<Cvijece> GetCvijece(int id)
    {
        var cvijeceOption = _cvijeceRepository.Get(id).Map(DtoMapping.ToDto);

        return cvijeceOption
            ? Ok(cvijeceOption.Data)
            : NotFound();
    }

    // PUT: api/Cvijece/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult EditCvijece(int id, Cvijece cvijece)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != cvijece.Id)
        {
            return BadRequest();
        }

        if (!_cvijeceRepository.Exists(id))
        {
            return NotFound();
        }

        return _cvijeceRepository.Update(cvijece.ToDbModel())
            ? AcceptedAtAction("EditCvijece", cvijece)
            : StatusCode(500);
    }

    // POST: api/Cvijece
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public ActionResult<Cvijece> CreatePerson(Cvijece cvijece)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return _cvijeceRepository.Insert(cvijece.ToDbModel())
            ? CreatedAtAction("GetCvijece", new { id = cvijece.Id }, cvijece)
            : StatusCode(500);
    }

    // DELETE: api/Cvijece/5
    [HttpDelete("{id}")]
    public IActionResult DeleteCvijece(int id)
    {
        if (!_cvijeceRepository.Exists(id))
            return NotFound();

        return _cvijeceRepository.Remove(id)
            ? NoContent()
            : StatusCode(500);
    }
}