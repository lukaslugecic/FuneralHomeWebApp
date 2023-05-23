using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class AddPogreb
{
    public Pogreb Pogreb { get; set; } = new Pogreb();
    public List<PogrebOpremaUsluge> Oprema { get; set; } = new List<PogrebOpremaUsluge>();
}

