using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Osmrtnica
{
    public int Id { get; set; }
    public byte[]? SlikaPok { get; set; }

    [Required(ErrorMessage = "Description can't be null")]
    [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters")]
    public string Opis { get; set; } = string.Empty;
    public bool ObjavaNaStranici { get; set; }

}


public static partial class DtoMapping
{
    public static Osmrtnica ToDto(this DomainModels.Osmrtnica osmrtnica)
        => new Osmrtnica()
        {
            Id = osmrtnica.Id,
            SlikaPok = osmrtnica.SlikaPok,
            Opis = osmrtnica.Opis,
            ObjavaNaStranici = osmrtnica.ObjavaNaStranici
        };

    public static DomainModels.Osmrtnica ToDomain(this Osmrtnica osmrtnica)
        => new DomainModels.Osmrtnica(
            osmrtnica.Id,
            osmrtnica.SlikaPok,
            osmrtnica.Opis,
            osmrtnica.ObjavaNaStranici
        );
}
