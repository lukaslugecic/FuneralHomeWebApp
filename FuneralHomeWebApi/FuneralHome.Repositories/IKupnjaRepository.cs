using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Kupnja repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IKupnjaRepository
    : IRepository<int, Kupnja>,
      IAggregateRepository<int, Kupnja>
{
    Result<IEnumerable<Kupnja>> GetAllByKorisnikId(int korisnikId);
    Result<Kupnja> GetLatestByKorisnikId(int korisnikId);
}