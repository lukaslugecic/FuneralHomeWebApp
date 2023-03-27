using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Person repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface ISmrtniSlucajRepository
    : IRepository<int, SmrtniSlucaj>,
      IAggregateRepository<int, SmrtniSlucaj>
{
}