using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PogrebAggregate
{
    public int Id { get; set; }
    public int SmrtniSlucajId { get; set; }
    //public DateOnly DatumPogreba { get; set; }
    public DateTime DatumPogreba { get; set; }
    public bool Kremacija { get; set; }
    public IEnumerable<PogrebOprema> PogrebOprema { get; set; } = Enumerable.Empty<PogrebOprema>();
    public IEnumerable<Usluga> PogrebUsluga { get; set; } = Enumerable.Empty<Usluga>();

    public SmrtniSlucaj SmrtniSlucaj { get; set; } = new SmrtniSlucaj();
    public Korisnik Korisnik { get; set; } = new Korisnik();
}


public static partial class DtoMapping
{
    public static PogrebAggregate ToAggregateDto(this DomainModels.Pogreb pogreb)
        => new PogrebAggregate()
        {
            Id = pogreb.Id,
            //SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            PogrebOprema = pogreb.PogrebOprema == null
                            ? new List<PogrebOprema>()
                            : pogreb.PogrebOprema.Select(pa => pa.ToDto()).ToList(),
            PogrebUsluga = pogreb.PogrebUsluga == null
                            ? new List<Usluga>()
                            : pogreb.PogrebUsluga.Select(pa => pa.ToDto()).ToList(),
            SmrtniSlucaj = pogreb.SmrtniSlucaj.ToDto(),
            Korisnik = pogreb.Korisnik.ToDto()
        };

    public static DomainModels.Pogreb ToDomain(this PogrebAggregate pogreb)
        => new DomainModels.Pogreb(
            pogreb.Id,
            pogreb.SmrtniSlucajId,
            pogreb.DatumPogreba,
            pogreb.Kremacija,
            pogreb.Korisnik.ToDomain(),
            pogreb.SmrtniSlucaj.ToDomain(),
            pogreb.PogrebOprema.Select(ToDomain),
            pogreb.PogrebUsluga.Select(ToDomain)
        );
}
