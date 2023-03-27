using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Urna
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
    public static Urna ToDto(this DomainModels.Urna urna)
        => new Urna()
        {
            Id = urna.Id,
            Naziv = urna.Naziv,
            Slika = urna.Slika,
            Kolicina = urna.Kolicina,
            Cijena = urna.Cijena
        };

    public static DomainModels.Urna ToDomain(this Urna urna)
        => new DomainModels.Urna(
            urna.Id,
            urna.Naziv,
            urna.Slika,
            urna.Kolicina,
            urna.Cijena
        );
}