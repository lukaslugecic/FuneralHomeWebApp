using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class JedinicaMjere
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be null")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Naziv { get; set; } = string.Empty;


}


public static partial class DtoMapping
{
    public static JedinicaMjere ToDto(this DomainModels.JedinicaMjere jedinicaMjere)
        => new JedinicaMjere()
        {
            Id = jedinicaMjere.Id,
            Naziv = jedinicaMjere.Naziv
        };

    public static DomainModels.JedinicaMjere ToDomain(this JedinicaMjere jedinicaMjere)
        => new DomainModels.JedinicaMjere(
            jedinicaMjere.Id,
            jedinicaMjere.Naziv
       );
}