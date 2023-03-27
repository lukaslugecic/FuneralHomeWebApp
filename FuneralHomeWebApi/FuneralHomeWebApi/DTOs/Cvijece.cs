using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Cvijece
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
    public static Cvijece ToDto(this DomainModels.Cvijece cvijece)
        => new Cvijece()
        {
            Id = cvijece.Id,
            Naziv = cvijece.Naziv,
            Slika = cvijece.Slika,
            Kolicina = cvijece.Kolicina,
            Cijena = cvijece.Cijena
        };

    public static DomainModels.Cvijece ToDbModel(this Cvijece cvijece)
        => new DomainModels.Cvijece(
            cvijece.Id,
            cvijece.Naziv,
            cvijece.Slika,
            cvijece.Kolicina,
            cvijece.Cijena
        );
}