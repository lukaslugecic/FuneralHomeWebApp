using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class AuthKorisnik
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
    [StringLength(100, ErrorMessage = "Password can't be longer than 100 characters")]
    public string Lozinka { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type can't be null")]
    [StringLength(1, ErrorMessage = "Type can't be longer than 1 character")]
    public string VrstaKorisnika { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}


public static partial class DtoMapping
{
    public static AuthKorisnik ToDto(this DomainModels.Korisnik korisnik, string token)
        => new AuthKorisnik()
        {
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            DatumRodenja = korisnik.DatumRodenja,
            Adresa = korisnik.Adresa,
            Oib = korisnik.Oib,
            Mail = korisnik.Mail,
            Lozinka = korisnik.Lozinka,
            VrstaKorisnika = korisnik.VrstaKorisnika,
            Token = token
        };

    public static DomainModels.Korisnik ToDomain(this AuthKorisnik korisnik)
        => new DomainModels.Korisnik(
            korisnik.Id,
            korisnik.Ime,
            korisnik.Prezime,
            korisnik.DatumRodenja,
            korisnik.Adresa,
            korisnik.Oib,
            korisnik.Mail,
            korisnik.Lozinka,
            korisnik.VrstaKorisnika
        );
}
