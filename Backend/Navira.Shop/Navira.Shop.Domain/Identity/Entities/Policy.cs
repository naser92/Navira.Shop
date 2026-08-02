using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;

namespace Navira.Shop.Domain.Identity
{
    public class Policy : AggregateRoot<int>, IFullAuditableEntity<Guid>, ISoftDeletableEntity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<PolicyPermission> PolicyPermission { get; set; }

        public virtual ICollection<RolePolicy> RolePolicy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
