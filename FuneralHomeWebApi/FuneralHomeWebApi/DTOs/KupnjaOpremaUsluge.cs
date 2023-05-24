using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class KupnjaOpremaUsluge
{
    [Required(ErrorMessage = "Oprema/usluga je obavezna!")]
    public OpremaUsluga OpremaUsluga { get; set; }
    public int Kolicina { get; set; }
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static KupnjaOpremaUsluge ToDto(this DomainModels.KupnjaOpremaUsluge kupnjaOpremaUsluge)
        => new KupnjaOpremaUsluge()
        {
            OpremaUsluga = kupnjaOpremaUsluge.OpremaUsluga.ToDto(),
            Kolicina = kupnjaOpremaUsluge.Kolicina,
            Cijena = kupnjaOpremaUsluge.Cijena
        };

    public static DomainModels.KupnjaOpremaUsluge ToDomain(this KupnjaOpremaUsluge kupnjaOpremaUsluge, int pogrebId)
        => new DomainModels.KupnjaOpremaUsluge(
            kupnjaOpremaUsluge.OpremaUsluga.ToDomain(),
            kupnjaOpremaUsluge.Kolicina,
            kupnjaOpremaUsluge.Cijena
        );
}
