using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Pogreb : AggregateRoot<int>
{
    private int _smrtniSlucajId;
    private DateTime _datumPogreba;
    private bool _kremacija;
    //private decimal _ukupnaCijena;

    public int SmrtniSlucajId { get => _smrtniSlucajId; set => _smrtniSlucajId = value; }
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    

    public Pogreb(int id, int smrtnislucajId, DateTime datumPogreba, bool kremacija) : base(id)
    {
        _smrtniSlucajId = smrtnislucajId;
        _datumPogreba = datumPogreba;
        _kremacija = kremacija;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Pogreb pogreb &&
               _id == pogreb._id &&
               _smrtniSlucajId == pogreb._smrtniSlucajId &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _smrtniSlucajId, _datumPogreba ,_kremacija);
    }

    public override Result IsValid()
        => Validation.Validate();
}