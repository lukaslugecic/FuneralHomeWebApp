using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Pogreb : AggregateRoot<int>
{
    private int _smrtniSlucajId;
    private DateTime _datumPogreba;
    private bool _kremacija;
    private DateTime _datumUgovaranja;
    private decimal _ukupnaCijena;
    private readonly List<PogrebOpremaUsluge> _pogrebOpremaUsluge;
    private SmrtniSlucaj? _smrtniSlucaj;
    private Korisnik? _korisnik;


    public int SmrtniSlucajId { get => _smrtniSlucajId; set => _smrtniSlucajId = value; }
    public DateTime DatumPogreba { get => _datumPogreba; set => _datumPogreba = value; }
    public bool Kremacija { get => _kremacija; set => _kremacija = value; }
    public DateTime DatumUgovaranja { get => _datumUgovaranja; set => _datumUgovaranja = value; }
    public decimal UkupnaCijena { get => _ukupnaCijena; set => _ukupnaCijena = value; }
    public IReadOnlyList<PogrebOpremaUsluge> PogrebOpremaUsluge => _pogrebOpremaUsluge.ToList();
    public SmrtniSlucaj? SmrtniSlucaj { get => _smrtniSlucaj; set => _smrtniSlucaj = value; }
    public Korisnik? Korisnik { get => _korisnik; set => _korisnik = value; }


    public Pogreb(int id, int smrtniSlucajId, DateTime datumPogreba, bool kremacija, DateTime datumUgovaranja, decimal ukupnaCijena,
        Korisnik? korisnik = null,
        SmrtniSlucaj? smrtniSlucaj = null,
        IEnumerable<PogrebOpremaUsluge>? pogrebOpremaUsluga = null
        ) : base(id)
    {
        _smrtniSlucajId = smrtniSlucajId;
        _datumPogreba = datumPogreba;
        _kremacija = kremacija;
        _korisnik = korisnik;
        _datumUgovaranja = datumUgovaranja;
        _ukupnaCijena = ukupnaCijena;
        _smrtniSlucaj = smrtniSlucaj;
        _pogrebOpremaUsluge = pogrebOpremaUsluga?.ToList() ?? new List<PogrebOpremaUsluge>();
    }


    public bool AddUkupnaCijena(decimal cijena)
    {
        if (cijena < 0)
            return false;
        _ukupnaCijena += cijena;
        return true;
    }

    public bool AddOprema(OpremaUsluga opremaUsluga, int kolicina)
    {
        // provjeri da li je oprema vec dodana, ako je, zbroji kolicine
        var pogrebOprema = _pogrebOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Equals(opremaUsluga));
        if (pogrebOprema is null)
        {
            _pogrebOpremaUsluge.Add(new PogrebOpremaUsluge(opremaUsluga, kolicina, opremaUsluga.Cijena));
            _ukupnaCijena += opremaUsluga.Cijena * kolicina;
            return true;
        }
        pogrebOprema.Kolicina += kolicina;
        // povecaj ukupnu cijenu
        _ukupnaCijena += opremaUsluga.Cijena * kolicina;
        return true;
    }

    public bool AddOprema(PogrebOpremaUsluge pogrebOpremaUsluga)
    {
        return AddOprema(pogrebOpremaUsluga.OpremaUsluga, pogrebOpremaUsluga.Kolicina);
    }

    public bool IncrementOpremaUsluga(int opremaUslugaId)
    {
        var pogrebOpremaUsluga = _pogrebOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Id == opremaUslugaId);
        if (pogrebOpremaUsluga is null)
            return false;
        pogrebOpremaUsluga.Kolicina++;
        _ukupnaCijena += pogrebOpremaUsluga.OpremaUsluga.Cijena;
        return true;
    }

    public bool DecrementOpremaUsluga(int opremaUslugaId)
    {
        var pogrebOpremaUsluga = _pogrebOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Id == opremaUslugaId);
        if (pogrebOpremaUsluga is null)
            return false;
        pogrebOpremaUsluga.Kolicina--;
        _ukupnaCijena -= pogrebOpremaUsluga.Cijena;
        if (pogrebOpremaUsluga.Kolicina == 0)
            _pogrebOpremaUsluge.Remove(pogrebOpremaUsluga);
        return true;
    }

    public bool RemoveOpremaUsluga(PogrebOpremaUsluge pogrebOprema)
    {
        return _pogrebOpremaUsluge.Remove(pogrebOprema);
    }

    public bool RemoveOpremaUsluga(OpremaUsluga oprema)
    {
        var pogrebOpremaUsluga = _pogrebOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Equals(oprema));
        // smanji ukupnu cijenu
        if (pogrebOpremaUsluga is not null && _pogrebOpremaUsluge.Remove(pogrebOpremaUsluga))
        {
            _ukupnaCijena -= pogrebOpremaUsluga.Cijena * pogrebOpremaUsluga.Kolicina;
            return true;
        }
        return false;
    }

   

    public void CalculateUkupnaCijena()
    {
        _ukupnaCijena = 0;
        foreach (var pogrebOprema in _pogrebOpremaUsluge)
        {
            _ukupnaCijena += pogrebOprema.Cijena * pogrebOprema.Kolicina;
        }
    }


    public void CalculateUkupnaCijena(decimal popust, string paket)
    {
        _ukupnaCijena = 0;
        foreach (var pogrebOprema in _pogrebOpremaUsluge)
        {
            if(paket.Equals("Oprema") && pogrebOprema.OpremaUsluga.JeOprema)
                _ukupnaCijena += pogrebOprema.OpremaUsluga.Cijena * pogrebOprema.Kolicina * popust;
            else if(paket.Equals("Usluge") && !pogrebOprema.OpremaUsluga.JeOprema)
                _ukupnaCijena += pogrebOprema.OpremaUsluga.Cijena * pogrebOprema.Kolicina * popust;
            else if(paket.Equals("Potpuni"))
                _ukupnaCijena += pogrebOprema.OpremaUsluga.Cijena * pogrebOprema.Kolicina * popust;
            else
                _ukupnaCijena += pogrebOprema.OpremaUsluga.Cijena * pogrebOprema.Kolicina;
        }
    }


    public void CalculateDiscount(Osiguranje osiguranje)
    {
        /*
         * postotak popusta se racuna kao
         * broj rata - (danasnji datum - datum ugovaranja).broj mjeseci
         * kroz broj rata
         */
        var brojMjeseci = (SmrtniSlucaj?.DatumSmrtiPok - osiguranje.DatumUgovaranja)?.Days / 30 ??
                          (DateTime.Now - osiguranje.DatumUgovaranja).Days / 30;
        var brojNeOtplacenihRata = osiguranje.BrojRata - brojMjeseci;
        if(brojNeOtplacenihRata < 0)
            brojNeOtplacenihRata = 0;
        var postotak = (decimal) brojNeOtplacenihRata / osiguranje.BrojRata;
        CalculateUkupnaCijena(postotak, osiguranje.NazivPaketa);
    }


    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Pogreb pogreb &&
               _id == pogreb._id &&
               _smrtniSlucajId == pogreb._smrtniSlucajId &&
               _datumPogreba == pogreb._datumPogreba &&
               _kremacija == pogreb._kremacija &&
               _datumUgovaranja == pogreb._datumUgovaranja &&
               _ukupnaCijena == pogreb._ukupnaCijena &&
               _pogrebOpremaUsluge.SequenceEqual(pogreb._pogrebOpremaUsluge);
               //&& _smrtniSlucaj.Equals(pogreb.SmrtniSlucaj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _smrtniSlucajId ,_datumPogreba ,_kremacija, _datumUgovaranja, _ukupnaCijena, _pogrebOpremaUsluge);
    }

    public override Result IsValid()
        => Validation.Validate();
}