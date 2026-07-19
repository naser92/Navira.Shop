using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Auth.Handlers
{
    public class RefreshTokenCommandHandler : CommandHandler, IQueryHandler<RefreshTokenCommand, AuthTokenDto>
    {
        private readonly IAuthService _authService;
        public RefreshTokenCommandHandler(IUnitOfWork uow, IAuthService authService) : base(uow)
        {
            _authService = authService;
        }

        public async Task<IResult<AuthTokenDto>> Handle(RefreshTokenCommand query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _authService.RefreshAsync(query, cancellationToken).ResultAsync();
            }
            catch (Exception ex)
            {
                var result = new AuthTokenDto();
                return result.FailResult(ex.Message);
            }
        }
    }
}
