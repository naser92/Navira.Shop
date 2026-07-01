using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Navira.Shop.Core.Entity;
using System.Linq.Expressions;
using System.Reflection;

namespace Navira.Shop.Core.Persistence.EF
{
    public static class GlobalFiltersManager
    {
        public static void ApplyMemoryOptimizedTables(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                .GetEntityTypes()
                .Where(eType => typeof(IMemoryOptimize).IsAssignableFrom(eType.ClrType)))
            {
                modelBuilder.Entity(entityType.Name, a => a.ToTable(x => x.IsMemoryOptimized()));
            }
        }

        public static void ApplySoftDeleteQueryFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                .GetEntityTypes()
                .Where(eType => typeof(ISoftDeletableEntity).IsAssignableFrom(eType.ClrType)))
            {
                entityType.addSoftDeleteQueryFilter();
            }
        }

        private static void addSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(GlobalFiltersManager)
                .GetMethod(nameof(getSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, new object[] { });
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression getSoftDeleteFilter<TEntity>() where TEntity : ISoftDeletableEntity
        {
            return (Expression<Func<TEntity, bool>>)(entity => !(bool)entity.IsDeleted);
        }
    }
}
