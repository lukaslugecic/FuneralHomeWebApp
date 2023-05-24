using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Kupnja
{
    public int Id { get; set; }
    public int KorisnikId { get; set; }
    public DateTime DatumKupovine { get; set; }
    public decimal UkupnaCijena { get; set; }
   
}


public static partial class DtoMapping
{
    public static Kupnja ToDto(this DomainModels.Kupnja kupnja)
        => new Kupnja()
        {
            Id = kupnja.Id,
            KorisnikId = kupnja.KorisnikId,
            DatumKupovine = kupnja.DatumKupovine,
            UkupnaCijena = kupnja.UkupnaCijena
        };

    public static DomainModels.Kupnja ToDomain(this Kupnja kupnja)
        => new DomainModels.Kupnja(
            kupnja.Id,
            kupnja.KorisnikId,
            kupnja.DatumKupovine,
            kupnja.UkupnaCijena
        );
}
