using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PogrebSmrtniSlucaj : AggregateRoot<int>
{
    private int _smrtniSlucajId;
    private string _imePok;
    private string _prezimePok;
    private DateTime _datumSmrti;
    private DateTime _datumPogreba;
    private bool _kremacija;
    private decimal _ukupnaCijena;
    private int _korisnikId;
    private string _ime;
    private string _prezime;
    private DateTime _datumUgovaranja;
    
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    public int SmrtniSlucajId { get => _smrtniSlucajId; set => _smrtniSlucajId = value; }
    public string ImePok { get => _imePok; set => _imePok = value; }
    public string PrezimePok { get => _prezimePok; set => _prezimePok = value; }
    public DateTime DatumSmrti { get => _datumSmrti; set => _datumSmrti = value; }
    public decimal UkupnaCijena { get => _ukupnaCijena; set => _ukupnaCijena = value; }
    public int KorisnikId { get => _korisnikId; set => _korisnikId = value; }
    public string Ime { get => _ime; set => _ime = value; }
    public string Prezime { get => _prezime; set => _prezime = value; }
    public DateTime DatumUgovaranja { get => _datumUgovaranja; set => _datumUgovaranja = value; }


    public PogrebSmrtniSlucaj(int id, int smrtniSlucajId, string imePok, string prezimePok, DateTime datumSmrti, DateTime datumPogreba, bool kremacija, decimal ukupnaCijena,
        int korisnikId, string ime, string prezime, DateTime datumUgovaranja) : base(id)
    {
        if (string.IsNullOrEmpty(imePok))
        {
            throw new ArgumentException($"'{nameof(imePok)}' cannot be null or empty.", nameof(imePok));
        }

        if (string.IsNullOrEmpty(prezimePok))
        {
            throw new ArgumentException($"'{nameof(prezimePok)}' cannot be null or empty.", nameof(prezimePok));
        }

        if (string.IsNullOrEmpty(ime))
        {
            throw new ArgumentException($"'{nameof(ime)}' cannot be null or empty.", nameof(ime));
        }

        if (string.IsNullOrEmpty(prezime))
        {
            throw new ArgumentException($"'{nameof(prezime)}' cannot be null or empty.", nameof(prezime));
        }
        
        _smrtniSlucajId = smrtniSlucajId;
        _datumSmrti = datumSmrti;
        _datumPogreba = datumPogreba;
        _kremacija = kremacija;
        _ukupnaCijena = ukupnaCijena;
        _imePok = imePok;
        _prezimePok = prezimePok;
        _korisnikId = korisnikId;
        _ime = ime;
        _prezime = prezime;
        _datumUgovaranja = datumUgovaranja;
    }


    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PogrebSmrtniSlucaj pogreb &&
               _id == pogreb._id &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija &&
               _ukupnaCijena == pogreb._ukupnaCijena &&
               _imePok == pogreb._imePok &&
               _prezimePok == pogreb._prezimePok &&
               _smrtniSlucajId == pogreb._smrtniSlucajId;
               
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _smrtniSlucajId, _imePok, _prezimePok, _datumPogreba, _kremacija, _ukupnaCijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _imePok.Length <= 50, "First name lenght must be less than 50 characters"),
            (() => _prezimePok.Length <= 50, "Last name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_imePok.Trim()), "First name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_prezimePok.Trim()), "Last name can't be null, empty, or whitespace"),
            (() => _ime.Length <= 50, "First name lenght must be less than 50 characters"),
            (() => _prezime.Length <= 50, "Last name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_ime.Trim()), "First name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_prezime.Trim()), "Last name can't be null, empty, or whitespace"))
        ;
}