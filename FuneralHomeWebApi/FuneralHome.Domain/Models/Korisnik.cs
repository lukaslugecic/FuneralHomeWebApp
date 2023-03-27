using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Korisnik : AggregateRoot<int>
{
    private string _ime;
    private string _prezime;
    private DateTime _datumRodenja;
    private string _adresa;
    private string _oib;
    private string _mail;
    private string _lozinka;
    private string _vrstaKorisnika;

    public string Ime { get => _ime; set => _ime = value; }
    public string Prezime { get => _prezime; set => _prezime = value; }
    public DateTime DatumRodenja { get => _datumRodenja; set => _datumRodenja = value; }
    public string Adresa { get => _adresa; set => _adresa = value; }
    public string Oib { get => _oib; set => _oib = value; }
    public string Mail { get => _mail; set => _mail = value; }
    public string Lozinka { get => _lozinka; set => _lozinka = value; }
    public string VrstaKorisnika { get => _vrstaKorisnika; set => _vrstaKorisnika = value; }
    

    public Korisnik(int id, string ime, string prezime,
        DateTime datumRodenja, string adresa, string oib,
        string mail, string lozinka, string vrstaKorisnika) : base(id)
    {
        if (string.IsNullOrEmpty(ime))
        {
            throw new ArgumentException($"'{nameof(ime)}' cannot be null or empty.", nameof(ime));
        }

        if (string.IsNullOrEmpty(prezime))
        {
            throw new ArgumentException($"'{nameof(prezime)}' cannot be null or empty.", nameof(prezime));
        }

        if (string.IsNullOrEmpty(adresa))
        {
            throw new ArgumentException($"'{nameof(adresa)}' cannot be null or empty.", nameof(adresa));
        }
        if (string.IsNullOrEmpty(oib))
        {
            throw new ArgumentException($"'{nameof(oib)}' cannot be null or empty.", nameof(oib));
        }
        if (string.IsNullOrEmpty(mail))
        {
            throw new ArgumentException($"'{nameof(mail)}' cannot be null or empty.", nameof(mail));
        }
        if (string.IsNullOrEmpty(lozinka))
        {
            throw new ArgumentException($"'{nameof(lozinka)}' cannot be null or empty.", nameof(lozinka));
        }
        if (string.IsNullOrEmpty(vrstaKorisnika))
        {
            throw new ArgumentException($"'{nameof(vrstaKorisnika)}' cannot be null or empty.", nameof(vrstaKorisnika));
        }

        _ime = ime;
        _prezime = prezime;
        _adresa = adresa;
        _datumRodenja = datumRodenja;
        _oib = oib;
        _mail = mail;
        _lozinka = lozinka;
        _vrstaKorisnika = vrstaKorisnika;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Korisnik korisnik &&
               _id == korisnik._id &&
               _ime == korisnik._ime &&
               _prezime == korisnik._prezime &&
               _datumRodenja == korisnik._datumRodenja &&
               _adresa == korisnik._adresa &&
               _oib == korisnik._oib &&
               _vrstaKorisnika == korisnik._vrstaKorisnika &&
               _mail == korisnik._mail &&
               _lozinka == korisnik._lozinka;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _ime, _prezime, _datumRodenja, _adresa, _oib, _mail, _lozinka);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _ime.Length <= 50, "First name lenght must be less than 50 characters"),
            (() => _prezime.Length <= 50, "Last name lenght must be less than 50 characters"),
            (() => _adresa.Length <= 50, "Address lenght must be less than 50 characters"),
            (() => _oib.Length <= 11, "OIB lenght must be less than 50 characters"),
            (() => _mail.Length <= 50, "E-mail address lenght must be less than 50 characters"),
            (() => _lozinka.Length <= 50, "Password lenght must be less than 50 characters"),
            (() => _vrstaKorisnika.Length <= 1, "Type lenght must be 1 character"),
            (() => !string.IsNullOrEmpty(_ime.Trim()), "First name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_prezime.Trim()), "Last name can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_adresa.Trim()), "Address can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_oib.Trim()), "OIB can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_mail.Trim()), "E-mail address can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_lozinka.Trim()), "Password can't be null, empty, or whitespace"),
            (() => !string.IsNullOrEmpty(_vrstaKorisnika.Trim()), "Type can't be null, empty, or whitespace")
            );
}