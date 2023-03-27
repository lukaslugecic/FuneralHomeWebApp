using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Oglas repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IOglasRepository
    : IRepository<int, Oglas>,
      IAggregateRepository<int, Oglas>
{
}