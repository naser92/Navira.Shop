using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Navira.Shop.Core.Security
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
                return Task.CompletedTask;

            var requiredPermission = requirement.Permission?.Trim();
            if (string.IsNullOrWhiteSpace(requiredPermission))
                return Task.CompletedTask;

            var userPermissions = ExtractPermissions(context.User);

            if (userPermissions.Any(p => string.Equals(p, requiredPermission, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static HashSet<string> ExtractPermissions(ClaimsPrincipal user)
        {
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var claim in user.Claims)
            {
                if (string.IsNullOrWhiteSpace(claim.Value))
                    continue;

                switch (claim.Type)
                {
                    case "permission":
                    case "permissions":
                        AddSplitValues(permissions, claim.Value);
                        break;

                    case "scope":
                    case "scp":
                        AddSplitValues(permissions, claim.Value);
                        break;

                    case "realm_access":
                        AddRealmAccessRoles(permissions, claim.Value);
                        break;
                }

                if (claim.Type.StartsWith("resource_access", StringComparison.OrdinalIgnoreCase))
                {
                    AddResourceAccessRoles(permissions, claim.Value);
                }
            }

            // fallback برای بعضی مپینگ‌ها
            foreach (var roleClaim in user.FindAll(ClaimTypes.Role))
            {
                AddSplitValues(permissions, roleClaim.Value);
            }

            return permissions;
        }

        private static void AddSplitValues(HashSet<string> permissions, string value)
        {
            var parts = value
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
                permissions.Add(part);
        }

        private static void AddRealmAccessRoles(HashSet<string> permissions, string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("roles", out var rolesElement) &&
                    rolesElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        if (role.ValueKind == JsonValueKind.String)
                            permissions.Add(role.GetString()!);
                    }
                }
            }
            catch
            {
                // ignore malformed claim
            }
        }

        private static void AddResourceAccessRoles(HashSet<string> permissions, string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var client in doc.RootElement.EnumerateObject())
                    {
                        if (client.Value.TryGetProperty("roles", out var rolesElement) &&
                            rolesElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var role in rolesElement.EnumerateArray())
                            {
                                if (role.ValueKind == JsonValueKind.String)
                                    permissions.Add(role.GetString()!);
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore malformed claim
            }
        }
    }
}
