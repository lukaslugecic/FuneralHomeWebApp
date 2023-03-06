namespace FuneralHome.Repositories;

/// <summary>
/// Facade interface for a Person repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TModel"></typeparam>
public interface IPogrebRepository<TKey, TModel> : IRepository<TKey, TModel>, IAggregateRepository<TKey, TModel>
{
}