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

            var result = await _authService.LoginAsync(query, cancellationToken);
            if (!string.IsNullOrWhiteSpace(result.Error))
                throw new ResultException("نام کاربری و یا کلمه عبور اشتباه است");

            return result.SuccessResult();


        }
    }
}
