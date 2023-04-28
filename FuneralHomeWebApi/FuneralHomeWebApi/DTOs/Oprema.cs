using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Oprema
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    public int VrstaOpremeId { get; set; }

    public byte[]? Slika { get; set; }
    public int ZalihaOpreme { get; set; }
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static Oprema ToDto(this DomainModels.Oprema oprema)
        => new Oprema()
        {
            Id = oprema.Id,
            VrstaOpremeId = oprema.VrstaOpremeId,
            Naziv = oprema.Naziv,
            Slika = oprema.Slika,
            ZalihaOpreme = oprema.ZalihaOpreme,
            Cijena = oprema.Cijena
        };

    public static DomainModels.Oprema ToDomain(this Oprema oprema)
        => new DomainModels.Oprema(
            oprema.Id,
            oprema.Naziv,
            oprema.VrstaOpremeId,
            oprema.Slika,
            oprema.ZalihaOpreme,
            oprema.Cijena
       );
}