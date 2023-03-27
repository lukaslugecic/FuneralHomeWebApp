using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Lijes : Entity<int>
{
    private string _naziv;
    private string _velicina;
    private byte[]? _slika;
    private int _kolicina;
    private decimal _cijena;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public string Velicina { get => _velicina; set => _velicina = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public int Kolicina { get => _kolicina; set => _kolicina = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }


    public Lijes(int id, string naziv, string velicina, byte[]? slika, int kolicina, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }
        if (string.IsNullOrEmpty(velicina))
        {
            throw new ArgumentException($"'{nameof(velicina)}' cannot be null or empty.", nameof(velicina));
        }


        _naziv = naziv;
        _velicina = velicina;
        _kolicina = kolicina;
        _cijena = cijena;
        _slika = slika;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Lijes lijes &&
               _id == lijes._id &&
               _naziv == lijes._naziv &&
               _velicina == lijes._velicina &&
               _slika == lijes._slika &&
               _kolicina == lijes._kolicina &&
               _cijena == lijes._cijena;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _velicina  ,_slika, _kolicina, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"),
            (() => _velicina.Length <= 50, "Size lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "Size can't be null, empty, or whitespace"));
}
