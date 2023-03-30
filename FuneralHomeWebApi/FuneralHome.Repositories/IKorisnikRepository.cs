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
    public bool Exists(string mail);
    public Result<Korisnik> GetByMail(string mail);
}