using FuneralHome.Commons;

namespace FuneralHome.Repositories;

/// <summary>
/// A base repository interface working over an entity
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TModel"></typeparam>
public interface IRepository<TKey, TModel>
{
    /// <summary>
    /// Gets an entity with given key/id
    /// </summary>
    /// <param name="id"></param>
    /// <returns><c>Option</c> of entity</returns>
    Option<TModel> Get(TKey id);

    /// <summary>
    /// Gets all entities 
    /// </summary>
    /// <returns><c>IEnumerable</c> of entities</returns>
    IEnumerable<TModel> GetAll();

    /// <summary>
    /// Inserts a new entity
    /// </summary>
    /// <param name="model">Model object</param>
    /// <returns><c>true</c> on success, <c>false</c> on fail</returns>
    bool Insert(TModel model);

    /// <summary>
    /// Updates an entity
    /// </summary>
    /// <param name="model">Model object</param>
    /// <returns><c>true</c> on success, <c>false</c> on fail</returns>
    bool Update(TModel model);

    /// <summary>
    /// Removes an entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns><c>true</c> on success, <c>false</c> on fail</returns>
    bool Remove(TKey id);

    /// <summary>
    /// Checks if an entity exists
    /// </summary>
    /// <param name="model">Model object</param>
    /// <returns><c>true</c> on exists, else <c>false</c></returns>
    bool Exists(TModel model);

    /// <summary>
    /// Checks if an entity with the given id exists 
    /// </summary>
    /// <param name="id">Entity id</param>
    /// <returns><c>true</c> on exists, else <c>false</c></returns>
    bool Exists(TKey id);
}