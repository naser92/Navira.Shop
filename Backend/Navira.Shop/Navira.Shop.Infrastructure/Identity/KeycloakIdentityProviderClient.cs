using Navira.Shop.Application.Auth;
using Navira.Shop.Application.Identity;
using Navira.Shop.Application.Identity.DTOs;
using Navira.Shop.Core.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Navira.Shop.Infrastructure.Identity
{
    public class KeycloakIdentityProviderClient : IIdentityProviderClient
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakConfig _config;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };


        public KeycloakIdentityProviderClient(HttpClient httpClient, AppSettings appSettings)
        {
            _httpClient = httpClient;
            _config = appSettings.KeycloakConfig;

        }

        public Task<AuthTokenDto> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _config.ClientId,
                ["client_secret"] = _config.ClientSecret,
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
                ["client_id"] = _config.ClientId,
                ["client_secret"] = _config.ClientSecret,
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

        public async Task<IReadOnlyList<KeycloakRoleDto>> GetRealmRolesAsync(CancellationToken cancellationToken = default)
        {
            var accessToken = await GetAdminAccessTokenAsync(cancellationToken);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl.TrimEnd('/')}/admin/realms/{Uri.EscapeDataString(_config.Realm)}/roles");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                throw new HttpRequestException(
                    $"Keycloak GetRealmRoles failed. " +
                    $"StatusCode: {(int)response.StatusCode}. " +
                    $"Response: {error}");
            }

            await using var stream =
            await response.Content.ReadAsStreamAsync(
                cancellationToken);

            var roles =
                await JsonSerializer.DeserializeAsync<
                    List<KeycloakRoleDto>>(
                    stream,
                    JsonOptions,
                    cancellationToken);

            return roles ?? [];

        }

        private async Task<string> GetAdminAccessTokenAsync(CancellationToken cancellationToken)
        {



            var tokenUrl =
                $"{_config.BaseUrl.TrimEnd('/')}/realms/{Uri.EscapeDataString(_config.Realm)}/protocol/openid-connect/token";

            using var content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _config.ClientId,
                    ["client_secret"] = _config.ClientSecret,
                });

            using var response = await _httpClient.PostAsync(
                tokenUrl,
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                throw new HttpRequestException(
                    $"Keycloak Admin Token request failed. " +
                    $"StatusCode: {(int)response.StatusCode}. " +
                    $"Response: {error}");
            }

            await using var stream =
                await response.Content.ReadAsStreamAsync(
                    cancellationToken);

            var tokenResponse =
                await JsonSerializer.DeserializeAsync<KeycloakTokenResponse>(
                    stream,
                    JsonOptions,
                    cancellationToken);

            if (tokenResponse == null ||
                string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new InvalidOperationException(
                    "Keycloak did not return an access token.");
            }

            return tokenResponse.AccessToken;
        }
        private string BuildTokenEndpoint()
        {
            var baseUrl = _config.BaseUrl.TrimEnd('/');
            var realm = _config.Realm;

            return $"{baseUrl}/realms/{realm}/protocol/openid-connect/token";
        }


    }
}
