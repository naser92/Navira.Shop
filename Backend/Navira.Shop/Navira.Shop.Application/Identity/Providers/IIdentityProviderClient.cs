namespace Navira.Shop.Application.Auth
{
    public interface IIdentityProviderClient
    {
        Task<AuthTokenDto> LoginAsync(string username, string password, CancellationToken cancellationToken);
        Task<AuthTokenDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
