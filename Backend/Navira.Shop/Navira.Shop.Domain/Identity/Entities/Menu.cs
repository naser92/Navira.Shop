using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navira.Shop.Domain.Identity
{
    public class Menu : AggregateRoot<int>, IFullAuditableEntity<Guid>, ISoftDeletableEntity
    {
        [ForeignKey("ParentId")]
        public virtual Menu Parent { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        public int? ParentId { get; set; }

        public int PermissionId { get; set; }

        public string Title { get; set; }

        public string Route { get; set; }

        public string Icon { get; set; }

        public int SortOrder { get; set; }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Menu> Childs { get; set; }
        public bool IsDeleted { get; set; }

        public Menu() { }
        public Menu(int? parentId, int permissionId, int? sortOrder, bool isVisible, bool isActive)
        {
            ParentId = parentId;
            PermissionId = permissionId;
            SortOrder = sortOrder ?? 0;
            IsVisible = isVisible;
            IsActive = isActive;
            IsDeleted = false;
        }

        public static Menu Create(int? parentId, int permissionId, int? sortOrder, bool? isVisible = null, bool? isActive = null)
        {
            return new Menu(
               parentId,
               permissionId,
               sortOrder,
               isVisible is null ? true : isVisible.Value,
               isActive is null ? true : isActive.Value);
        }

        public void ChangeVisible(bool isVisible) => IsVisible = isVisible;
        public void ChangeStatus(bool isActive) => IsActive = isActive;
        public void SetTitle(string title) => Title = title;
        public void SetIcon(string icon) => Icon = icon;
        public void SetRoute(string route) => Route = route;
        public void Delete() => IsDeleted = true;
    }
}
