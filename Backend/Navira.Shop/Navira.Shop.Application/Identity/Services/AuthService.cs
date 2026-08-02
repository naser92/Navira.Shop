

using Navira.Shop.Application.Auth;

namespace Navira.Shop.Application.Identity
{
    public sealed class AuthService : IAuthService
    {
        private readonly IIdentityProviderClient _identityProviderClient;
        public AuthService(IIdentityProviderClient identityProviderClient)
        {
            _identityProviderClient = identityProviderClient;
        }

        public Task<AuthTokenDto> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            return _identityProviderClient.LoginAsync(command.Username, command.Password, cancellationToken);
        }

        public Task<AuthTokenDto> RefreshAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
        {
            return _identityProviderClient.RefreshAsync(command.RefreshToken, cancellationToken);
        }
    }
}
