namespace Navira.Shop.Core.Domain
{
    public interface IAuditableEntity
    { }
    public interface ICreateAuditableEntity<TKey> : IAuditableEntity
    {
    }
    public interface IModifyAuditableEntity<TKey> : IAuditableEntity
    {
    }
    public interface IDeleteAuditableEntity<TKey> : IAuditableEntity
    {
        public bool IsDeleted { get; set; }
    }
    public interface IFullAuditableEntity<TKey> : ICreateAuditableEntity<TKey>, IModifyAuditableEntity<TKey?>, IDeleteAuditableEntity<TKey?>
    {
    }

}