using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class Osiguranje : AggregateRoot<int>
{
    private DateTime _datumUgoravaranja;
    private bool _placanjeNaRate;
    private int _korisnikId;
    private int _brojRata;
    private string _ime;
    private string _prezime;
    private int _paketOsiguranjaId;
    private string _nazivPaketa;

    public DateTime DatumUgovaranja { get => _datumUgoravaranja; set => _datumUgoravaranja = value; }
    public bool PlacanjeNaRate { get => _placanjeNaRate; set => _placanjeNaRate = value; }
    public int KorisnikId { get => _korisnikId; set => _korisnikId = value; }
    public string Ime { get => _ime; set => _ime = value; }
    public string Prezime { get => _prezime; set => _prezime = value; }
    public int BrojRata { get => _brojRata; set => _brojRata = value; }
    public int PaketOsiguranjaId { get => _paketOsiguranjaId; set => _paketOsiguranjaId = value; }
    public string NazivPaketa { get => _nazivPaketa; set => _nazivPaketa = value; }



    public Osiguranje(int id, int korisnikId, string ime, string prezime, DateTime datumUgovaranja, bool placanjeNaRate,
        int brojRata, int paketOsiguranjaId, string nazivPaketa) : base(id)
    {
        _ime = ime;
        _prezime = prezime;
        _korisnikId = korisnikId;
        _datumUgoravaranja = datumUgovaranja;
        _placanjeNaRate = placanjeNaRate;
        _brojRata = brojRata;
        _paketOsiguranjaId = paketOsiguranjaId;
        _nazivPaketa = nazivPaketa;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is Osiguranje osiguranje &&
               _id == osiguranje._id &&
               _korisnikId == osiguranje._korisnikId &&
               _placanjeNaRate == osiguranje._placanjeNaRate &&
               _datumUgoravaranja == osiguranje._datumUgoravaranja &&
               _brojRata == osiguranje._brojRata &&
               _paketOsiguranjaId == osiguranje._paketOsiguranjaId;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _korisnikId, _datumUgoravaranja, _placanjeNaRate, _brojRata, _paketOsiguranjaId);
    }

    public override Result IsValid()
        => Validation.Validate();
}
