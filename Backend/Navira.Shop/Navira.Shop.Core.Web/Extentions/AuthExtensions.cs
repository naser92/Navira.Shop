using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Security;
using System.Security.Claims;
using System.Text.Json;

namespace Navira.Shop.Core.Web
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var keycloak = appSettings?.KeycloakConfig;

            if (keycloak == null ||
               string.IsNullOrWhiteSpace(keycloak.BaseUrl) ||
               string.IsNullOrWhiteSpace(keycloak.Realm) ||
               string.IsNullOrWhiteSpace(keycloak.ClientId))
            {
                return services;
            }



            var authority = BuildAuthority(keycloak.BaseUrl, keycloak.Realm);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = keycloak.ClientId;
                    options.RequireHttpsMetadata = !authority.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
                    options.SaveToken = true;
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authority,

                        ValidateAudience = true,
                        ValidAudience = keycloak.ClientId,

                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        NameClaimType = "preferred_username",
                        RoleClaimType = ClaimTypes.Role,

                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var identity = context.Principal?.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                MapKeycloakRoles(identity, context.Options.Audience);
                            }

                            return Task.CompletedTask;
                        }
                        //OnMessageReceived = context =>
                        //{
                        //    // backward compatibility with current AppEngin behavior
                        //    // current code also reads token from query string
                        //    var token = context.Request.Query["token"].FirstOrDefault();
                        //    if (!string.IsNullOrWhiteSpace(token))
                        //        context.Token = token;

                        //    return Task.CompletedTask;
                        //},

                        //OnTokenValidated = context =>
                        //{
                        //    if (context.Principal?.Identity is not ClaimsIdentity identity)
                        //        return Task.CompletedTask;

                        //    AddRealmRoles(identity, context.Principal);
                        //    AddResourceRoles(identity, context.Principal, keycloak.ClientId);

                        //    // normalize sub => NameIdentifier for easier use in app
                        //    var sub = context.Principal.FindFirst("sub")?.Value;
                        //    if (!string.IsNullOrWhiteSpace(sub) &&
                        //        !identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                        //    {
                        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, sub));
                        //    }

                        //    return Task.CompletedTask;
                        //},

                        //OnChallenge = context =>
                        //{
                        //    return Task.CompletedTask;
                        //},

                        //OnAuthenticationFailed = context =>
                        //{
                        //    return Task.CompletedTask;
                        //}
                    };
                });

            //services.AddAuthorization(options =>
            //{
            //    // fallback: authenticated user required unless [AllowAnonymous]
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }

        private static string BuildAuthority(string baseUrl, string realm)
        {
            var normalizedBaseUrl = baseUrl.TrimEnd('/');

            if (normalizedBaseUrl.EndsWith($"/realms/{realm}", StringComparison.OrdinalIgnoreCase))
                return normalizedBaseUrl;

            if (normalizedBaseUrl.EndsWith("/auth", StringComparison.OrdinalIgnoreCase))
                return $"{normalizedBaseUrl}/realms/{realm}";

            return $"{normalizedBaseUrl}/realms/{realm}";
        }

        private static void AddRealmRoles(ClaimsIdentity identity, ClaimsPrincipal principal)
        {
            var realmAccess = principal.FindFirst("realm_access")?.Value;
            if (string.IsNullOrWhiteSpace(realmAccess))
                return;

            try
            {
                using var doc = JsonDocument.Parse(realmAccess);

                if (doc.RootElement.TryGetProperty("roles", out var rolesElement) &&
                    rolesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var roleName = role.GetString();
                        if (string.IsNullOrWhiteSpace(roleName))
                            continue;

                        if (!identity.HasClaim(identity.RoleClaimType, roleName))
                            identity.AddClaim(new Claim(identity.RoleClaimType, roleName));
                    }
                }
            }
            catch
            {
                // optional: log
            }
        }

        private static void AddResourceRoles(
            ClaimsIdentity identity,
            ClaimsPrincipal principal,
            string clientId)
        {
            var resourceAccess = principal.FindFirst("resource_access")?.Value;
            if (string.IsNullOrWhiteSpace(resourceAccess))
                return;

            try
            {
                using var doc = JsonDocument.Parse(resourceAccess);

                if (!doc.RootElement.TryGetProperty(clientId, out var clientElement))
                    return;

                if (!clientElement.TryGetProperty("roles", out var rolesElement) ||
                    rolesElement.ValueKind != JsonValueKind.Array)
                    return;

                foreach (var role in rolesElement.EnumerateArray())
                {
                    var roleName = role.GetString();
                    if (string.IsNullOrWhiteSpace(roleName))
                        continue;

                    if (!identity.HasClaim(identity.RoleClaimType, roleName))
                        identity.AddClaim(new Claim(identity.RoleClaimType, roleName));
                }
            }
            catch
            {
                // optional: log
            }
        }

        private static void MapKeycloakRoles(ClaimsIdentity identity, string? clientId)
        {
            var claims = identity.Claims.ToList();

            foreach (var claim in claims)
            {
                if (claim.Type == "realm_access")
                {
                    AddRolesFromRealmAccess(identity, claim.Value);
                }

                if (claim.Type == "resource_access")
                {
                    AddRolesFromResourceAccess(identity, claim.Value, clientId);
                }
            }
        }

        private static void AddRolesFromRealmAccess(ClaimsIdentity identity, string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("roles", out var rolesElement) &&
                    rolesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var roleValue = role.GetString();
                        if (!string.IsNullOrWhiteSpace(roleValue) &&
                            !identity.HasClaim(ClaimTypes.Role, roleValue))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static void AddRolesFromResourceAccess(ClaimsIdentity identity, string json, string? clientId)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    return;

                foreach (var client in doc.RootElement.EnumerateObject())
                {
                    if (!string.IsNullOrWhiteSpace(clientId) &&
                        !string.Equals(client.Name, clientId, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (client.Value.TryGetProperty("roles", out var rolesElement) &&
                        rolesElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            var roleValue = role.GetString();
                            if (!string.IsNullOrWhiteSpace(roleValue) &&
                                !identity.HasClaim(ClaimTypes.Role, roleValue))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
