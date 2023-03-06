namespace FuneralHome.Repositories;


/// <summary>
/// Facade interface for a Role repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TModel"></typeparam>
public interface IKorisnikRepository<TKey, TModel> : IRepository<TKey, TModel>, IAggregateRepository<TKey, TModel>
{
}
