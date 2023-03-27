using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Osmrtnica : Entity<int>
{
    private string _naziv;
    private byte[]? _slika;
    private decimal _cijena;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }


    public Osmrtnica(int id, string naziv, byte[]? slika,decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }


        _naziv = naziv;
        _cijena = cijena;
        _slika = slika;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Osmrtnica osmrtnica &&
               _id == osmrtnica._id &&
               _naziv == osmrtnica._naziv &&
               _slika == osmrtnica._slika &&
               _cijena == osmrtnica._cijena;


    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _slika, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}