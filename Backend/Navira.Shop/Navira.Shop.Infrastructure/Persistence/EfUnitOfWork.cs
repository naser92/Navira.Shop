using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Persistence;

namespace Navira.Shop.Infrastructure.Persistence
{
    /// <summary>
    /// Implements the EXISTING Navira.Shop.Core.Persistence.IUnitOfWork — note this is
    /// Commit()/Rollback(), not a SaveChangesAsync()-shaped contract. That shape is fixed
    /// by Navira.Shop.Core.Bus.BusControl, which calls handler.Uow.Commit() automatically
    /// after every command handler's Handle(...) returns successfully (and Rollback() if
    /// it throws) — see BusControl.Send&lt;TCommand&gt;. This class cannot change that
    /// calling convention; it has to fit inside it.
    ///
    /// Domain event dispatch therefore happens INSIDE Commit(), not in a separate
    /// SaveChangesAsync wrapper: Commit() is the only hook BusControl gives us that runs
    /// after the handler's work is done and before the bus considers the command finished.
    /// Events are collected from the DbContext's ChangeTracker BEFORE calling
    /// SaveChangesAsync, then dispatched AFTER it succeeds — so a handler that publishes
    /// to RabbitMQ (via the existing IPublisher in NaviraShop.Core.Mq) never observes a
    /// change that might still be rolled back.
    /// </summary>
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IDomainEventDispatcher _dispatcher;

        public EfUnitOfWork(DbContext dbContext, IDomainEventDispatcher dispatcher)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public async Task Commit()
        {
            var aggregatesWithEvents = _dbContext.ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .OfType<IHasDomainEvents>()
                .Where(a => a.DomainEvents.Count > 0)
                .ToList();

            await _dbContext.SaveChangesAsync();

            var allEvents = aggregatesWithEvents.SelectMany(a => a.DomainEvents).ToList();
            if (allEvents.Count > 0)
            {
                await _dispatcher.DispatchAsync(allEvents);
                foreach (var aggregate in aggregatesWithEvents)
                    aggregate.ClearDomainEvents();
            }
        }

        public Task Rollback()
        {
            // EF's change tracker discards pending changes simply by not calling
            // SaveChangesAsync. Detaching tracked entities here prevents a retried
            // operation on the same DbContext instance from accidentally resubmitting
            // the failed change set, which matters if the DbContext is scoped per-request
            // rather than per-command.
            foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;

            return Task.CompletedTask;
        }
    }

}
