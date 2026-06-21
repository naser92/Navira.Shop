namespace Navira.Shop.Core.Configuration
{
    public class SystemInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string[] AllowOrigins { get; set; } = new string[0];
    }
}
