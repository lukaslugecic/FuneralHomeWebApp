using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Usluga : Entity<int>
{
    private string _naziv;
    private int _vrstaUslugeId;
    private string _opis;
    private decimal _cijena;

    public string Naziv { get => _naziv; set => _naziv = value; }
    public string Opis { get => _opis; set => _opis = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
    public int VrstaUslugeId { get => _vrstaUslugeId; set => _vrstaUslugeId = value; }


    public Usluga(int id, string naziv, int vrstaUslugeId, string opis, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }


        _naziv = naziv;
        _opis = opis;
        _cijena = cijena;
        _vrstaUslugeId = vrstaUslugeId;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
               obj is Usluga usluga &&
               _id == usluga._id &&
               _naziv == usluga._naziv &&
               _vrstaUslugeId == usluga._vrstaUslugeId &&
               _opis == usluga._opis &&
               _cijena == usluga._cijena;
              

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _vrstaUslugeId, _opis, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"),
            (() => _opis.Length <= 100, "Description lenght must be less than 100 characters"),
            (() => !string.IsNullOrEmpty(_opis.Trim()), "Description can't be null, empty, or whitespace"));
}