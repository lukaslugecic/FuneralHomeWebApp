using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Oglas : AggregateRoot<int>
{
    private byte[]? _slikaPok;
    private string _opis;
    private bool _objavaNaStranici;


    public byte[]? SlikaPok { get => _slikaPok; set => _slikaPok = value; }
    public string Opis { get => _opis; set => _opis = value; }
    public bool ObjavaNaStranici { get => _objavaNaStranici; set => _objavaNaStranici = value; }


    public Oglas(int id, byte[]? slikaPok, string opis, bool objavaNaStranici) : base(id)
    {
        _slikaPok = slikaPok;
        _opis = opis;
        _objavaNaStranici = objavaNaStranici;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Oglas oglas &&
               _id == oglas._id &&
               _slikaPok == oglas._slikaPok &&
               _opis == oglas._opis &&
               _objavaNaStranici == oglas._objavaNaStranici;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _slikaPok, _opis, _objavaNaStranici);
    }

    public override Result IsValid()
        => Validation.Validate();
}
