using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;

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
        public Guid UserId => GetCurrentUserInfo().UserId.Value;
        public bool IsAdmin => GetCurrentUserInfo().IsAdmin;
        public Guid? TraceId => GetCurrentUserInfo()?.TraceId;
        public int? UserType => GetCurrentUserInfo()?.UserType;

        public UserInfoDto GetCurrentUserInfo()
        {
            TokenSensitiveData sensitiveData = null;
            JwtSecurityToken claimsIdentity = null;

            var token = GrtTokenFromHeader();
            if (string.IsNullOrWhiteSpace(token))
                token = GrtTokenQuery();
            //if (token.IsNotNullOrEmpty())
            //    token = GrtTokenFromBody();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    claimsIdentity = handler.ReadJwtToken(token);
                }
            }


            if ((claimsIdentity?.Claims).IsNullOrEmpty())
                return new UserInfoDto();

            var sensitiveDataContent = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "sd")?.Value;
            var Jti = claimsIdentity.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var userName = claimsIdentity.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

            sensitiveData = TokenSensitiveData.Decrypt(sensitiveDataContent);


            return new UserInfoDto()
            {
                UserId = sensitiveData.UserId,
                IsAdmin = sensitiveData.IsAdmin,
                UserType = sensitiveData.UserType,
                TraceId = Jti.TryParseGuid(),


            };
        }

        private string GrtTokenQuery()
        {
            return _httpContextAccessor.HttpContext?.Request?.Query["token"];
        }
        private string GrtTokenFromHeader()
        {
            string authorizationHeader = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length);
                // Process the token
            }
            return null;
        }


    }
}

