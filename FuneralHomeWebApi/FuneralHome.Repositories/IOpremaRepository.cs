using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Oprema repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IOpremaRepository
    : IRepository<int, Oprema>,
      IAggregateRepository<int, Oprema>
{
    Result<IEnumerable<Oprema>> GetAllByType(int id);

    Result IncreaseZaliha(Oprema model, int kolicina);
    Result DecreaseZaliha(Oprema model, int kolicina);
}