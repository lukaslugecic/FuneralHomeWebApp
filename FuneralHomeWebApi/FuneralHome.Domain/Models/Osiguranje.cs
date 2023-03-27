using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Osiguranje : Entity<int>
{
    private DateTime _datumUgoravaranja;
    private bool _placanjeNaRate;


    public DateTime DatumUgovaranja { get => _datumUgoravaranja; set => _datumUgoravaranja = value; }
    public bool PlacanjeNaRate { get => _placanjeNaRate; set => _placanjeNaRate = value; }


    public Osiguranje(int id, DateTime datumUgovaranja, bool placanjeNaRate) : base(id)
    {
        _datumUgoravaranja = datumUgovaranja;
        _placanjeNaRate = placanjeNaRate;

    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Osiguranje osiguranje &&
               _id == osiguranje._id &&
               _placanjeNaRate == osiguranje._placanjeNaRate &&
               _datumUgoravaranja == osiguranje._datumUgoravaranja;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _datumUgoravaranja, _placanjeNaRate);
    }

    public override Result IsValid()
        => Validation.Validate();
}
