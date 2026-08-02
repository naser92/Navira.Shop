using Navira.Shop.Core.Persistence.EF;
using Navira.Shop.Domain.Identity.Entities;

namespace Navira.Shop.Application.Identity
{
    public interface IRolePolicyWriteRepository : IWriteRepository<RolePolicy, int>
    {
    }
}
