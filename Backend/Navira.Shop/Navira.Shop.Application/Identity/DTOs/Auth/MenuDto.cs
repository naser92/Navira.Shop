using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Identity
{
    public class MenuDto : BaseDto<int>
    {
        public int PermissionId { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }
        public string Icone { get; set; }
        public int SortOrder { get; set; }
        public List<MenuDto> Childs { get; set; }
    }
}
