

using Navira.Shop.Core.Extensions;

namespace Navira.Shop.Core.Configuration
{
    public record RestConfig(string BaseUrl, string Token = null, string TokenKey = "secret-key", string HealthPath = "config/CheckConnection", bool HasApiPrefix = true)
    {
        public string Url => HasApiPrefix
            ? $"{BaseUrl.TrimEnd('/')}/api"
            : BaseUrl;
        public string HealthUrl => HealthPath.IsNullOrWhiteSpace()
            ? Url
            : $"{Url.TrimEnd('/')}/{HealthPath.TrimStart('/')}";
    }
}
