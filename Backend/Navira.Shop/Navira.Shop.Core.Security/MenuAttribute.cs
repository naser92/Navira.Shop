namespace Navira.Shop.Core.Security
{
    public class MenuAttribute : Attribute
    {

        public string Title { get; set; }
        public string Icon { get; set; }
        public string CodePermission { get; set; }
        public string Action { get; set; }
        public int? SortOrder { get; init; }
        public MenuAttribute(string codePermission, string title)
        {
            CodePermission = codePermission;
            Title = title;
        }

        public MenuAttribute(string codePermission)
        {
            CodePermission = codePermission;
        }

    }
}
