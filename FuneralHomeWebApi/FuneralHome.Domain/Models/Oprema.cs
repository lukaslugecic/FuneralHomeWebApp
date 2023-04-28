using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Oprema : AggregateRoot<int>
{
    private string _naziv;
    private byte[]? _slika;
    private int _kolicinaNaSkladistu;
    private decimal _cijena;
    private int _vrstaOpremeId;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public int KolicinaNaSkladistu { get => _kolicinaNaSkladistu; set => _kolicinaNaSkladistu = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
    public int VrstaOpremeId { get => _vrstaOpremeId; set => _vrstaOpremeId = value; }


    public Oprema(int id, string naziv, int vrstaOpremeId, byte[]? slika, int kolicinaNaSkladistu, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }


        _naziv = naziv;
        _kolicinaNaSkladistu = kolicinaNaSkladistu;
        _cijena = cijena;
        _slika = slika;
        _vrstaOpremeId = vrstaOpremeId;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Oprema oprema &&
               _id == oprema._id &&
               _naziv == oprema._naziv &&
               _slika == oprema._slika &&
               _kolicinaNaSkladistu == oprema._kolicinaNaSkladistu &&
               _cijena == oprema._cijena &&
               _vrstaOpremeId == oprema._vrstaOpremeId;


    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _slika, _kolicinaNaSkladistu, _cijena, _vrstaOpremeId);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}