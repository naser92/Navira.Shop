using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Identity
{
    public class GetRolePolicyByRoleIdCommand : ICommand
    {
        public Guid RoleId { get; set; }
    }
}
