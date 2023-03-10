using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class Pogreb
{
    public int Id { get; set; }
    public int SmrtniSlucajId { get; set; }
    //public DateOnly DatumPogreba { get; set; }
    public DateTime DatumPogreba { get; set; }
    public bool Kremacija { get; set; }
    public int? UrnaId { get; set; }
    public int? LijesId { get; set; }
    public int? CvijeceId { get; set; }
    public int? NadgrobniZnakId { get; set; }
    public int? GlazbaId { get; set; }
    public bool Snimanje { get; set; }
    public bool Branitelj { get; set; }
    public bool Golubica { get; set; }
    public decimal UkupnaCijena { get; set; }

}


public static partial class DtoMapping
{
    public static Pogreb ToDto(this DbModels.Pogreb pogreb)
        => new Pogreb()
        {
            Id = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            UrnaId = pogreb.UrnaId,
            LijesId = pogreb.LijesId,
            CvijeceId = pogreb.LijesId,
            NadgrobniZnakId = pogreb.NadgrobniZnakId,
            GlazbaId = pogreb.GlazbaId,
            Snimanje = pogreb.Snimanje,
            Branitelj = pogreb.Branitelj,
            Golubica = pogreb.Golubica,
            UkupnaCijena = pogreb.UkupnaCijena
        };

    public static DbModels.Pogreb ToDbModel(this Pogreb pogreb)
        => new DbModels.Pogreb()
        {
            Id = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            UrnaId = pogreb.UrnaId,
            LijesId = pogreb.LijesId,
            CvijeceId = pogreb.LijesId,
            NadgrobniZnakId = pogreb.NadgrobniZnakId,
            GlazbaId = pogreb.GlazbaId,
            Snimanje = pogreb.Snimanje,
            Branitelj = pogreb.Branitelj,
            Golubica = pogreb.Golubica,
            UkupnaCijena = pogreb.UkupnaCijena
        };
}
