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

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            //if (!string.IsNullOrWhiteSpace(policyName) &&
            //    policyName.StartsWith($"{PermissionRequirement.PolicyPrefix}:", StringComparison.OrdinalIgnoreCase))
            //{
            //    var permission = policyName.Substring($"{PermissionRequirement.PolicyPrefix}:".Length);

            //    var policy = new AuthorizationPolicyBuilder()
            //        .AddAuthenticationSchemes("Bearer")
            //        .RequireAuthenticatedUser()
            //        .AddRequirements(new PermissionRequirement(permission))
            //        .Build();

            //    return Task.FromResult<AuthorizationPolicy?>(policy);
            //}

            //return base.GetPolicyAsync(policyName);

            // اگر Policyهای معمولی باشد
            var policy = await base.GetPolicyAsync(policyName);

            if (policy != null)
                return policy;

            // Permission Policy
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
        }
    }
}
