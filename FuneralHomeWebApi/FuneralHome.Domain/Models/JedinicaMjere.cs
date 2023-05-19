using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class JedinicaMjere : Entity<int>
{
    private string _naziv;
    public string Naziv { get => _naziv; set => _naziv = value; }


    public JedinicaMjere(int id, string naziv) : base(id)
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
                obj is JedinicaMjere jedinica &&
               _id == jedinica._id &&
               _naziv == jedinica._naziv;

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