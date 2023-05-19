using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class VrstaOpremeUsluge
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;

    public bool JeOprema { get; set; }
    public int JedinicaMjereId { get; set; }

}


public static partial class DtoMapping
{
    public static VrstaOpremeUsluge ToDto(this DomainModels.VrstaOpremeUsluge vrstaOpremeUsluge)
        => new VrstaOpremeUsluge()
        {
            Id = vrstaOpremeUsluge.Id,
            Naziv = vrstaOpremeUsluge.Naziv,
            JeOprema = vrstaOpremeUsluge.JeOprema,
            JedinicaMjereId = vrstaOpremeUsluge.JedinicaMjereId
        };

    public static DomainModels.VrstaOpremeUsluge ToDomain(this VrstaOpremeUsluge vrstaOpremeUsluge)
        => new DomainModels.VrstaOpremeUsluge(
            vrstaOpremeUsluge.Id,
            vrstaOpremeUsluge.Naziv,
            vrstaOpremeUsluge.JeOprema,
            vrstaOpremeUsluge.JedinicaMjereId
       );
}