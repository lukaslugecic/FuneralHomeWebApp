using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PogrebSmrtniSlucaj
{
    public int Id { get; set; }
    public string ImePok { get; set; } = string.Empty;
    public string PrezimePok { get; set; } = string.Empty;
    public DateTime DatumPogreba { get; set; }
    public bool Kremacija { get; set; }

    //public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static PogrebSmrtniSlucaj ToDto(this DomainModels.PogrebSmrtniSlucaj pogreb)
        => new PogrebSmrtniSlucaj()
        {
            Id = pogreb.Id,
            ImePok = pogreb.ImePok,
            PrezimePok = pogreb.PrezimePok,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            //Cijena = pogreb.Cijena
        };

    public static DomainModels.PogrebSmrtniSlucaj ToDomain(this PogrebSmrtniSlucaj pogreb)
        => new DomainModels.PogrebSmrtniSlucaj(
            pogreb.Id,
            pogreb.ImePok,
            pogreb.PrezimePok,
            pogreb.DatumPogreba,
            pogreb.Kremacija
           // pogreb.Cijena
        );
}
