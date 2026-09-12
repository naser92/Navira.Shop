using Navira.Shop.Application.Common;

namespace Navira.Shop.Application.Identity
{
    public class PolicyGetFilterDto : PaginationModel
    {

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsActive { get; set; }
    }
}
