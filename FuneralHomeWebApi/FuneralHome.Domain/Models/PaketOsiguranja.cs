using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PaketOsiguranja : Entity<int>
{

    private string _naziv;
    private decimal _cijena;

    public string Naziv { get => _naziv; set => _naziv = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
  

    public PaketOsiguranja(int id, string naziv, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }

        _naziv = naziv;
        _cijena = cijena;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PaketOsiguranja paket &&
                _id == paket._id &&
                _naziv == paket._naziv &&
                _cijena == paket._cijena;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}
