using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class VrstaOpremeUsluge : Entity<int>
{
    private string _naziv;
    private bool _jeOprema;
    public string Naziv { get => _naziv; set => _naziv = value; }
    public bool JeOprema { get => _jeOprema; set => _jeOprema = value; }



    public VrstaOpremeUsluge(int id, string naziv, bool jeOprema) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }
        _naziv = naziv;
        _jeOprema = jeOprema;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is VrstaOpremeUsluge vrsta &&
               _id == vrsta._id &&
               _naziv == vrsta._naziv &&
               _jeOprema == vrsta._jeOprema;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _jeOprema);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}