using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Auth
{
    public class RefreshTokenCommand : ICommand
    {
        public string RefreshToken { get; set; }

    }
}
