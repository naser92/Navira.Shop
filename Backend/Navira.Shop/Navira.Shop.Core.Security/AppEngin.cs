using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Navira.Shop.Core.Security
{
    public class AppEngin : IAppEngin
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppEngin(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;
        public Guid UserId => GetCurrentUserInfo().UserId ?? Guid.Empty;
        public bool IsAdmin => GetCurrentUserInfo().IsAdmin;
        public Guid? TraceId => GetCurrentUserInfo().TraceId;
        public int? UserType => GetCurrentUserInfo().UserType;

        public UserInfoDto GetCurrentUserInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return new UserInfoDto();

            var userInfo = new UserInfoDto();

            var userIdValue =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                user.FindFirst("sub")?.Value;

            if (!string.IsNullOrWhiteSpace(userIdValue))
                userInfo.UserId = userIdValue.TryParseGuid();

            var traceIdValue =
                user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ??
                user.FindFirst("jti")?.Value;

            if (!string.IsNullOrWhiteSpace(traceIdValue))
                userInfo.TraceId = traceIdValue.TryParseGuid();

            var userTypeValue = user.FindFirst("user_type")?.Value;
            if (!string.IsNullOrWhiteSpace(userTypeValue) && int.TryParse(userTypeValue, out var userType))
                userInfo.UserType = userType;

            userInfo.IsAdmin =
                user.IsInRole("admin") ||
                user.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "admin") ||
                user.Claims.Any(x => x.Type == "role" && x.Value == "admin");




            userInfo.Roles = user.Claims
                .Where(x =>
                    x.Type == ClaimTypes.Role ||
                    x.Type.Equals("roles", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();







            return userInfo;
        }

    }
}

