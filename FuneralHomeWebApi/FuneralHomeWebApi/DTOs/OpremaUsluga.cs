using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class OpremaUsluga
{
    public int Id { get; set; }
    public int VrstaOpremeUslugeId { get; set; }
    public string VrstaOpremeUslugeNaziv { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;
    public byte[]? Slika { get; set; }
    public int? Zaliha { get; set; }
    public string? Opis { get; set; } = string.Empty;
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static OpremaUsluga ToDto(this DomainModels.OpremaUsluga opremaUsluga)
        => new OpremaUsluga()
        {
            Id = opremaUsluga.Id,
            VrstaOpremeUslugeId = opremaUsluga.VrstaOpremeUslugeId,
            VrstaOpremeUslugeNaziv = opremaUsluga.VrstaOpremeUslugeNaziv,
            Naziv = opremaUsluga.Naziv,
            Slika = opremaUsluga.Slika,
            Zaliha = opremaUsluga.Zaliha,
            Opis = opremaUsluga.Opis,
            Cijena = opremaUsluga.Cijena
        };

    public static DomainModels.OpremaUsluga ToDomain(this OpremaUsluga opremaUsluga)
        => new DomainModels.OpremaUsluga(
            opremaUsluga.Id,
            opremaUsluga.VrstaOpremeUslugeId,
            opremaUsluga.VrstaOpremeUslugeNaziv,
            opremaUsluga.Naziv,
            opremaUsluga.Slika,
            opremaUsluga.Zaliha,
            opremaUsluga.Opis,
            opremaUsluga.Cijena
       );
}