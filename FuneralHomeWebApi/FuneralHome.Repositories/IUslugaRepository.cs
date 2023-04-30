using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Usluga repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IUslugaRepository
    : IRepository<int, Usluga>,
      IAggregateRepository<int, Usluga>
{
    Result<IEnumerable<Usluga>> GetAllByType(int id);
}