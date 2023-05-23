using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PaketOsiguranja : Entity<int>
{

    private string _naziv;

    public string Naziv { get => _naziv; set => _naziv = value; }
  

    public PaketOsiguranja(int id, string naziv) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }

        _naziv = naziv;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PaketOsiguranja paket &&
                _id == paket._id &&
                _naziv == paket._naziv;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}
