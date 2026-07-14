using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Auth.Handlers
{
    public class LoginCommandHandler : CommandHandler, IQueryHandler<LoginCommand, AuthTokenDto>
    {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IUnitOfWork uow, IAuthService authService) : base(uow)
        {

            _authService = authService;
        }

        public async Task<IResult<AuthTokenDto>> Handle(LoginCommand query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _authService.LoginAsync(query, cancellationToken).ResultAsync();
            }
            catch (Exception ex)
            {
                AuthTokenDto result = null;
                return result.FailResult(ex.Message);
            }

        }
    }
}
