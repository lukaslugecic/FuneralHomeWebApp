using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class KupnjaOpremaUsluge : ValueObject
{
    private OpremaUsluga _opremaUsluga;
    private int _kolicina;
    private decimal _cijena;

    public OpremaUsluga OpremaUsluga { get => _opremaUsluga; set => _opremaUsluga = value; }
    public int Kolicina { get => _kolicina; set => _kolicina = value; }
    public decimal Cijena { get => _cijena; set => _cijena = value; }

    public KupnjaOpremaUsluge(OpremaUsluga opremaUsluga, int kolicina, decimal cijena)
    {
        _opremaUsluga = opremaUsluga ?? throw new ArgumentNullException(nameof(opremaUsluga));
        _kolicina = kolicina;
        _cijena = cijena;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is KupnjaOpremaUsluge pou &&
                _kolicina == pou._kolicina &&
                _opremaUsluga == pou._opremaUsluga &&
                _cijena == pou._cijena;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_opremaUsluga, _kolicina, _cijena);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _opremaUsluga != null, "Equipment or service can't be null"));
}