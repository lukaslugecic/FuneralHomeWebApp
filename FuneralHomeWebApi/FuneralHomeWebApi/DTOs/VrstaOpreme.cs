using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class VrstaOpreme
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

}


public static partial class DtoMapping
{
    public static VrstaOpreme ToDto(this DomainModels.VrstaOpremeUsluge vrstaOpreme)
        => new VrstaOpreme()
        {
            Id = vrstaOpreme.Id,
            Naziv = vrstaOpreme.Naziv
        };

    public static DomainModels.VrstaOpremeUsluge ToDomain(this VrstaOpreme vrstaOpreme)
        => new DomainModels.VrstaOpremeUsluge(
            vrstaOpreme.Id,
            vrstaOpreme.Naziv
       );
}