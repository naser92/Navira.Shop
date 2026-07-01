using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Navira.Shop.Core.Mapper;

namespace Navira.Shop.Core.Domain
{
    public class PriorityShadowProperties
    {
        public int Priority { get; set; }
    }

    public static class PrioritisedShadowProperties
    {




        public static readonly Func<object, int> EFPropertyPriority =
                                        entity => EF.Property<int>(entity, Priority);
        public static readonly string Priority = nameof(Priority);


        public static void AddPrioritisedShadowProperty(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                                                .GetEntityTypes()
                                                .Where(e => typeof(IHasPriority).IsAssignableFrom(e.ClrType)))
            {


                modelBuilder.Entity(entityType.ClrType)
                            .Property<int>(Priority);
            }
        }

        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entityType);
        /// <summary>
        /// More info: http://www.dotnettips.info/post/2507
        /// </summary>
        public static void SetPrioritisedEntityPropertyValues(
            this ChangeTracker changeTracker, DbContext context)
        {
            //var modifiedEntries = changeTracker.Entries<IPrioritisedEntity>()
            //                                .Where(x => x.State == EntityState.Modified);
            //foreach (var modifiedEntry in modifiedEntries)
            //{
            //    modifiedEntry.SetModifiedPrioritisedShadowProperties(props);
            //}

            var addedEntries = changeTracker.Entries<IHasPriority>()
                                            .Where(x => x.State == EntityState.Added);
            foreach (var addedEntry in addedEntries)
            {
                addedEntry.SetAddedPrioritisedShadowProperties(context);
            }
        }

        public static void SetAddedPrioritisedShadowProperties(this EntityEntry<IHasPriority> addedEntry, DbContext context)
        {
            var query = context.Query(addedEntry.Metadata.Name);
            var maxvalue = query.ProjectTo<PriorityShadowProperties>().Any() ? query.ProjectTo<PriorityShadowProperties>().Max(x => x.Priority) : 0;

            addedEntry.Property(Priority).CurrentValue = maxvalue + 1;

        }

    }
}