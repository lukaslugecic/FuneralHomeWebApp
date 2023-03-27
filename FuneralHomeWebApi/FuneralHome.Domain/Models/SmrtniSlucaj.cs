using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class SmrtniSlucaj : AggregateRoot<int>
{
    private int _korisnikId;
    private string _imePok;
    private string _prezimePok;
    private string _oibPok;
    private DateTime _datumRodenjaPok;
    private DateTime _datumSmrtiPok;

    public int KorisnikId { get => _korisnikId; set => _korisnikId = value; }
    public string ImePok { get => _imePok; set => _imePok = value; }
    public string PrezimePok { get => _prezimePok; set => _prezimePok = value; }
    public DateTime DatumRodenjaPok { get => _datumRodenjaPok; set => _datumRodenjaPok = value; }
    public DateTime DatumSmrtiPok { get => _datumSmrtiPok; set => _datumSmrtiPok = value; }
    public string Oibpok { get => _oibPok; set => _oibPok = value; }


    public SmrtniSlucaj(int id, int korisnikId ,string ime, string prezime,
        DateTime datumRodenja, DateTime datumSmrti, string oib) : base(id)
    {
        if (string.IsNullOrEmpty(ime))
        {
            throw new ArgumentException($"'{nameof(ime)}' cannot be null or empty.", nameof(ime));
        }

        if (string.IsNullOrEmpty(prezime))
        {
            throw new ArgumentException($"'{nameof(prezime)}' cannot be null or empty.", nameof(prezime));
        }
        
        if (string.IsNullOrEmpty(oib))
        {
            throw new ArgumentException($"'{nameof(oib)}' cannot be null or empty.", nameof(oib));
        }

        _korisnikId = korisnikId;
        _imePok = ime;
        _prezimePok = prezime;
        _datumRodenjaPok = datumRodenja;
        _datumSmrtiPok = datumSmrti;
        _oibPok = oib;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is SmrtniSlucaj slucaj &&
               _id == slucaj._id &&
               _imePok == slucaj._imePok &&
               _prezimePok == slucaj._prezimePok &&
               _datumRodenjaPok == slucaj._datumRodenjaPok &&
               _datumSmrtiPok == slucaj._datumSmrtiPok &&
               _oibPok == slucaj._oibPok;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _korisnikId, _imePok, _prezimePok, _oibPok ,_datumRodenjaPok, _datumSmrtiPok);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _imePok.Length <= 50, "First name lenght must be less than 50 characters"),
            (() => _prezimePok.Length <= 50, "Last name lenght must be less than 50 characters"),
            (() => _oibPok.Length <= 11, "OIB lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_imePok.Trim()), "First name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_prezimePok.Trim()), "Last name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_oibPok.Trim()), "OIB can't be null, empty, or whitespace"));
            
}