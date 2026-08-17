using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navira.Shop.Core.Configuration;
using Navira.Shop.Core.Security;
using System.Security.Claims;
using System.Text.Json;

namespace Navira.Shop.Core.Web
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, AppSettings appSettings, IWebHostEnvironment environment)
        {
            var keycloak = appSettings?.KeycloakConfig
                    ?? throw new InvalidOperationException("Keycloak configuration is missing.");



            var authority = BuildAuthority(keycloak.BaseUrl, keycloak.Realm);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = keycloak.ClientId;

                    options.MapInboundClaims = false;
                    options.SaveToken = false;

                    options.RequireHttpsMetadata = !environment.IsDevelopment();
                    options.IncludeErrorDetails = environment.IsDevelopment();
                    options.RefreshOnIssuerKeyNotFound = true;

                    options.TokenValidationParameters = new()
                    {
                        NameClaimType = "preferred_username",
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            if (context.Principal?.Identity is ClaimsIdentity identity)
                            {
                                MapKeycloakRoles(identity, keycloak.ClientId);
                            }

                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            // TODO:
                            // ILogger.LogWarning(context.Exception,...)
                            var ex = context.Exception;

                            Console.WriteLine(ex.ToString());

                            return Task.CompletedTask;
                        },


                        OnChallenge = async context =>
                         {
                             context.HandleResponse();

                             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                             context.Response.ContentType = "application/json";

                             await context.Response.WriteAsJsonAsync(new
                             {
                                 status = 401,
                                 error = "Unauthorized bbb"
                             });
                         },

                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(new
                            {
                                status = 403,
                                error = "Forbidden"
                            });
                        }

                    };
                });



            return services;
        }

        private static string BuildAuthority(string baseUrl, string realm)
        {
            return $"{baseUrl.TrimEnd('/')}/realms/{realm}";
        }

        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddSingleton<
                IAuthorizationPolicyProvider,
                PermissionPolicyProvider>();

            services.AddScoped<
                IAuthorizationHandler,
                PermissionAuthorizationHandler>();

            return services;
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
