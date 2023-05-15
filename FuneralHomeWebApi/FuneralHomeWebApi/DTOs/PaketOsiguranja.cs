using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class PaketOsiguranja
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;
    public decimal Cijena { get; set; }

}


public static partial class DtoMapping
{
    public static PaketOsiguranja ToDto(this DomainModels.PaketOsiguranja paket)
        => new PaketOsiguranja()
        {
            Id = paket.Id,
            Naziv = paket.Naziv,
            Cijena = paket.Cijena
        };

    public static DomainModels.PaketOsiguranja ToDomain(this PaketOsiguranja paket)
        => new DomainModels.PaketOsiguranja(
            paket.Id,
            paket.Naziv,
            paket.Cijena
       );
}