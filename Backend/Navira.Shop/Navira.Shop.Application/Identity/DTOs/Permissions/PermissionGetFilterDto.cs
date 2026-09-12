using Navira.Shop.Application.Common;

namespace Navira.Shop.Application.Identity
{
    public class PermissionGetFilterDto : PaginationModel
    {
        public string ControllerName { get; set; }
        public string Scope { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }
    }
}
