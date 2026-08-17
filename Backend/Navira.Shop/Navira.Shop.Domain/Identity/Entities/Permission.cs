using Navira.Shop.Core.Domain;

namespace Navira.Shop.Domain.Identity
{
    public class Permission : AggregateRoot<int>, IFullAuditableEntity<Guid>
    {
        public int BaseSubSystemId { get; set; }

        public string ControllerName { get; set; }

        public string Scope { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
        public virtual ICollection<PolicyPermission> PolicyPermission { get; set; }

        public virtual ICollection<Menu> Menu { get; set; }


        public Permission() { }

        public Permission(int baseSubSystemId, string controllerName, string scope, string code, string title, string description, bool isActive, bool isDeleted)
        {
            BaseSubSystemId = baseSubSystemId;
            ControllerName = controllerName;
            Scope = scope;
            Code = code;
            Title = title;
            Description = description;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }

        public static Permission Create(string scope, string controllerName, string title, string code = null)
        {
            if (string.IsNullOrEmpty(scope) || string.IsNullOrEmpty(controllerName))
                throw new DomainException("دیتا معتبر نیست");

            return new Permission(
                1,
                controllerName,
                scope,
                string.IsNullOrWhiteSpace(code) ? $"{controllerName}.{scope}" : code,
                title,
                null,
                true,
                false
                );
        }

        public void ChangeTitle(string title) => Title = title.Trim();

        public void AddDescription(string description) => Description = description.Trim();
    }
}
