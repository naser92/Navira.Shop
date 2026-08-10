using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Identity
{
    public class Policy : FullEntity<int>, IFullAuditableEntity<Guid>
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
