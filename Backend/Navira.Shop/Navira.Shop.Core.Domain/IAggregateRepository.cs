namespace Navira.Shop.Core.Domain
{
    /// <summary>
    /// One repository per aggregate root — never per entity, never the generic
    /// query-heavy repository. This is intentionally narrower than both existing
    /// repository contracts in the solution:
    ///   - Navira.Shop.Core.Persistence.IRepository&lt;T,TKey&gt; (Get/FindBy/Insert/Update/Delete)
    ///     — the original CQRS-era contract, still fine for simple read/write needs
    ///       outside the DDD aggregates (e.g. lookups, reference data).
    ///   - Navira.Shop.Core.Persistence.EF.IRepository&lt;TEntity,TKey&gt; — the
    ///     caching-aware, projection-heavy repository built for read-side/query use.
    ///     Aggregates should NOT be loaded through this one: returning a partial
    ///     projection of an aggregate (via its generic GetById&lt;T&gt;) would let code
    ///     construct an aggregate without its full object graph, which breaks the
    ///     invariant checks that assume the aggregate is fully loaded.
    ///
    /// GetByIdAsync always returns the complete aggregate (all child entities loaded) —
    /// implementations should .Include() everything reachable from the root. There is
    /// deliberately no generic Update(...): EF's change tracker picks up mutations on
    /// a loaded aggregate automatically when Commit() runs.
    /// </summary>
    public interface IAggregateRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
    {
        Task<TAggregate?> GetByIdAsync(TId id, CancellationToken ct = default);
        Task AddAsync(TAggregate aggregate, CancellationToken ct = default);
        void Remove(TAggregate aggregate);
    }

}
