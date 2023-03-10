using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class Korisnik
{
    public int Id { get; set; }
    [Required(ErrorMessage = "First name can't be null")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string Ime { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name can't be null")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string Prezime { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime DatumRodenja { get; set; }
    [Required(ErrorMessage = "Address can't be null")]
    [StringLength(50, ErrorMessage = "Address can't be longer than 50 characters")]
    public string Adresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "OIB can't be null")]
    [StringLength(11, ErrorMessage = "OIB can't be longer than 11 characters")]
    public string Oib { get; set; } = string.Empty;
    [Required(ErrorMessage = "E-mail address can't be null")]
    [StringLength(50, ErrorMessage = "E-mail address can't be longer than 50 characters")]
    public string Mail { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password can't be null")]
    [StringLength(50, ErrorMessage = "Password can't be longer than 50 characters")]
    public string Lozinka { get; set; } = string.Empty;
}


public static partial class DtoMapping
{
    public static Korisnik ToDto(this DbModels.Korisnik korisnik)
        => new Korisnik()
        {
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            DatumRodenja = korisnik.DatumRodenja,
            Adresa = korisnik.Adresa,
            Oib = korisnik.Oib,
            Mail = korisnik.Mail,
            Lozinka = korisnik.Lozinka,
        };

    public static DbModels.Korisnik ToDbModel(this Korisnik korisnik)
        => new DbModels.Korisnik()
        {
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            DatumRodenja = korisnik.DatumRodenja,
            Adresa = korisnik.Adresa,
            Oib = korisnik.Oib,
            Mail = korisnik.Mail,
            Lozinka = korisnik.Lozinka
        };
}