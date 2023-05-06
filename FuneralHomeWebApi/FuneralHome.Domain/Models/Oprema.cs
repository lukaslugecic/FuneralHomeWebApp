using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Oprema : AggregateRoot<int>
{
    private string _naziv;
    private byte[]? _slika;
    private int _zalihaOpreme;
    private decimal _cijena;
    private int _vrstaOpremeId;
    private string _vrstaOpremeNaziv;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public int ZalihaOpreme { get => _zalihaOpreme; set => _zalihaOpreme = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
    public int VrstaOpremeId { get => _vrstaOpremeId; set => _vrstaOpremeId = value; }

    public string VrstaOpremeNaziv { get => _vrstaOpremeNaziv; set => _vrstaOpremeNaziv = value; }


    public Oprema(int id, string naziv, int vrstaOpremeId, string vrstaOpremeNaziv, byte[]? slika, int zalihaOpreme, decimal cijena) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }

        if (string.IsNullOrEmpty(vrstaOpremeNaziv))
        {
            throw new ArgumentException($"'{nameof(vrstaOpremeNaziv)}' cannot be null or empty.", nameof(vrstaOpremeNaziv));
        }


        _naziv = naziv;
        _zalihaOpreme = zalihaOpreme;
        _cijena = cijena;
        _slika = slika;
        _vrstaOpremeId = vrstaOpremeId;
        _vrstaOpremeNaziv = vrstaOpremeNaziv;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Oprema oprema &&
               _id == oprema._id &&
               _naziv == oprema._naziv &&
              // _slika == oprema._slika &&
               _zalihaOpreme == oprema._zalihaOpreme &&
               _cijena == oprema._cijena &&
               _vrstaOpremeId == oprema._vrstaOpremeId &&
               _vrstaOpremeNaziv == oprema._vrstaOpremeNaziv;


    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _slika, _zalihaOpreme, _cijena, _vrstaOpremeId, _vrstaOpremeNaziv);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}