using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Pogreb : AggregateRoot<int>
{
    private int _smrtniSlucajId;
    private DateTime _datumPogreba;
    private bool _kremacija;
    private int? _urnaId;
    private int? _lijesId;
    private int? _cvijeceId;
    private int? _nadgrobniZnakId;
    private int? _glazbaId;
    private bool _snimanje;
    private bool _branitelj;
    private bool _golubica;
    private decimal _ukupnaCijena;

    public int SmrtniSlucajId { get => _smrtniSlucajId; set => _smrtniSlucajId = value; }
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    public int? UrnaId { get => _urnaId; set => _urnaId = value; }
    public int? LijesId { get => _lijesId; set => _lijesId = value; }
    public int? CvijeceId { get => _cvijeceId; set => _cvijeceId = value; }
    public int? NadgrobniZnakId { get => _nadgrobniZnakId; set => _nadgrobniZnakId = value; }
    public int? GlazbaId { get => _glazbaId; set => _glazbaId = value; }
    public bool Snimanje { get => _snimanje; set => _snimanje = value; }
    public bool Branitelj { get => _branitelj; set => _branitelj = value; }
    public bool Golubica { get => _golubica; set => _golubica = value; }
    public decimal UkupnaCijena { get => _ukupnaCijena; set => _ukupnaCijena = value; }


    public Pogreb(int id, int smrtnislucajId, DateTime datumPogreba, bool kremacija,
        int? urnaId, int? lijesId, int? cvijeceId, int? nadgrobniZnakId, int? glazbaId, bool snimanje,
        bool branitelj, bool golubica, decimal ukupnaCijena) : base(id)
    {


        _smrtniSlucajId = smrtnislucajId;
        _datumPogreba = datumPogreba;
        _kremacija = kremacija;
        _urnaId = urnaId;
        _lijesId = lijesId;
        _cvijeceId = cvijeceId;
        _nadgrobniZnakId = nadgrobniZnakId;
        _glazbaId = glazbaId;
        _snimanje = snimanje;
        _branitelj = branitelj;
        _golubica = golubica;
        _ukupnaCijena = ukupnaCijena;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Pogreb pogreb &&
               _id == pogreb._id &&
               _smrtniSlucajId == pogreb._smrtniSlucajId &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija &&
               _urnaId == pogreb._urnaId &&
               _lijesId == pogreb._lijesId &&
               _nadgrobniZnakId == pogreb._nadgrobniZnakId &&
               _glazbaId == pogreb._glazbaId &&
               _branitelj == pogreb._branitelj &&
               _golubica == pogreb._golubica &&
               _ukupnaCijena == pogreb._ukupnaCijena;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _smrtniSlucajId, _datumPogreba ,_kremacija, _ukupnaCijena);
    }

    public override Result IsValid()
        => Validation.Validate();
}