using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Osiguranje
{
    public int Id { get; set; }
    public DateTime DatumUgovaranja { get; set; }
    public bool PlacanjeNaRate { get; set; }
}


public static partial class DtoMapping
{
    public static Osiguranje ToDto(this DomainModels.Osiguranje osiguranje)
        => new Osiguranje()
        {
            Id = osiguranje.Id,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate
        };

    public static DomainModels.Osiguranje ToDomain(this Osiguranje osiguranje)
        => new DomainModels.Osiguranje(
            osiguranje.Id,
            osiguranje.DatumUgovaranja,
            osiguranje.PlacanjeNaRate
        );
}