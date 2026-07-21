using Navira.Shop.Application.Auth;
using Navira.Shop.Core.Configuration;
using System.Text.Json;

namespace Navira.Shop.Infrastructure.Identity
{
    public class KeycloakIdentityProviderClient : IIdentityProviderClient
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public KeycloakIdentityProviderClient(HttpClient httpClient, AppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public Task<AuthTokenDto> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _appSettings.KeycloakConfig.ClientId,
                ["client_secret"] = _appSettings.KeycloakConfig.ClientSecret,
                ["username"] = username,
                ["password"] = password
            };

            var result = RequestTokenAsync(form, cancellationToken);
            return result;
        }

        public Task<AuthTokenDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = _appSettings.KeycloakConfig.ClientId,
                ["client_secret"] = _appSettings.KeycloakConfig.ClientSecret,
                ["refresh_token"] = refreshToken
            };

            return RequestTokenAsync(form, cancellationToken);
        }

        private async Task<AuthTokenDto> RequestTokenAsync(Dictionary<string, string> form, CancellationToken cancellationToken)
        {
            var tokenEndpoint = BuildTokenEndpoint();


            using var response = await _httpClient.PostAsync(
                tokenEndpoint,
                new FormUrlEncodedContent(form),
                cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            //if (!response.IsSuccessStatusCode)
            //    throw new UnauthorizedAccessException(content);

            var token = JsonSerializer.Deserialize<AuthTokenDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (token == null)
                throw new InvalidOperationException("Invalid token response from identity provider.");

            return token;
        }

        private string BuildTokenEndpoint()
        {
            var baseUrl = _appSettings.KeycloakConfig.BaseUrl.TrimEnd('/');
            var realm = _appSettings.KeycloakConfig.Realm;

            return $"{baseUrl}/realms/{realm}/protocol/openid-connect/token";
        }
    }
}
