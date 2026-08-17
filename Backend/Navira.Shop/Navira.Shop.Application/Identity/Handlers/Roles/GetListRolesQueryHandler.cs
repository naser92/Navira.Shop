using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity.Handlers.Roles
{
    public class GetListRolesQueryHandler : CommandHandler, IQueryHandler<GetListRolesCommand, IReadOnlyList<KeycloakRoleDto>>
    {
        private readonly IRoleQueryService _roleQueryService;
        public GetListRolesQueryHandler(IUnitOfWork uow, IRoleQueryService roleQueryService) : base(uow)
        {
            _roleQueryService = roleQueryService;
        }

        public async Task<IResult<IReadOnlyList<KeycloakRoleDto>>> Handle(GetListRolesCommand query, CancellationToken cancellationToken = default)
        {
            return await _roleQueryService.GetList().ResultAsync();
        }
    }
}
