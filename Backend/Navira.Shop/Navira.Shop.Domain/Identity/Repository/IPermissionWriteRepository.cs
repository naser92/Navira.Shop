using Navira.Shop.Core.Persistence.EF;

namespace Navira.Shop.Domain.Identity
{
    public interface IPermissionWriteRepository : IWriteRepository<Permission, int>
    {
    }
}
