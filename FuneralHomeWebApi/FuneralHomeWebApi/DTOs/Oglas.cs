using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

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
    public static Oglas ToDto(this DbModels.Oglas oglas)
        => new Oglas()
        {
            Id = oglas.Id,
            SlikaPok = oglas.SlikaPok,
            Opis = oglas.Opis,
            ObjavaNaStranici = oglas.ObjavaNaStranici
        };

    public static DbModels.Oglas ToDbModel(this Oglas oglas)
        => new DbModels.Oglas()
        {
            Id = oglas.Id,
            SlikaPok = oglas.SlikaPok,
            Opis = oglas.Opis,
            ObjavaNaStranici = oglas.ObjavaNaStranici
        };
}
