using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Security;
using System.Security.Claims;

namespace Navira.Shop.Core.Domain
{
    public class AppShadowProperties
    {
        //public string UserAgent { set; get; }
        // public string UserIp { set; get; }
        public DateTime Now { set; get; }
        public Guid UserId { set; get; }
    }

    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// and http://www.dotnettips.info/post/2507
    /// and http://www.dotnettips.info/post/2232
    /// </summary>
    public static class AuditableShadowProperties
    {
        // public static readonly Func<object, Guid> EFPropertyCreatedByUserId =entity => EF.Property<Guid>(entity, CreatedByUserId);
        public static readonly string CreatedByUserId = nameof(CreatedByUserId);

        //public static readonly Func<object, DateTime?> EFPropertyCreatedDate =entity => EF.Property<DateTime?>(entity, CreatedDate);
        public static readonly string CreatedDate = nameof(CreatedDate);


        //public static readonly Func<object, DateTime?> EFPropertyModifiedDateTime =entity => EF.Property<DateTime?>(entity, ModifiedDate);
        public static readonly string ModifiedDate = nameof(ModifiedDate);

        //public static readonly Func<object, Guid?> EFPropertyModifiedByUserId =entity => EF.Property<Guid?>(entity, ModifiedByUserId);
        public static readonly string ModifiedByUserId = nameof(ModifiedByUserId);


        // public static readonly Func<object, DateTime?> EFPropertyDeletedDateTime =entity => EF.Property<DateTime?>(entity, DeletedDate);
        public static readonly string DeletedDate = nameof(DeletedDate);

        // public static readonly Func<object, Guid?> EFPropertyDeletedByUserId =entity => EF.Property<Guid?>(entity, DeletedByUserId);
        public static readonly string DeletedByUserId = nameof(DeletedByUserId);

        //public static readonly Func<object, Guid?> EFPropertyIsDeleted =entity => EF.Property<Guid?>(entity, IsDeleted);
        public static readonly string IsDeleted = nameof(IsDeleted);

        public static void AddAuditableShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => typeof(IAuditableEntity).IsAssignableFrom(e.ClrType)))
            {

                if (entityType.ClrType.IsAssignableToGeneric(typeof(ICreateAuditableEntity<>)))
                {
                    var createdByUserIdType = entityType.ClrType.GetInheritedGenericArgumentType(typeof(ICreateAuditableEntity<>));
                    if (createdByUserIdType.IsNotNull())
                    {
                        modelBuilder.Entity(entityType.ClrType)
                                    .Property(createdByUserIdType, CreatedByUserId);

                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime>(CreatedDate);
                    }
                }
                if (entityType.ClrType.IsAssignableToGeneric(typeof(IModifyAuditableEntity<>)))
                {
                    var modifiedByUserIdType = entityType.ClrType.GetInheritedGenericArgumentType(typeof(IModifyAuditableEntity<>));
                    if (modifiedByUserIdType.IsNotNull())
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(modifiedByUserIdType, ModifiedByUserId);

                        modelBuilder.Entity(entityType.ClrType)
                                    .Property<DateTime?>(ModifiedDate);
                    }
                }
                if (entityType.ClrType.IsAssignableToGeneric(typeof(IDeleteAuditableEntity<>)))
                {
                    var deletedByUserIdType = entityType.ClrType.GetInheritedGenericArgumentType(typeof(IDeleteAuditableEntity<>));
                    if (deletedByUserIdType.IsNotNull())
                    {
                        modelBuilder.Entity(entityType.ClrType)
                    .Property(deletedByUserIdType, DeletedByUserId);


                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime?>(DeletedDate);

                        modelBuilder.Entity(entityType.ClrType)
                            .Property<bool>(IsDeleted);
                    }
                }
            }
        }

        /// <summary>
        /// More info: http://www.dotnettips.info/post/2507
        /// </summary>
        public static void SetAuditableEntityPropertyValues(
            this ChangeTracker changeTracker,
            AppShadowProperties props)
        {
            if (props == null)
            {
                return;
            }

            foreach (var entry in changeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.SetAddedShadowProperties(props);
                        break;
                    case EntityState.Modified:
                        entry.SetModifiedShadowProperties(props);
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged; //NOTE: For soft-deletes to work with the original `Remove` method.
                        entry.SetDeleteShadowProperties(props);
                        break;
                }
                ;
            }
        }

        public static void SetAddedShadowProperties(this EntityEntry<IAuditableEntity> addedEntry, AppShadowProperties props)
        {
            if (props == null && !typeof(ICreateAuditableEntity<>).IsAssignableFrom(addedEntry.GetType()))
                return;

            addedEntry.Property(CreatedDate).CurrentValue = props.Now;

            addedEntry.Property(CreatedByUserId).CurrentValue = props.UserId;
        }

        public static void SetDeleteShadowProperties(this EntityEntry<IAuditableEntity> deleteEntry, AppShadowProperties props)
        {
            if (props == null && !typeof(IDeleteAuditableEntity<>).IsAssignableFrom(deleteEntry.GetType()))
                return;

            HandleDependent(deleteEntry, props);
        }

        private static void HandleDependent(EntityEntry entry, AppShadowProperties props)
        {

            entry.Property(IsDeleted).CurrentValue = true;
            entry.Property(DeletedDate).CurrentValue = props.Now;

            entry.Property(DeletedByUserId).CurrentValue = props.UserId;
        }

        public static void SetModifiedShadowProperties(this EntityEntry<IAuditableEntity> modifiedEntry, AppShadowProperties props)
        {
            if (props == null && !typeof(IModifyAuditableEntity<>).IsAssignableFrom(modifiedEntry.GetType()))
                return;

            modifiedEntry.Property(ModifiedDate).CurrentValue = props.Now;

            modifiedEntry.Property(ModifiedByUserId).CurrentValue = props.UserId;
        }

        public static AppShadowProperties GetShadowProperties(this IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                return null;
            }

            var httpContext = httpContextAccessor?.HttpContext;
            return new AppShadowProperties
            {

                UserId = GetUserId(httpContext),
                Now = DateTime.Now
            };
        }

        public static Guid GetUserId(HttpContext httpContext)
        {
            var userId = Guid.Empty;
            var claimsIdentity = ((ClaimsIdentity)httpContext?.User?.Identity);
            if (claimsIdentity is null)
                return userId;
            if (!claimsIdentity.Claims.Any())
                return userId;

            var value = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "sd")?.Value;
            var sensitiveData = TokenSensitiveData.Decrypt(value);
            if (!sensitiveData.UserId.IsNull())
            {
                userId = sensitiveData.UserId;
            }
            return userId;
        }
    }
}