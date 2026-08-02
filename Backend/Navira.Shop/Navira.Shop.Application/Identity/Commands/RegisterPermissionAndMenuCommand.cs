using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Identity
{
    public class RegisterPermissionAndMenuCommand : ICommand
    {
        public RegisterMenuAndPermissionEvent data { get; set; }
    }
}
