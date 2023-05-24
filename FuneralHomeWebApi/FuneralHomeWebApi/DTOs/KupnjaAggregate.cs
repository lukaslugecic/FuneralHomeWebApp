using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class KupnjaAggregate
{
    public int Id { get; set; }
    public int KorisnikId { get; set; }
    public DateTime DatumKupovine { get; set; }
   
    public decimal UkupnaCijena { get; set; }
    public IEnumerable<KupnjaOpremaUsluge> OpremaUsluge { get; set; } = Enumerable.Empty<KupnjaOpremaUsluge>();
    public Korisnik Korisnik { get; set; } = new Korisnik();
}


public static partial class DtoMapping
{
    public static KupnjaAggregate ToAggregateDto(this DomainModels.Kupnja kupnja)
        => new KupnjaAggregate()
        {
            Id = kupnja.Id,
            KorisnikId = kupnja.KorisnikId,
            DatumKupovine = kupnja.DatumKupovine,
            UkupnaCijena = kupnja.UkupnaCijena,
            OpremaUsluge = kupnja.KupnjaOpremaUsluge == null
                            ? new List<KupnjaOpremaUsluge>()
                            : kupnja.KupnjaOpremaUsluge.Select(pa => pa.ToDto()).ToList(),
            Korisnik = kupnja.Korisnik!.ToDto()
        };

    public static DomainModels.Kupnja ToDomain(this KupnjaAggregate kupnja)
        => new DomainModels.Kupnja(
            kupnja.Id,
            kupnja.KorisnikId,
            kupnja.DatumKupovine,
            kupnja.UkupnaCijena,
            kupnja.Korisnik.ToDomain(),
            kupnja.OpremaUsluge.Select(ToDomain)
        );
}
