using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PogrebOprema
{
    [Required(ErrorMessage = "Oprema je obavezna!")]
    public Oprema Oprema { get; set; }
    public int Kolicina { get; set; }
}


public static partial class DtoMapping
{
    public static PogrebOprema ToDto(this DomainModels.PogrebOprema pogrebOprema)
        => new PogrebOprema()
        {
            Oprema = pogrebOprema.Oprema.ToDto(),
            Kolicina = pogrebOprema.Kolicina
        };

    public static DomainModels.PogrebOprema ToDomain(this PogrebOprema pogrebOprema, int pogrebId)
        => new DomainModels.PogrebOprema(
            pogrebOprema.Oprema.ToDomain(),
            pogrebOprema.Kolicina
        );
}
