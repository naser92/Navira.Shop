namespace Navira.Shop.Core.Security
{
    public class MenuAttribute : Attribute
    {

        public string Title { get; set; }
        public string Icon { get; set; }
        public string CodePermission { get; set; }
        public string Route { get; set; }
        public MenuAttribute(string codePermission, string title)
        {
            CodePermission = codePermission;
            Title = title;
        }

    }
}
