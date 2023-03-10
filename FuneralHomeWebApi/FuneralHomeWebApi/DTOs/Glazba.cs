using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class Glazba
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description can't be null")]
    [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters")]
    public string Opis { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact can't be null")]
    [StringLength(50, ErrorMessage = "Contact can't be longer than 50 characters")]
    public string Kontakt { get; set; } = string.Empty;

    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static Glazba ToDto(this DbModels.Glazba glazba)
        => new Glazba()
        {
            Id = glazba.Id,
            Naziv = glazba.Naziv,
            Opis = glazba.Opis,
            Kontakt = glazba.Kontakt,
            Cijena = glazba.Cijena
        };

    public static DbModels.Glazba ToDbModel(this Glazba glazba)
        => new DbModels.Glazba()
        {
            Id = glazba.Id,
            Naziv = glazba.Naziv,
            Opis = glazba.Opis,
            Kontakt = glazba.Kontakt,
            Cijena = glazba.Cijena
        };
}