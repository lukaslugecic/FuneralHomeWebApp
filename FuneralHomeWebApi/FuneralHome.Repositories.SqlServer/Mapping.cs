using FuneralHome.Domain.Models;
using System.Data;
using System;
using DbModels = FuneralHome.DataAccess.SqlServer.Data.DbModels;
namespace FuneralHome.Repositories.SqlServer;
public static class Mapping
{
    public static Cvijece ToDomain(this DbModels.Cvijece cvijece)
        => new Cvijece(
            cvijece.Id,
            cvijece.Naziv,
            cvijece.Slika,
            cvijece.Kolicina,
            cvijece.Cijena
        );

    public static DbModels.Cvijece ToDbModel(this Cvijece cvijece)
        => new DbModels.Cvijece()
        {
            Id = cvijece.Id,
            Naziv = cvijece.Naziv,
            Slika = cvijece.Slika,
            Kolicina = cvijece.Kolicina,
            Cijena = cvijece.Cijena
        };

    public static Glazba ToDomain(this DbModels.Glazba glazba)
        => new Glazba(
            glazba.Id,
            glazba.Naziv,
            glazba.Opis,
            glazba.Kontakt,
            glazba.Cijena
        );

    public static DbModels.Glazba ToDbModel(this Glazba glazba)
        => new DbModels.Glazba()
        {
            Id = glazba.Id,
            Naziv = glazba.Naziv,
            Opis = glazba.Opis,
            Kontakt = glazba.Kontakt,
            Cijena = glazba.Cijena 
        };

    public static Korisnik ToDomain(this DbModels.Korisnik korisnik)
        => new Korisnik(
            korisnik.Id,
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
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            DatumRodenja = korisnik.DatumRodenja,
            Adresa = korisnik.Adresa,
            Oib = korisnik.Oib,
            Mail = korisnik.Mail,
            Lozinka = korisnik.Lozinka,
            VrstaKorisnika = korisnik.VrstaKorisnika
        };

    public static Lijes ToDomain(this DbModels.Lijes lijes)
        => new Lijes(
            lijes.Id,
            lijes.Naziv,
            lijes.Velicina,
            lijes.Slika,
            lijes.Kolicina,
            lijes.Cijena
        );

    public static DbModels.Lijes ToDbModel(this Lijes lijes)
        => new DbModels.Lijes()
        {
            Id = lijes.Id,
            Naziv = lijes.Naziv,
            Velicina = lijes.Velicina,
            Slika = lijes.Slika,
            Kolicina = lijes.Kolicina,
            Cijena = lijes.Cijena
        };

    public static NadgrobniZnak ToDomain(this DbModels.NadgrobniZnak znak)
       => new NadgrobniZnak(
           znak.Id,
           znak.Naziv,
           znak.Slika,
           znak.Kolicina,
           znak.Cijena
       );

    public static DbModels.NadgrobniZnak ToDbModel(this NadgrobniZnak znak)
        => new DbModels.NadgrobniZnak()
        {
            Id = znak.Id,
            Naziv = znak.Naziv,
            Slika = znak.Slika,
            Kolicina = znak.Kolicina,
            Cijena = znak.Cijena
        };


    public static Oglas ToDomain(this DbModels.Oglas oglas)
       => new Oglas(
           oglas.Id,
           oglas.SlikaPok,
           oglas.Opis,
           oglas.ObjavaNaStranici
       );

    public static DbModels.Oglas ToDbModel(this Oglas oglas)
        => new DbModels.Oglas()
        {
           Id= oglas.Id,
           SlikaPok = oglas.SlikaPok,
           Opis = oglas.Opis,
           ObjavaNaStranici = oglas.ObjavaNaStranici
        };

    public static Osiguranje ToDomain(this DbModels.Osiguranje osiguranje)
       => new Osiguranje(
           osiguranje.Id,
           osiguranje.DatumUgovaranja,
           osiguranje.PlacanjeNaRate
       );

    public static DbModels.Osiguranje ToDbModel(this Osiguranje osiguranje)
        => new DbModels.Osiguranje()
        {
            Id = osiguranje.Id,
            DatumUgovaranja = osiguranje.DatumUgovaranja,
            PlacanjeNaRate = osiguranje.PlacanjeNaRate,
        };

    public static Pogreb ToDomain(this DbModels.Pogreb pogreb)
       => new Pogreb(
           pogreb.Id,
           pogreb.SmrtniSlucajId,
           pogreb.DatumPogreba,
           pogreb.Kremacija,
           pogreb.UrnaId,
           pogreb.LijesId,
           pogreb.CvijeceId,
           pogreb.NadgrobniZnakId,
           pogreb.GlazbaId,
           pogreb.Snimanje,
           pogreb.Branitelj,
           pogreb.Golubica,
           pogreb.UkupnaCijena
       );

    public static DbModels.Pogreb ToDbModel(this Pogreb pogreb)
        => new DbModels.Pogreb()
        {
           Id = pogreb.Id,
           SmrtniSlucajId = pogreb.SmrtniSlucajId,
           DatumPogreba = pogreb.DatumPogreba,
           Kremacija = pogreb.Kremacija,
           UrnaId = pogreb.UrnaId,
           LijesId = pogreb.LijesId,
           CvijeceId = pogreb.CvijeceId,
           NadgrobniZnakId = pogreb.NadgrobniZnakId,
           GlazbaId = pogreb.GlazbaId,
           Branitelj = pogreb.Branitelj,
           Golubica = pogreb.Golubica,
           UkupnaCijena = pogreb.UkupnaCijena
        };

    public static SmrtniSlucaj ToDomain(this DbModels.SmrtniSlucaj smrtniSlucaj)
       => new SmrtniSlucaj(
           smrtniSlucaj.Id,
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
           Id = smrtniSlucaj.Id,
           KorisnikId = smrtniSlucaj.KorisnikId,
           ImePok = smrtniSlucaj.ImePok,
           PrezimePok = smrtniSlucaj.PrezimePok,
           DatumRodenjaPok = smrtniSlucaj.DatumRodenjaPok,
           DatumSmrtiPok = smrtniSlucaj.DatumSmrtiPok,
           Oibpok = smrtniSlucaj.Oibpok
        };

    public static Urna ToDomain(this DbModels.Urna urna)
       => new Urna(
           urna.Id,
           urna.Naziv,
           urna.Slika,
           urna.Kolicina,
           urna.Cijena
       );

    public static DbModels.Urna ToDbModel(this Urna urna)
        => new DbModels.Urna()
        {
            Id = urna.Id,
            Naziv = urna.Naziv,
            Slika = urna.Slika,
            Kolicina = urna.Kolicina,
            Cijena = urna.Cijena
        };
}