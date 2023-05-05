using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Pogreb : AggregateRoot<int>
{
    private int _smrtniSlucajId;
    private DateTime _datumPogreba;
    private bool _kremacija;
    private decimal _ukupnaCijena;
    private readonly List<PogrebOprema> _pogrebOprema;
    private readonly List<Usluga> _pogrebUsluga;
    private SmrtniSlucaj? _smrtniSlucaj; // readonly?
    private Korisnik? _korisnik;

    public int SmrtniSlucajId { get => _smrtniSlucajId; set => _smrtniSlucajId = value; }
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    public decimal UkupnaCijena { get => _ukupnaCijena; set => _ukupnaCijena = value; }
    public IReadOnlyList<PogrebOprema> PogrebOprema => _pogrebOprema.ToList();
    public IReadOnlyList<Usluga> PogrebUsluga => _pogrebUsluga.ToList();
    public SmrtniSlucaj? SmrtniSlucaj { get => _smrtniSlucaj; set => _smrtniSlucaj = value;}
    public Korisnik? Korisnik { get => _korisnik; set => _korisnik = value; }
    

    public Pogreb(int id, int smrtniSlucajId, DateTime datumPogreba, bool kremacija, decimal ukupnaCijena,
        Korisnik? korisnik = null,
        SmrtniSlucaj? smrtniSlucaj = null,
        IEnumerable<PogrebOprema>? pogrebOprema = null,
        IEnumerable<Usluga>? pogrebUsluga = null) : base(id)
    {
        _smrtniSlucajId = smrtniSlucajId;
        _datumPogreba = datumPogreba;
        _kremacija = kremacija;
        _korisnik = korisnik;
        _ukupnaCijena = ukupnaCijena;
        _smrtniSlucaj = smrtniSlucaj;
        _pogrebOprema = pogrebOprema?.ToList() ?? new List<PogrebOprema>();
        _pogrebUsluga = pogrebUsluga?.ToList() ?? new List<Usluga>();
    }

    public bool AddOprema(Oprema oprema, int kolicina)
    {
        var PogrebOprema = new PogrebOprema(oprema, kolicina);
        _pogrebOprema.Add(PogrebOprema);
        return true;
    }

    public bool AddOprema(PogrebOprema pogrebOprema)
    {
        return AddOprema(pogrebOprema.Oprema, pogrebOprema.Kolicina);
    }

    public bool IncrementOprema(int opremaId)
    {
        var pogrebOprema = _pogrebOprema.FirstOrDefault(po => po.Oprema.Id == opremaId);
        if (pogrebOprema is null)
            return false;
        pogrebOprema.Kolicina++;
        return true;
    }

    public bool DecrementOprema(int opremaId)
    {
        var pogrebOprema = _pogrebOprema.FirstOrDefault(po => po.Oprema.Id == opremaId);
        if (pogrebOprema is null)
            return false;
        pogrebOprema.Kolicina--;
        if (pogrebOprema.Kolicina == 0)
            _pogrebOprema.Remove(pogrebOprema);
        return true;
    }

    public bool RemoveOprema(PogrebOprema pogrebOprema)
    {
        return _pogrebOprema.Remove(pogrebOprema);
    }

    public bool RemoveOprema(Oprema oprema)
    {
        var pogrebOprema = _pogrebOprema.FirstOrDefault(po => po.Oprema.Equals(oprema));
        return pogrebOprema != null
            && _pogrebOprema.Remove(pogrebOprema);
    }

    public bool AddUsluga(Usluga usluga)
    {
        _pogrebUsluga.Add(usluga);
        return true;
    }

    public bool RemoveUsluga(Usluga usluga)
    {
        return _pogrebUsluga.Remove(usluga);
    }


    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Pogreb pogreb &&
               _id == pogreb._id &&
               _smrtniSlucajId == pogreb._smrtniSlucajId &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija &&
               _ukupnaCijena == pogreb._ukupnaCijena &&
               _pogrebOprema.SequenceEqual(pogreb._pogrebOprema) &&
               _pogrebUsluga.SequenceEqual(pogreb._pogrebUsluga);
               //&& _smrtniSlucaj.Equals(pogreb.SmrtniSlucaj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _smrtniSlucajId ,_datumPogreba ,_kremacija, _ukupnaCijena, _pogrebOprema, _pogrebUsluga);
    }

    public override Result IsValid()
        => Validation.Validate();
}