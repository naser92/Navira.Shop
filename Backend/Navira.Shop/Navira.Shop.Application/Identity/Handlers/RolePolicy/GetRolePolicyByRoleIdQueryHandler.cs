using Navira.Shop.Application.Identity.ServicesContract;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class GetRolePolicyByRoleIdQueryHandler : CommandHandler, IQueryHandler<GetRolePolicyByRoleIdCommand, IEnumerable<RolePolicyGetByRoleIdDto>>
    {
        private readonly IRolePolicyQueryService _rolePolicyQueryService;
        public GetRolePolicyByRoleIdQueryHandler(IUnitOfWork uow, IRolePolicyQueryService rolePolicyQueryService) : base(uow)
        {
            _rolePolicyQueryService = rolePolicyQueryService;
        }

        public async Task<IResult<IEnumerable<RolePolicyGetByRoleIdDto>>> Handle(GetRolePolicyByRoleIdCommand query, CancellationToken cancellationToken = default)
        {
            return await _rolePolicyQueryService.GetByRoleId(query.RoleId).ResultAsync();
        }
    }
}
