using FuneralHome.Domain.Models;
using System.Data;
using System;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
namespace FuneralHome.Repositories.SqlServer;
public static class Mapping
{
    
    public static Korisnik ToDomain(this DbModels.Korisnik korisnik)
        => new Korisnik(
            korisnik.IdKorisnik,
            korisnik.Ime,
            korisnik.Prezime,
            korisnik.DatumRodenja,
            korisnik.Adresa,
            korisnik.Oib,
            korisnik.Mail,
            korisnik.Lozinka,
            korisnik.VrstaKorisnika
            // TO DO mozda neki agregat slozit
        );

    public static DbModels.Korisnik ToDbModel(this Korisnik korisnik)
        => new DbModels.Korisnik()
        {
            IdKorisnik = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            DatumRodenja = korisnik.DatumRodenja,
            Adresa = korisnik.Adresa,
            Oib = korisnik.Oib,
            Mail = korisnik.Mail,
            Lozinka = korisnik.Lozinka,
            VrstaKorisnika = korisnik.VrstaKorisnika
        };



    public static Osiguranje ToDomain(this DbModels.Osiguranje osiguranje)
       => new Osiguranje(
           osiguranje.IdOsiguranje,
           osiguranje.KorisnikId,
           osiguranje.Korisnik.Ime,
           osiguranje.Korisnik.Prezime,
           osiguranje.DatumUgovaranja,
           osiguranje.PlacanjeNaRate,
           osiguranje.BrojRata,
           osiguranje.PaketOsiguranjaId,
           osiguranje.PaketOsiguranja.Naziv
       );

    public static DbModels.Osiguranje ToDbModel(this Osiguranje osiguranje)
        => new DbModels.Osiguranje()
        {
            IdOsiguranje = osiguranje.Id,
            KorisnikId = osiguranje.KorisnikId,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate,
            BrojRata = osiguranje.BrojRata,
            PaketOsiguranjaId = osiguranje.PaketOsiguranjaId
        };

    public static PaketOsiguranja ToDomain(this DbModels.PaketOsiguranja paket)
        => new PaketOsiguranja(
                paket.IdPaketOsiguranja,
                paket.Naziv
           );

    public static DbModels.PaketOsiguranja ToDbModel(this PaketOsiguranja paket)
        => new DbModels.PaketOsiguranja()
        {
            IdPaketOsiguranja = paket.Id,
            Naziv = paket.Naziv
        };

    public static JedinicaMjere ToDomain(this DbModels.JedinicaMjere jedinicaMjere)
        => new JedinicaMjere(
                jedinicaMjere.IdJednicaMjere,
                jedinicaMjere.Naziv
           );

    public static DbModels.JedinicaMjere ToDbModel(this JedinicaMjere jedinicaMjere)
        => new DbModels.JedinicaMjere()
        {
            IdJednicaMjere = jedinicaMjere.Id,
            Naziv = jedinicaMjere.Naziv
        };

    public static Pogreb ToDomain(this DbModels.Pogreb pogreb)
       => new Pogreb(
           pogreb.IdPogreb,
           pogreb.SmrtniSlucajId,
           pogreb.DatumPogreba,
           pogreb.Kremacija,
           pogreb.DatumUgovaranja,
           pogreb.UkupnaCijena,
           pogreb.SmrtniSlucaj?.Korisnik.ToDomain(),
           pogreb.SmrtniSlucaj?.ToDomain(),
           pogreb.PogrebOpremaUsluge.Select(ToDomain)
       );

    public static DbModels.Pogreb ToDbModel(this Pogreb pogreb)
        => new DbModels.Pogreb()
        {
            IdPogreb = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            DatumUgovaranja = pogreb.DatumUgovaranja,
            Kremacija = pogreb.Kremacija,
            UkupnaCijena = pogreb.UkupnaCijena,
           // PogrebOprema = pogreb.PogrebOprema.Select(po => po.ToDbModel(pogreb.Id)).ToList(),
           // Usluga = pogreb.PogrebUsluga.Select(pu => pu.ToDbModel()).ToList(),
           // SmrtniSlucaj = pogreb.SmrtniSlucaj?.ToDbModel()
        };

    public static PogrebOpremaUsluge ToDomain(this DbModels.PogrebOpremaUsluge pogrebOprema)
        => new PogrebOpremaUsluge(
            pogrebOprema.OpremaUsluga.ToDomain(),
            (int) pogrebOprema.Kolicina,
            pogrebOprema.Cijena
            );
    public static DbModels.PogrebOpremaUsluge ToDbModel(this PogrebOpremaUsluge pogrebOprema, int pogrebId)
       => new DbModels.PogrebOpremaUsluge()
       {
           PogrebId = pogrebId,
           OpremaUslugaId = pogrebOprema.OpremaUsluga.Id,
           Kolicina = pogrebOprema.Kolicina,
           Cijena = pogrebOprema.Cijena
       };

    public static PogrebSmrtniSlucaj ToDomain2(this DbModels.Pogreb pogreb)
        => new PogrebSmrtniSlucaj(
                pogreb.IdPogreb,
                pogreb.SmrtniSlucajId,
                pogreb.SmrtniSlucaj.ImePok,
                pogreb.SmrtniSlucaj.PrezimePok,
                pogreb.SmrtniSlucaj.DatumSmrtiPok,
                pogreb.DatumPogreba,
                pogreb.Kremacija,
                pogreb.UkupnaCijena,
                pogreb.SmrtniSlucaj.KorisnikId,
                pogreb.SmrtniSlucaj.Korisnik.Ime,
                pogreb.SmrtniSlucaj.Korisnik.Prezime,
                pogreb.DatumUgovaranja
            );

    public static DbModels.Pogreb ToDbModel(this PogrebSmrtniSlucaj pogreb)
        => new DbModels.Pogreb()
        {
            IdPogreb = pogreb.Id,
            SmrtniSlucajId = pogreb.SmrtniSlucajId,
            DatumPogreba = pogreb.DatumPogreba,
            Kremacija = pogreb.Kremacija,
            DatumUgovaranja = pogreb.DatumUgovaranja,
            UkupnaCijena = pogreb.UkupnaCijena
        };

    public static SmrtniSlucaj ToDomain(this DbModels.SmrtniSlucaj smrtniSlucaj)
       => new SmrtniSlucaj(
           smrtniSlucaj.IdSmrtniSlucaj,
           smrtniSlucaj.KorisnikId,
           smrtniSlucaj.ImePok,
           smrtniSlucaj.PrezimePok,
           smrtniSlucaj.DatumRodenjaPok,
           smrtniSlucaj.DatumSmrtiPok,
           smrtniSlucaj.Oibpok
       );

    public static DbModels.SmrtniSlucaj ToDbModel(this SmrtniSlucaj smrtniSlucaj)
        => new DbModels.SmrtniSlucaj()
        {
           IdSmrtniSlucaj = smrtniSlucaj.Id,
           KorisnikId = smrtniSlucaj.KorisnikId,
           ImePok = smrtniSlucaj.ImePok,
           PrezimePok = smrtniSlucaj.PrezimePok,
           DatumRodenjaPok = smrtniSlucaj.DatumRodenjaPok,
           DatumSmrtiPok = smrtniSlucaj.DatumSmrtiPok,
           Oibpok = smrtniSlucaj.Oibpok
        };


    public static OpremaUsluga ToDomain(this DbModels.OpremaUsluga opremaUsluga)
        => new OpremaUsluga(
            opremaUsluga.IdOpremaUsluga,
            opremaUsluga.VrstaOpremeUslugeId,
            opremaUsluga.VrstaOpremeUsluge.Naziv,
            opremaUsluga.VrstaOpremeUsluge.JeOprema,
            opremaUsluga.Naziv,
            opremaUsluga.Slika,
            opremaUsluga.Zaliha,
            opremaUsluga.Opis,
            opremaUsluga.VrstaOpremeUsluge.JedinicaMjereId is not null ? opremaUsluga.VrstaOpremeUsluge.JedinicaMjere.Naziv : "",
            opremaUsluga.Cijena
           );

    public static DbModels.OpremaUsluga ToDbModel(this OpremaUsluga opremaUsluga)
        => new DbModels.OpremaUsluga()
        {
            IdOpremaUsluga = opremaUsluga.Id,
            VrstaOpremeUslugeId = opremaUsluga.VrstaOpremeUslugeId,
            Naziv = opremaUsluga.Naziv,
            Slika = opremaUsluga.Slika,
            Zaliha = opremaUsluga.Zaliha,
            Opis = opremaUsluga.Opis,
            Cijena = opremaUsluga.Cijena

        };

    public static VrstaOpremeUsluge ToDomain(this DbModels.VrstaOpremeUsluge vrstaOpremeUsluge)
        => new VrstaOpremeUsluge(
                vrstaOpremeUsluge.IdVrstaOpremeUsluge,
                vrstaOpremeUsluge.Naziv,
                vrstaOpremeUsluge.JeOprema,
                vrstaOpremeUsluge.JedinicaMjereId,
                vrstaOpremeUsluge.JedinicaMjereId is not null ? vrstaOpremeUsluge.JedinicaMjere.Naziv : ""
                );

    public static DbModels.VrstaOpremeUsluge ToDbModel(this VrstaOpremeUsluge vrstaOpremeUsluge)
        => new DbModels.VrstaOpremeUsluge()
        {
            IdVrstaOpremeUsluge = vrstaOpremeUsluge.Id,
            Naziv = vrstaOpremeUsluge.Naziv,
            JeOprema = vrstaOpremeUsluge.JeOprema,
            JedinicaMjereId = vrstaOpremeUsluge.JedinicaMjereId
        };

}