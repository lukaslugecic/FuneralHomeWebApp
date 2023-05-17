using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class PogrebOpremaUsluga : ValueObject
{
    private OpremaUsluga _opremaUsluga;
    private int _kolicina;


    public OpremaUsluga OpremaUsluga { get => _opremaUsluga; set => _opremaUsluga = value; }
    public int Kolicina { get => _kolicina; set => _kolicina = value; }


    public PogrebOpremaUsluga(OpremaUsluga opremaUsluga, int kolicina)
    {
        _opremaUsluga = opremaUsluga ?? throw new ArgumentNullException(nameof(opremaUsluga));
        _kolicina = kolicina;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is PogrebOpremaUsluga pou &&
                _kolicina == pou._kolicina &&
                _opremaUsluga == pou._opremaUsluga;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_opremaUsluga, _kolicina);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _opremaUsluga != null, "Equipment or service can't be null"));
}