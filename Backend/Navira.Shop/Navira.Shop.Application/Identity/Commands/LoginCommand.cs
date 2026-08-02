using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Auth
{
    public class LoginCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }


    }
}
