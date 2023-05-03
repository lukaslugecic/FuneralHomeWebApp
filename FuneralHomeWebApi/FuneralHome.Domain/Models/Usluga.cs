using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Usluga : AggregateRoot<int>
{
    private string _naziv;
    private int _vrstaUslugeId;
    private string _vrstaUslugeNaziv;
    private string _opis;
    private decimal _cijena;

    public string Naziv { get => _naziv; set => _naziv = value; }
    public string Opis { get => _opis; set => _opis = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
    public int VrstaUslugeId { get => _vrstaUslugeId; set => _vrstaUslugeId = value; }
    public string VrstaUslugeNaziv { get => _vrstaUslugeNaziv; set => _vrstaUslugeNaziv = value; }


    public Usluga(int id, string naziv, int vrstaUslugeId, string vrstaUslugeNaziv, string opis, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }

        if(string.IsNullOrEmpty(vrstaUslugeNaziv))
        {
            throw new ArgumentException($"'{nameof(vrstaUslugeNaziv)}' cannot be null or empty.", nameof(vrstaUslugeNaziv));
        }
        


        _naziv = naziv;
        _opis = opis;
        _cijena = cijena;
        _vrstaUslugeId = vrstaUslugeId;
        _vrstaUslugeNaziv = vrstaUslugeNaziv;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
               obj is Usluga usluga &&
               _id == usluga._id &&
               _naziv == usluga._naziv &&
               _vrstaUslugeId == usluga._vrstaUslugeId &&
               _vrstaUslugeNaziv == usluga._vrstaUslugeNaziv &&
               _opis == usluga._opis &&
               _cijena == usluga._cijena;
              

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _vrstaUslugeId, _vrstaUslugeNaziv ,_opis, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"),
            (() => _opis.Length <= 100, "Description lenght must be less than 100 characters"),
            (() => !string.IsNullOrEmpty(_opis.Trim()), "Description can't be null, empty, or whitespace"));
}