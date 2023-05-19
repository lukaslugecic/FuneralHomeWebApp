using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Oprema repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IOpremaUslugaRepository
    : IRepository<int, OpremaUsluga>,
      IAggregateRepository<int, OpremaUsluga>
{
    Result<IEnumerable<OpremaUsluga>> GetAllByType(int id);

    Result IncreaseZaliha(OpremaUsluga model, int kolicina);
    Result DecreaseZaliha(OpremaUsluga model, int kolicina);
}