using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Usluga
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    public int VrstaUslugeId { get; set; }

    [Required(ErrorMessage = "Description can't be null")]
    [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters")]
    public string Opis { get; set; } = string.Empty;
    public decimal Cijena { get; set; }
}


public static partial class DtoMapping
{
    public static Usluga ToDto(this DomainModels.Usluga usluga)
        => new Usluga()
        {
            Id = usluga.Id,
            VrstaUslugeId = usluga.VrstaUslugeId,
            Naziv = usluga.Naziv,
            Opis = usluga.Opis,
            Cijena = usluga.Cijena
        };

    public static DomainModels.Usluga ToDomain(this Usluga usluga)
        => new DomainModels.Usluga(
            usluga.Id,
            usluga.Naziv,
            usluga.VrstaUslugeId,
            usluga.Opis,
            usluga.Cijena
       );
}