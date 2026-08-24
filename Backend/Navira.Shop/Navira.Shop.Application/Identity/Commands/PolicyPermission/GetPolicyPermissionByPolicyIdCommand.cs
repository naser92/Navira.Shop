using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Identity
{
    public class GetPolicyPermissionByPolicyIdCommand : ICommand
    {
        public int PolicyId { get; set; }
    }
}
