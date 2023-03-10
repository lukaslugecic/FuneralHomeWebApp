using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class Osiguranje
{
    public int Id { get; set; }
    public DateTime DatumUgovaranja { get; set; }
    public bool PlacanjeNaRate { get; set; }
}


public static partial class DtoMapping
{
    public static Osiguranje ToDto(this DbModels.Osiguranje osiguranje)
        => new Osiguranje()
        {
            Id = osiguranje.Id,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate
        };

    public static DbModels.Osiguranje ToDbModel(this Osiguranje osiguranje)
        => new DbModels.Osiguranje()
        {
            Id = osiguranje.Id,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate
        };
}