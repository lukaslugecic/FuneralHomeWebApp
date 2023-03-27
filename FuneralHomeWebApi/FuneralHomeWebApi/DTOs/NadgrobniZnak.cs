using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

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
    public static NadgrobniZnak ToDto(this DomainModels.NadgrobniZnak znak)
        => new NadgrobniZnak()
        {
            Id = znak.Id,
            Naziv = znak.Naziv,
            Slika = znak.Slika,
            Kolicina = znak.Kolicina,
            Cijena = znak.Cijena
        };

    public static DomainModels.NadgrobniZnak ToDbModel(this NadgrobniZnak znak)
        => new DomainModels.NadgrobniZnak(
            znak.Id,
            znak.Naziv,
            znak.Slika,
            znak.Kolicina,
            znak.Cijena
        );
}