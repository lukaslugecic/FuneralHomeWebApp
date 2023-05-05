using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Pogreb
{
    public int Id { get; set; }
    public int SmrtniSlucajId { get; set; }
    public DateTime DatumPogreba { get; set; }
    public bool Kremacija { get; set; }
    public decimal UkupnaCijena { get; set; } 
}


public static partial class DtoMapping
{
    public static Pogreb ToDto(this DomainModels.Pogreb pogreb)
        => new Pogreb()
        {
            Id = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            UkupnaCijena = pogreb.UkupnaCijena
        };

    public static DomainModels.Pogreb ToDomain(this Pogreb pogreb)
        => new DomainModels.Pogreb(
            pogreb.Id,
            pogreb.SmrtniSlucajId,
            pogreb.DatumPogreba,
            pogreb.Kremacija,
            pogreb.UkupnaCijena
        );
}
