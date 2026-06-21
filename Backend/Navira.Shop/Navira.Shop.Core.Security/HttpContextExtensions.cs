using Microsoft.AspNetCore.Http;
using Navira.Shop.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Navira.Shop.Core.Security
{
    public static class HttpContextExtensions
    {
        public static (TokenSensitiveData, string) GetSensitiveDate(this HttpContext httpContext)
        {
            var claimsIdentity = ((ClaimsIdentity)httpContext?.User?.Identity);
            var userName = "";
            TokenSensitiveData sensitiveData = null;
            if (claimsIdentity.IsNotNull() && claimsIdentity.Claims.IsNotNullOrEmpty())
            {
                var sensitiveDataContent = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "sd")?.Value;
                userName = claimsIdentity.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
                sensitiveData = TokenSensitiveData.Decrypt(sensitiveDataContent);
            }
            return (sensitiveData, userName);
        }
    }
}
