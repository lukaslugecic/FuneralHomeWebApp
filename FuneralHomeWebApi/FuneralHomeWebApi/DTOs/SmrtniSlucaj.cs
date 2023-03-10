using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;

namespace FuneralHome.DTOs;

public class SmrtniSlucaj
{
    public int Id { get; set; }
    public int KorisnikId { get; set; }
    [Required(ErrorMessage = "First name can't be null")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string ImePok { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name can't be null")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string PrezimePok { get; set; } = string.Empty;
    [Required(ErrorMessage = "OIB can't be null")]
    [StringLength(11, ErrorMessage = "OIB can't be longer than 11 characters")]
    public string Oibpok { get; set; } = string.Empty;
    //public DateOnly DatumRodenjaPok { get; set; }
    //public DateOnly DatumSmrtiPok { get; set; }
    public DateTime DatumRodenjaPok { get; set; }
    public DateTime DatumSmrtiPok { get; set; }

}


public static partial class DtoMapping
{
    public static SmrtniSlucaj ToDto(this DbModels.SmrtniSlucaj smrtniSlucaj)
        => new SmrtniSlucaj()
        {
            Id = smrtniSlucaj.Id,
            KorisnikId = smrtniSlucaj.KorisnikId,
            ImePok = smrtniSlucaj.ImePok,
            PrezimePok = smrtniSlucaj.PrezimePok,
            Oibpok = smrtniSlucaj.Oibpok,
            DatumRodenjaPok = smrtniSlucaj.DatumRodenjaPok,
            DatumSmrtiPok = smrtniSlucaj.DatumSmrtiPok
        };

    public static DbModels.SmrtniSlucaj ToDbModel(this SmrtniSlucaj smrtniSlucaj)
        => new DbModels.SmrtniSlucaj()
        {
            Id = smrtniSlucaj.Id,
            KorisnikId = smrtniSlucaj.KorisnikId,
            ImePok = smrtniSlucaj.ImePok,
            PrezimePok = smrtniSlucaj.PrezimePok,
            Oibpok = smrtniSlucaj.Oibpok,
            DatumRodenjaPok = smrtniSlucaj.DatumRodenjaPok,
            DatumSmrtiPok = smrtniSlucaj.DatumSmrtiPok
        };
}
