using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Korisnik repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IKorisnikRepository
    : IRepository<int, Korisnik>,
      IAggregateRepository<int, Korisnik>
{
    bool Exists(string mail);
    Result<Korisnik> GetByMail(string mail);

    Result<IEnumerable<Korisnik>> GetAllWithoutInsurance();
    string CreateToken(Korisnik korisnik);
}