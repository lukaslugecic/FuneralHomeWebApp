using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Pogreb repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IPogrebRepository
    : IRepository<int, Pogreb>,
      IAggregateRepository<int, Pogreb>
{
    Result<IEnumerable<PogrebSmrtniSlucaj>> GetAllPogrebSmrtniSlucaj();
}