using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Urna : Entity<int>
{
    private string _naziv;
    private byte[]? _slika;
    private int _kolicina;
    private decimal _cijena;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public int Kolicina { get => _kolicina; set => _kolicina = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }


    public Urna(int id, string naziv, byte[]? slika, int kolicina, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }


        _naziv = naziv;
        _kolicina = kolicina;
        _cijena = cijena;
        _slika = slika;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Urna urna &&
               _id == urna._id &&
               _naziv == urna._naziv &&
               _slika == urna._slika &&
               _kolicina == urna._kolicina &&
               _cijena == urna._cijena;


    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _slika, _kolicina, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}