using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a PaketOsiguranja repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IPaketOsiguranjaRepository
    : IRepository<int, PaketOsiguranja>
{
}