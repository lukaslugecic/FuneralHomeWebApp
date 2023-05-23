using BaseLibrary;
using FuneralHome.Commons;
using System.Data;

namespace FuneralHome.Domain.Models;
public class VrstaOpremeUsluge : Entity<int>
{
    private string _naziv;
    private bool _jeOprema;
    private int? _jedinicaMjereId;
    private string? _jedinicaMjereNaziv;
    public string Naziv { get => _naziv; set => _naziv = value; }
    public bool JeOprema { get => _jeOprema; set => _jeOprema = value; }
    public int? JedinicaMjereId { get => _jedinicaMjereId; set => _jedinicaMjereId = value; }
    public string? JedinicaMjereNaziv { get => _jedinicaMjereNaziv; set => _jedinicaMjereNaziv = value; }


    public VrstaOpremeUsluge(int id, string naziv, bool jeOprema, int? jedinicaMjereId, string? jedinicaMjereNaziv) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }
        _naziv = naziv;
        _jeOprema = jeOprema;
        _jedinicaMjereId = jedinicaMjereId;
        _jedinicaMjereNaziv = jedinicaMjereNaziv;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is VrstaOpremeUsluge vrsta &&
               _id == vrsta._id &&
               _naziv == vrsta._naziv &&
               _jeOprema == vrsta._jeOprema &&
               _jedinicaMjereId == vrsta._jedinicaMjereId;

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id, _naziv, _jeOprema, _jedinicaMjereId);
    }

    public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "Name lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_naziv.Trim()), "First name can't be null, empty, or whitespace"));
}