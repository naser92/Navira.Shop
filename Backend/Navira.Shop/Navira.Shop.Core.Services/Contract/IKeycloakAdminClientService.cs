using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navira.Shop.Core.Services
{
    public interface IKeycloakAdminClientService
    {
        Task<string> EnsureResourceAsync(string resourceName);
        Task EnsureScopeAsync(string scopeName);
        Task AttachScopeToResourceAsync(string resourceId, string scopeName);
    }
}
