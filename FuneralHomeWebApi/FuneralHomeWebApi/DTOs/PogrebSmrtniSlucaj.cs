using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PogrebSmrtniSlucaj
{
    public int Id { get; set; }

    public int SmrtniSlucajId { get; set; }
    public string ImePok { get; set; } = string.Empty;
    public string PrezimePok { get; set; } = string.Empty;
    public DateTime DatumSmrti { get; set; }
    public DateTime DatumPogreba { get; set; }
    public bool Kremacija { get; set; }
    public int KorisnikId { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public decimal UkupnaCijena { get; set; }
}


public static partial class DtoMapping
{
    public static PogrebSmrtniSlucaj ToDto(this DomainModels.PogrebSmrtniSlucaj pogreb)
        => new PogrebSmrtniSlucaj()
        {
            Id = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            ImePok = pogreb.ImePok,
            PrezimePok = pogreb.PrezimePok,
            DatumSmrti = pogreb.DatumSmrti,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            UkupnaCijena = pogreb.UkupnaCijena,
            KorisnikId = pogreb.KorisnikId,
            Ime = pogreb.Ime,
            Prezime = pogreb.Prezime
        };

    public static DomainModels.PogrebSmrtniSlucaj ToDomain(this PogrebSmrtniSlucaj pogreb)
        => new DomainModels.PogrebSmrtniSlucaj(
            pogreb.Id,
            pogreb.SmrtniSlucajId,
            pogreb.ImePok,
            pogreb.PrezimePok,
            pogreb.DatumSmrti,
            pogreb.DatumPogreba,
            pogreb.Kremacija,
            pogreb.UkupnaCijena,
            pogreb.KorisnikId,
            pogreb.Ime,
            pogreb.Prezime
        );
}
