using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Glazba : Entity<int>
{
    private string _naziv;
    private string _opis;
    private string _kontakt;
    private decimal _cijena;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public string Opis { get => _opis; set => _opis = value; }
    public string Kontakt { get => _kontakt; set => _kontakt = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }


    public Glazba(int id, string naziv, string opis, string kontakt, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }
        if (string.IsNullOrEmpty(opis))
        {
            throw new ArgumentException($"'{nameof(opis)}' cannot be null or empty.", nameof(opis));
        }
        if (string.IsNullOrEmpty(kontakt))
        {
            throw new ArgumentException($"'{nameof(kontakt)}' cannot be null or empty.", nameof(kontakt));
        }


        _naziv = naziv;
        _opis = opis;
        _kontakt = kontakt;
        _cijena = cijena;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Glazba glazba &&
               _id == glazba._id &&
               _naziv == glazba._naziv &&
               _opis == glazba._opis &&
               _kontakt == glazba._kontakt &&
               _cijena == glazba._cijena;


    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _opis, _kontakt, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"),
            (() => _opis.Length <= 100, "Description lenght must be less than 100 characters"),
            (() => !string.IsNullOrEmpty(_opis.Trim()), "First name can't be null, empty, or whitespace"),
            (() => _kontakt.Length <= 50, "Contact lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_kontakt.Trim()), "First name can't be null, empty, or whitespace"));
}