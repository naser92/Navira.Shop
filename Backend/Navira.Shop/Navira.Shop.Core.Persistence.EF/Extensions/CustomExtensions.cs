using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;
using Navira.Shop.Core.Mapper;
using System.Reflection;

namespace Navira.Shop.Core.Persistence.EF
{
    public static class CustomExtensions
    {
        public static TEntity FindTracked<TEntity>(this DbContext context, params object[] keyValues)
            where TEntity : class
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();
            var stateManager = context.GetDependencies().StateManager;
            var entry = stateManager.TryGetEntry(key, keyValues);
            return (entry?.Entity as TEntity) ??
                   GenerateModel<TEntity>(key.Properties.Select(x => x.PropertyInfo), keyValues);
        }

        private static TEntity GenerateModel<TEntity>(IEnumerable<PropertyInfo> properties, params object[] keyValues)
        {
            var instance = Activator.CreateInstance<TEntity>();

            properties.Each((property, index) => { property.SetValue(instance, keyValues[index], null); });

            return instance;
        }

        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }

        public static bool Each<T>(this IEnumerable<T> ie, Func<T, int, bool> action)
        {
            int i = 0;
            foreach (T e in ie)
                if (!action(e, i++))
                    return false;
            return true;
        }

        public static List<EntityChange> GetEvent(this ChangeTracker changeTracker)
        {
            return changeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged && x.Entity is IAggregateRoot).Select(GetEntityChange)
                .ToList();
        }

        private static EntityChange GetEntityChange(EntityEntry entryEntity)
        {

            var entity = entryEntity.Entity;
            if (entity != null)
            {
                var idPropertyName = "";
                var dataPropertyName = "";
                var state = entryEntity.State;
                switch (entryEntity.State)
                {
                    case EntityState.Added:
                        idPropertyName = AuditableShadowProperties.CreatedByUserId;
                        dataPropertyName = AuditableShadowProperties.CreatedDate;
                        break;
                    case EntityState.Modified:
                        if ((bool)entryEntity.Property("IsDeleted").CurrentValue)
                        {
                            idPropertyName = AuditableShadowProperties.DeletedByUserId;
                            dataPropertyName = AuditableShadowProperties.DeletedDate;
                            state = EntityState.Deleted;
                        }
                        else
                        {
                            idPropertyName = AuditableShadowProperties.ModifiedByUserId;
                            dataPropertyName = AuditableShadowProperties.ModifiedDate;
                        }

                        break;
                    case EntityState.Deleted:
                        idPropertyName = AuditableShadowProperties.DeletedByUserId;
                        dataPropertyName = AuditableShadowProperties.DeletedDate;
                        break;

                }

                var doBy = idPropertyName == "" ? null : (Guid?)entryEntity.Property(idPropertyName).CurrentValue;
                var dodate = dataPropertyName == ""
                    ? null
                    : (DateTime?)entryEntity.Property(dataPropertyName).CurrentValue;
                return new EntityChange((IAggregateRoot)entity, state, doBy, dodate);

            }

            return null;
        }

        public static List<IEvent> GetEntityChangeEvents(this List<EntityChange> entityChange)
        {
            return entityChange.Select(GetEntityChangeEvent).ToList();
        }

        private static IEvent GetEntityChangeEvent(EntityChange entityChange)
        {

            var entity = entityChange.Entity;
            if (entity != null)
            {

                var entityType = entity.GetType();
                var modelEvent = entityType;
                var attribute = entityType.GetCustomAttributes(typeof(EntityEventAttribute), false).FirstOrDefault();
                if (attribute != null)
                    modelEvent = ((EntityEventAttribute)attribute).AllEvent;

                Type entityEventType = null;
                switch (entityChange.State)
                {
                    case EntityState.Added:
                        if (attribute != null && ((EntityEventAttribute)attribute).CreateEvent != null)
                            modelEvent = ((EntityEventAttribute)attribute).CreateEvent;
                        entityEventType = typeof(EntityCreatedEvent<>);
                        break;
                    case EntityState.Modified:
                        if (attribute != null && ((EntityEventAttribute)attribute).UpdateEvent != null)
                            modelEvent = ((EntityEventAttribute)attribute).UpdateEvent;
                        entityEventType = typeof(EntityUpdatedEvent<>);
                        break;
                    case EntityState.Deleted:
                        if (attribute != null && ((EntityEventAttribute)attribute).DeleteEvent != null)
                            modelEvent = ((EntityEventAttribute)attribute).DeleteEvent;
                        entityEventType = typeof(EntityDeletedEvent<>);
                        break;

                }

                if (attribute != null && modelEvent != null)
                {
                    var entityEvent = entityEventType.MakeGenericType(modelEvent);
                    return (IEvent)Activator.CreateInstance(entityEvent, entity.Map(entityType, modelEvent), entityChange.DoBy,
                        entityChange.DoDate);
                }
                else if (attribute == null)
                {
                    var entityEvent = entityEventType.MakeGenericType(entityType);
                    return (IEvent)Activator.CreateInstance(entityEvent, entity, entityChange.DoBy,
                       entityChange.DoDate);
                }

            }
            return null;
        }
    }
}
