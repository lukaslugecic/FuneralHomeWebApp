using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class VrstaUsluge
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

}


public static partial class DtoMapping
{
    public static VrstaUsluge ToDto(this DomainModels.VrstaUsluge vrstaUsluge)
        => new VrstaUsluge()
        {
            Id = vrstaUsluge.Id,
            Naziv = vrstaUsluge.Naziv
        };

    public static DomainModels.VrstaUsluge ToDomain(this VrstaUsluge vrstaUsluge)
        => new DomainModels.VrstaUsluge(
            vrstaUsluge.Id,
            vrstaUsluge.Naziv
       );
}