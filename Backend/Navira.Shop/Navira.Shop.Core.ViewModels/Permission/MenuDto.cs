namespace Navira.Shop.Core.ViewModels
{
    public sealed class MenuDto
    {
        public string Title { get; init; } = default!;
        public string PermissionCode { get; init; } = default!;
        public MenuDto Parent { get; init; }
        public string Icon { get; init; }
        public string Route { get; init; }
        public int? SortOrder { get; init; }
    }
}
