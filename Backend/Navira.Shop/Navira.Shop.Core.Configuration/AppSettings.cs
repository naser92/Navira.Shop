using Navira.Shop.Core.Extensions;

namespace Navira.Shop.Core.Configuration
{
    public partial class AppSettings
    {
        public ScalarConfig Scalar { get; set; }
        public SystemInfo SystemInfo { get; set; }
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public KeycloakConfig Keycloak { get; set; }
        public CacheConfig CacheConfig { get; set; } = new();
        public RedisConfig RedisConfig { get; set; } = new();
        public List<string> IgnoreCheckPath { get; set; } = new();
        public IDictionary<string, string> Values { get; set; }
        public string GetValue(string name)
        {
            if (Values.IsNullOrEmpty())
                return null;
            if (Values.ContainsKey(name))
                return Values[name];
            else return null;
        }
        public T GetValue<T>(string name)
        {
            var value = GetValue(name);
            if (value.IsNullOrWhiteSpace())
                return default;
            return CommonHelper.To<T>(value);
        }
        public T GetValue<T>(string name, T defaultvalue)
        {
            var value = GetValue(name);
            if (value.IsNullOrWhiteSpace())
                return defaultvalue ?? default;
            return CommonHelper.To<T>(value);
        }
        public IDictionary<string, RestConfig> RestsConfig { get; set; }
        public bool ShowErrorMessage { get; set; } = true;
        public string Issuer { get; set; } = "WindowLink.Security.Bearer";
        public string Audience { get; set; } = "WindowLink.Security.Bearer.User";
        public string SigningKey { get; set; } = "1234567890123456789012345678901234567890";
        public string EncryptionKey { get; set; } = "D7CE1D4D9E7F403";
    }
}
