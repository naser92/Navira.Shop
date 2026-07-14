using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Navira.Shop.Core.Security
{
    public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!string.IsNullOrWhiteSpace(policyName) &&
                policyName.StartsWith($"{PermissionRequirement.PolicyPrefix}:", StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName.Substring($"{PermissionRequirement.PolicyPrefix}:".Length);

                var policy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Bearer")
                    .RequireAuthenticatedUser()
                    .AddRequirements(new PermissionRequirement(permission))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return base.GetPolicyAsync(policyName);
        }
    }
}
