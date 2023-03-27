using BaseLibrary;
using FuneralHome.Domain;

namespace FuneralHome.Repositories
{
    /// <summary>
    /// Interface extension for repositories working over aggregates
    /// </summary>
    /// <typeparam name="TKey">Type of key in aggregate root</typeparam>
    /// <typeparam name="TAggregate">Type of aggregate root</typeparam>
    public interface IAggregateRepository<TKey, TAggregate> where TAggregate : AggregateRoot<TKey>
    {
        /// <summary>
        /// Gets an aggregate with given key/id
        /// </summary>
        /// <param name="id">Aggregate root id</param>
        /// <returns>Option of <c>TAggregate</c></returns>
        Result<TAggregate> GetAggregate(TKey id);

        /// <summary>
        /// Gets all aggregates
        /// </summary>
        /// <returns><c>IEnumerable</c> of <c>TAggregate</c></returns>
        Result<IEnumerable<TAggregate>> GetAllAggregates();

        /// <summary>
        /// Updates the entire aggregate
        /// </summary>
        /// <param name="model">Aggregate object</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure</returns>
        Result UpdateAggregate(TAggregate model);
    }
}