using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PogrebOprema : ValueObject
{
    private Oprema _oprema;
    private int _kolicina;

    public Oprema Oprema { get => _oprema; set => _oprema = value; }
    public int Kolicina { get => _kolicina; set => _kolicina = value; }


    public PogrebOprema(Oprema oprema, int kolicina)
    {
        _oprema = oprema ?? throw new ArgumentNullException(nameof(oprema));
        _kolicina = kolicina;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PogrebOprema po &&
                _kolicina == po._kolicina &&
                _oprema == po._oprema;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_oprema, _kolicina);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _oprema != null, "Equipment can't be null"));
}