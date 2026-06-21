using Navira.Shop.Core.Extensions;

namespace Navira.Shop.Core.Configuration
{
    public class ScalarConfig
    {
        public string Version { get; set; } = "v1";
        public string UrlPrefix { get; set; }
        public bool Enabled { get; set; } = true;
        public string GetUrlPrefix => UrlPrefix.IsNotNullOrWhiteSpace() ? "/" + UrlPrefix.ToLower() : "";
    }
}
