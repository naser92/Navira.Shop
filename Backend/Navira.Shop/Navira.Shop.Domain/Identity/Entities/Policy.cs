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

        public Policy() { }

        public Policy(string name, string title, string description, bool isSystem, bool isActive)
        {
            Name = name;
            Title = title;
            Description = description;
            IsSystem = isSystem;
            IsActive = isActive;
        }

        public static Policy Create(string name, string title, string description = null, bool isSystem = false, bool isActive = true)
        {
            return new Policy(
                name,
                title,
                description,
                isSystem,
                isActive
                    );
        }

        public void ChaneName(string name) => Name = name;
        public void ChaneTitle(string title) => Title = title;
        public void ChaneDescription(string description) => Description = description;
    }
}
