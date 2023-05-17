using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class OpremaUsluga : AggregateRoot<int>
{
    
    private string _naziv;
    private byte[]? _slika;
    private int? _zaliha;
    private string? _opis;
    private decimal _cijena;

    private int _jedinicaMjereId;
    private int _vrstaOpremeUslugeId;
    private string _vrstaOpremeUslugeNaziv;


    public string Naziv { get => _naziv; set => _naziv = value; }
    public byte[]? Slika { get => _slika; set => _slika = value; }
    public int? Zaliha { get => _zaliha; set => _zaliha = value; }
    public string? Opis { get => _opis; set => _opis = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }
    public int VrstaOpremeUslugeId { get => _vrstaOpremeUslugeId; set => _vrstaOpremeUslugeId = value; }
    public int JedinicaMjereId { get => _jedinicaMjereId; set => _jedinicaMjereId = value; }
    public string VrstaOpremeUslugeNaziv { get => _vrstaOpremeUslugeNaziv; set => _vrstaOpremeUslugeNaziv = value; }


    public OpremaUsluga(int id, int vrstaOpremeId, string vrstaOpremeNaziv, int jedinicnaMjeraId, string naziv,
        byte[]? slika, int? zaliha, string? opis, decimal cijena) : base(id)
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
        _cijena = cijena;
        _slika = slika;
        _zaliha = zaliha;
        _opis = opis;
        _vrstaOpremeUslugeId = vrstaOpremeId;
        _vrstaOpremeUslugeNaziv = vrstaOpremeNaziv;
        _jedinicaMjereId = jedinicnaMjeraId;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is OpremaUsluga ou &&
                _id == ou._id &&
                _naziv == ou._naziv &&
                // _slika == ou._slika &&
                _cijena == ou.Cijena &&
                _opis == ou._opis &&
                _zaliha == ou._zaliha &&
                _vrstaOpremeUslugeId == ou._vrstaOpremeUslugeId &&
                _vrstaOpremeUslugeNaziv == ou._vrstaOpremeUslugeNaziv &&
                _jedinicaMjereId == ou._jedinicaMjereId;



    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _slika, _opis, _zaliha, _cijena, _vrstaOpremeUslugeId, _jedinicaMjereId);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}