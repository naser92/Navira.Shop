using Navira.Shop.Application.Common;
using Navira.Shop.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Application.Identity
{
    public class MenuModel : BaseReadModel<int>, IAuditableEntity
    {

        public int? ParentId { get; set; }

        public int PermissionId { get; set; }

        public string Title { get; set; }

        public string Route { get; set; }

        public string Icon { get; set; }

        public int SortOrder { get; set; }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParentId")]
        public virtual MenuModel Parent { get; set; }

        [ForeignKey("PermissionId")]
        public virtual PermissionModel Permission { get; set; }

        public virtual ICollection<MenuModel> Childs { get; set; }

    }
}
