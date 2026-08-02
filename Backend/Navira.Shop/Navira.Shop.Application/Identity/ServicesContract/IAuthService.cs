using Navira.Shop.Application.Auth;
using Navira.Shop.Core.Service;

namespace Navira.Shop.Application.Identity
{
    public interface IAuthService : IBaseService
    {
        Task<AuthTokenDto> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
        Task<AuthTokenDto> RefreshAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default);
    }
}
