using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class NadgrobniZnak
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    public byte[]? Slika { get; set; }
    public int Kolicina { get; set; }
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static NadgrobniZnak ToDto(this DbModels.NadgrobniZnak znak)
        => new NadgrobniZnak()
        {
            Id = znak.Id,
            Naziv = znak.Naziv,
            Slika = znak.Slika,
            Kolicina = znak.Kolicina,
            Cijena = znak.Cijena
        };

    public static DbModels.NadgrobniZnak ToDbModel(this NadgrobniZnak znak)
        => new DbModels.NadgrobniZnak()
        {
            Id = znak.Id,
            Naziv = znak.Naziv,
            Slika = znak.Slika,
            Kolicina = znak.Kolicina,
            Cijena = znak.Cijena
        };
}