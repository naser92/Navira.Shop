using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;

namespace Navira.Shop.Application.Identity
{
    public class PermissionModel : BaseReadModel<int>, IAuditableEntity
    {

        public int BaseSubSystemId { get; set; }

        public string ControllerName { get; set; }

        public string Scope { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<PolicyPermissionModel> PolicyPermission { get; set; }


        public virtual ICollection<MenuModel> Menu { get; set; }


    }
}
