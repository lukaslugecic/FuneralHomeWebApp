using BaseLibrary;
using FuneralHome.Domain.Models;
using System;

namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a VrstaOpreme repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IVrstaOpremeUslugeRepository
    : IRepository<int, VrstaOpremeUsluge>
{
    Result<IEnumerable<VrstaOpremeUsluge>> GetAllOprema();
    Result<IEnumerable<VrstaOpremeUsluge>> GetAllUsluge();
}