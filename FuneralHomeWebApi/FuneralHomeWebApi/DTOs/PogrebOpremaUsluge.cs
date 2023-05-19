using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PogrebOpremaUsluge
{
    [Required(ErrorMessage = "Oprema/usluga je obavezna!")]
    public OpremaUsluga OpremaUsluga { get; set; }
    public int Kolicina { get; set; }
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static PogrebOpremaUsluge ToDto(this DomainModels.PogrebOpremaUsluge pogrebOpremaUsluge)
        => new PogrebOpremaUsluge()
        {
            OpremaUsluga = pogrebOpremaUsluge.OpremaUsluga.ToDto(),
            Kolicina = pogrebOpremaUsluge.Kolicina,
            Cijena = pogrebOpremaUsluge.Cijena
        };

    public static DomainModels.PogrebOpremaUsluge ToDomain(this PogrebOpremaUsluge pogrebOpremaUsluge, int pogrebId)
        => new DomainModels.PogrebOpremaUsluge(
            pogrebOpremaUsluge.OpremaUsluga.ToDomain(),
            pogrebOpremaUsluge.Kolicina,
            pogrebOpremaUsluge.Cijena
        );
}
