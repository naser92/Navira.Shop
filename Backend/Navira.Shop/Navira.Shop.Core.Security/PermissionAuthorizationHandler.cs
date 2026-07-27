using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Navira.Shop.Core.Security
{
    public sealed class PermissionAuthorizationHandler
     : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionProvider _permissionProvider;


        public PermissionAuthorizationHandler(IPermissionProvider permissionProvider)
        {
            _permissionProvider = permissionProvider;
        }


        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {

            if (!context.User.Identity!.IsAuthenticated)
            {
                return;
            }


            var roles = context.User
                .FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();


            if (!roles.Any())
            {
                return;
            }


            foreach (var role in roles)
            {
                var permissions =
                    await _permissionProvider
                    .GetPermissionsAsync(role);


                if (permissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
