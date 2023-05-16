using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Osiguranje
{
    public int Id { get; set; }
    public int KorisnikId { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public DateTime DatumUgovaranja { get; set; }
    public bool PlacanjeNaRate { get; set; }
    public int BrojRata { get; set; }
    public int PaketOsiguranjaId { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string NazivPaketa { get; set; } = string.Empty;
    public decimal CijenaPaketa { get; set; }
}


public static partial class DtoMapping
{
    public static Osiguranje ToDto(this DomainModels.Osiguranje osiguranje)
        => new Osiguranje()
        {
            Id = osiguranje.Id,
            Ime = osiguranje.Ime,
            Prezime = osiguranje.Prezime,
            KorisnikId = osiguranje.KorisnikId,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate,
            BrojRata = osiguranje.BrojRata,
            PaketOsiguranjaId = osiguranje.PaketOsiguranjaId,
            NazivPaketa = osiguranje.NazivPaketa,
            CijenaPaketa = osiguranje.CijenaPaketa

        };

    public static DomainModels.Osiguranje ToDomain(this Osiguranje osiguranje)
        => new DomainModels.Osiguranje(
            osiguranje.Id,
            osiguranje.KorisnikId,
            osiguranje.Ime,
            osiguranje.Prezime,
            osiguranje.DatumUgovaranja,
            osiguranje.PlacanjeNaRate,
            osiguranje.BrojRata,
            osiguranje.PaketOsiguranjaId,
            osiguranje.NazivPaketa,
            osiguranje.CijenaPaketa
        );
}