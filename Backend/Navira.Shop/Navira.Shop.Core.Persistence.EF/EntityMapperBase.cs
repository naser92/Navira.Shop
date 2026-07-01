using GreenPipes.Internals.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;
using Navira.Shop.Core.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Core.Persistence.EF
{
    public class EntityMapperBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity<TKey>//,ISoftDeletableEntity,IHasRowVersion
    {
        //public Type MapperType => typeof(TEntity);

        public virtual void Configure(EntityTypeBuilder<TEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            //if(typeof(TKey).IsNumericType())
            //    entityBuilder.Property(t => t.Id).UseHiLo(typeof(TEntity).Name + "Key");

            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(ISoftDeletableEntity)))
                entityBuilder.Property("IsDeleted").HasDefaultValue(false);
            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(IHasPriority)))
                entityBuilder.Property("Priority").HasDefaultValue(0);
            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(IHasRowVersion)))
                entityBuilder.Property("RowVersion").IsRowVersion();

        }
    }
    public class EntityReadMapperBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity<TKey>
    {

        public virtual void Configure(EntityTypeBuilder<TEntity> entityBuilder)
        {
            var table = typeof(TEntity).GetAttribute<TableAttribute>();
            if (table.IsNullOrEmpty())
                entityBuilder.ToTable(typeof(TEntity).Name.Replace("Model", ""));

            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Id).ValueGeneratedNever();
            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(ISoftDeletableEntity)))
                entityBuilder.Property("IsDeleted").HasDefaultValue(false);
            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(IHasPriority)))
                entityBuilder.Property("Priority").HasDefaultValue(0);
            if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(IHasRowVersion)))
                entityBuilder.Property("RowVersion").IsRowVersion();

        }
    }
}
