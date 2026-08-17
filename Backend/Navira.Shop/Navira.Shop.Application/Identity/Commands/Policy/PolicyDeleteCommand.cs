using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Identity
{
    public class PolicyDeleteCommand : BaseDto<int>, ICommand
    {

        public PolicyDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
