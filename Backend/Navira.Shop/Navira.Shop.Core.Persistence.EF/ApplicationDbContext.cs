using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using NaviraShop.Core.Mq;

namespace Navira.Shop.Core.Persistence.EF
{
    public abstract class ApplicationDbContext : DbContext
    {
        private IPublisher _publisher;
        protected ApplicationDbContext() : base()
        {
        }
        protected ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }
        public async Task Save()
        {
            await SaveChangesAsync();
            // beforeSaveTriggers();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(async (command) => await _publisher.Log(command), new[] { RelationalEventId.CommandError, RelationalEventId.TransactionError });
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var assembly in _typeFinder.GetAssemblies())
            //{
            //   modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            //}
            // This should be placed here, at the end.

            modelBuilder.ApplyMemoryOptimizedTables();
            modelBuilder.ApplySoftDeleteQueryFilters();
            base.OnModelCreating(modelBuilder);
        }

    }
    public abstract class WriteFrameworkDbContext : ApplicationDbContext, IUnitOfWork
    {
        private IBus _bus;
        private IDomainEventDispatcher _dispatcher;

        protected WriteFrameworkDbContext(IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected WriteFrameworkDbContext()
            : base()
        {
        }

        protected WriteFrameworkDbContext(DbContextOptions options, IBus bus, IPublisher publisher)
            : base(options, publisher)
        {
            _bus = bus;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.AddAuditableShadowProperties();
            modelBuilder.AddPrioritisedShadowProperty();
            base.OnModelCreating(modelBuilder);

        }

        public async Task Commit()
        {
            beforeSaveTriggers();

            var aggregatesWithEvents = ChangeTracker.Entries()
                    .Select(e => e.Entity)
                    .OfType<IHasDomainEvents>()
                    .Where(a => a.DomainEvents.Count > 0)
                    .ToList();


            await Save();

            var allEvents = aggregatesWithEvents.SelectMany(a => a.DomainEvents).ToList();
            if (allEvents.Count > 0)
            {
                await _dispatcher.DispatchAsync(allEvents);
                foreach (var a in aggregatesWithEvents) a.ClearDomainEvents();
            }
        }

        //}
        //private Task PublishEvents(List<EntityChange> changes)
        //{
        //    var events = changes.GetEntityChangeEvents();
        //    PublishEvents(events);
        //    foreach (var change in changes.Where(x=>x.Entity.Changes.Count>0).ToList())
        //    {
        //        PublishEvents(change.Entity.Changes);
        //    }
        //    return Task.CompletedTask;
        //}
        //private Task PublishEvents(IList<IEvent> events)
        //{
        //    foreach (var item in events.Where(x=>x!=null).ToList())
        //    {
        //        _bus.Publish(item);
        //    }
        //    return Task.CompletedTask;
        //}
        public Task Rollback()
        {
            //https://stackoverflow.com/questions/5466677/undo-changes-in-entity-framework-entities
            var changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var props = this.GetService<IHttpContextAccessor>()?.GetShadowProperties();
            ChangeTracker.SetAuditableEntityPropertyValues(props);
            ChangeTracker.SetPrioritisedEntityPropertyValues(this);
        }
        private void beforeSaveTriggers()
        {
            ChangeTracker.AdjustStringEntityPropertyValues();
            setShadowProperties();
        }
    }
    public abstract class QueryFrameworkDbContext : ApplicationDbContext
    {
        protected QueryFrameworkDbContext() : base()
        {
        }
        protected QueryFrameworkDbContext(DbContextOptions options, IPublisher publisher) : base(options, publisher)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
