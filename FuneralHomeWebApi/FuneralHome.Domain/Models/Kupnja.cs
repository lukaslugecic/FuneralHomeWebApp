using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Kupnja : AggregateRoot<int>
{
    private int _korisnikId;
    private DateTime _datumKupovine;
    private decimal _ukupnaCijena;
    private readonly List<KupnjaOpremaUsluge> _kupnjaOpremaUsluge;
    private Korisnik? _korisnik;


    public int KorisnikId { get => _korisnikId; set => _korisnikId = value; }
    public DateTime DatumKupovine { get => _datumKupovine; set => _datumKupovine = value; }
    public decimal UkupnaCijena { get => _ukupnaCijena; set => _ukupnaCijena = value; }
    public IReadOnlyList<KupnjaOpremaUsluge> KupnjaOpremaUsluge => _kupnjaOpremaUsluge.ToList();
    public Korisnik? Korisnik { get => _korisnik; set => _korisnik = value; }


    public Kupnja(int id, int korisnikId, DateTime datumKupnje, decimal ukupnaCijena,
        Korisnik? korisnik = null,
        IEnumerable<KupnjaOpremaUsluge>? kupnjaOpremaUsluge = null
        ) : base(id)
    {
        _korisnikId = korisnikId;
        _datumKupovine = datumKupnje;
        _ukupnaCijena = ukupnaCijena;
        _korisnik = korisnik;
        _kupnjaOpremaUsluge = kupnjaOpremaUsluge?.ToList() ?? new List<KupnjaOpremaUsluge>();
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
        var kupnjaOprema = _kupnjaOpremaUsluge.FirstOrDefault(ko => ko.OpremaUsluga.Equals(opremaUsluga));
        if (kupnjaOprema is null)
        {
            _kupnjaOpremaUsluge.Add(new KupnjaOpremaUsluge(opremaUsluga, kolicina, opremaUsluga.Cijena));
            _ukupnaCijena += opremaUsluga.Cijena * kolicina;
            return true;
        }
        kupnjaOprema.Kolicina += kolicina;
        // povecaj ukupnu cijenu
        _ukupnaCijena += opremaUsluga.Cijena * kolicina;
        return true;
    }

    public bool AddOprema(KupnjaOpremaUsluge kupnjaOpremaUsluga)
    {
        return AddOprema(kupnjaOpremaUsluga.OpremaUsluga, kupnjaOpremaUsluga.Kolicina);
    }

    public bool IncrementOpremaUsluga(int opremaUslugaId)
    {
        var pogrebOpremaUsluga = _kupnjaOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Id == opremaUslugaId);
        if (pogrebOpremaUsluga is null)
            return false;
        pogrebOpremaUsluga.Kolicina++;
        _ukupnaCijena += pogrebOpremaUsluga.OpremaUsluga.Cijena;
        return true;
    }

    public bool DecrementOpremaUsluga(int opremaUslugaId)
    {
        var pogrebOpremaUsluga =  _kupnjaOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Id == opremaUslugaId);
        if (pogrebOpremaUsluga is null)
            return false;
        pogrebOpremaUsluga.Kolicina--;
        _ukupnaCijena -= pogrebOpremaUsluga.Cijena;
        if (pogrebOpremaUsluga.Kolicina == 0)
            _kupnjaOpremaUsluge.Remove(pogrebOpremaUsluga);
        return true;
    }

    public bool RemoveOpremaUsluga(KupnjaOpremaUsluge kupnjaOprema)
    {
        return _kupnjaOpremaUsluge.Remove(kupnjaOprema);
    }

    public bool RemoveOpremaUsluga(OpremaUsluga oprema)
    {
        var pogrebOpremaUsluga = _kupnjaOpremaUsluge.FirstOrDefault(po => po.OpremaUsluga.Equals(oprema));
        // smanji ukupnu cijenu
        if (pogrebOpremaUsluga is not null && _kupnjaOpremaUsluge.Remove(pogrebOpremaUsluga))
        {
            _ukupnaCijena -= pogrebOpremaUsluga.Cijena * pogrebOpremaUsluga.Kolicina;
            return true;
        }
        return false;
    }



    public void CalculateUkupnaCijena()
    {
        _ukupnaCijena = 0;
        foreach (var pogrebOprema in _kupnjaOpremaUsluge)
        {
            _ukupnaCijena += pogrebOprema.Cijena * pogrebOprema.Kolicina;
        }
    }



    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Kupnja kupnja &&
                _id == kupnja._id &&
                _korisnikId == kupnja._korisnikId &&
                _datumKupovine == kupnja._datumKupovine &&
                _ukupnaCijena == kupnja._ukupnaCijena &&
                _kupnjaOpremaUsluge.SequenceEqual(kupnja._kupnjaOpremaUsluge);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _korisnikId, _datumKupovine, _ukupnaCijena, _kupnjaOpremaUsluge);
    }

    public override Result IsValid()
        => Validation.Validate();
}