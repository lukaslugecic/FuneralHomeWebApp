using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PogrebSmrtniSlucaj : AggregateRoot<int>
{
    private string _imePok;
    private string _prezimePok;
    private DateTime _datumPogreba;
    private bool _kremacija;
    // private decimal _cijena;
    private string _ime;
    private string _prezime;
    
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    public string ImePok { get => _imePok; set => _imePok = value; }
    public string PrezimePok { get => _prezimePok; set => _prezimePok = value; }
    //public decimal Cijena { get => _cijena; set => _cijena = value; }

    public string Ime { get => _ime; set => _ime = value; }
    public string Prezime { get => _prezime; set => _prezime = value; }


    public PogrebSmrtniSlucaj(int id, string imePok, string prezimePok, DateTime datumPogreba, bool kremacija, string ime, string prezime) : base(id)
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

        _datumPogreba = datumPogreba;
        _kremacija = kremacija;
        //_cijena = cijena;
        _imePok = imePok;
        _prezimePok = prezimePok;
        _ime = ime;
        _prezime = prezime;
    }


    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PogrebSmrtniSlucaj pogreb &&
               _id == pogreb._id &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija &&
               _imePok == pogreb._imePok &&
               _prezimePok == pogreb._prezimePok;
               //_cijena == pogreb._cijena;
               
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _imePok, _prezimePok, _datumPogreba, _kremacija);
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