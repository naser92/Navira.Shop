using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity.Handlers.Roles
{
    public class GetListRolesQueryHandler : CommandHandler, IQueryHandler<GetListRolesCommand, RoleListDto>
    {
        private readonly IRoleQueryService _roleQueryService;
        public GetListRolesQueryHandler(IUnitOfWork uow, IRoleQueryService roleQueryService) : base(uow)
        {
            _roleQueryService = roleQueryService;
        }

        public async Task<IResult<RoleListDto>> Handle(GetListRolesCommand query, CancellationToken cancellationToken = default)
        {
            try
            {
                var listRole = await _roleQueryService.GetList();
                var result = new RoleListDto()
                {
                    Data = listRole,
                    TotalCount = listRole.Count
                };

                return result.SuccessResult();
            }
            catch (Exception ex)
            {
                throw new ResultException(ex.Message);
            }
        }
    }
}
