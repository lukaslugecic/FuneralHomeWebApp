using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Osmrtnica : Entity<int>
{
    private byte[]? _slikaPok;
    private string _opis;
    private bool _objavaNaStranici;


    public byte[]? SlikaPok { get => _slikaPok; set => _slikaPok = value; }
    public string Opis { get => _opis; set => _opis = value; }
    public bool ObjavaNaStranici { get => _objavaNaStranici; set => _objavaNaStranici = value; }


    public Osmrtnica(int id, byte[]? slikaPok, string opis, bool objavaNaStranici) : base(id)
    {
        _slikaPok = slikaPok;
        _opis = opis;
        _objavaNaStranici = objavaNaStranici;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Osmrtnica osmrtnica &&
               _id == osmrtnica._id &&
               _slikaPok == osmrtnica._slikaPok &&
               _opis == osmrtnica._opis &&
               _objavaNaStranici == osmrtnica._objavaNaStranici;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _slikaPok, _opis, _objavaNaStranici);
    }

    public override Result IsValid()
        => Validation.Validate();
}
