namespace Navira.Shop.Core.Security
{
    public interface IPermissionProvider
    {
        Task<HashSet<string>> GetPermissionsAsync(string role);
    }
}
