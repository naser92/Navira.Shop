using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class UserAccessInfoQueryHandler : CommandHandler, IQueryHandler<UserAccessInfoCommand, UserAccessInfoDto>
    {
        private readonly IMenuQueryService _menuQueryService;
        public UserAccessInfoQueryHandler(IUnitOfWork uow, IMenuQueryService menuQueryService) : base(uow)
        {
            _menuQueryService = menuQueryService;
        }

        public async Task<IResult<UserAccessInfoDto>> Handle(UserAccessInfoCommand query, CancellationToken cancellationToken = default)
        {
            var menu = await _menuQueryService.GetMenu();

            return new UserAccessInfoDto()
            {
                Menus = menu,

            }.SuccessResult();
        }
    }
}
