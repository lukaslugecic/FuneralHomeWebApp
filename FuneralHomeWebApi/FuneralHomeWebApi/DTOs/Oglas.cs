using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class Oglas
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
    public static Oglas ToDto(this DomainModels.Oglas oglas)
        => new Oglas()
        {
            Id = oglas.Id,
            SlikaPok = oglas.SlikaPok,
            Opis = oglas.Opis,
            ObjavaNaStranici = oglas.ObjavaNaStranici
        };

    public static DomainModels.Oglas ToDbModel(this Oglas oglas)
        => new DomainModels.Oglas(
            oglas.Id,
            oglas.SlikaPok,
            oglas.Opis,
            oglas.ObjavaNaStranici
        );
}
