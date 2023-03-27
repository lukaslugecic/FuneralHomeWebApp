using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Osmrtnica
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    public byte[]? Slika { get; set; }
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static Osmrtnica ToDto(this DomainModels.Osmrtnica osmrtnica)
        => new Osmrtnica()
        {
            Id = osmrtnica.Id,
            Naziv = osmrtnica.Naziv,
            Slika = osmrtnica.Slika,
            Cijena = osmrtnica.Cijena
        };

    public static DomainModels.Osmrtnica ToDomain(this Osmrtnica osmrtnica)
        => new DomainModels.Osmrtnica(
            osmrtnica.Id,
            osmrtnica.Naziv,
            osmrtnica.Slika,
            osmrtnica.Cijena
        );
}