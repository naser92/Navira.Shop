using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Identity
{
    public class PolicyListQueryHandler : CommandHandler, IQueryHandler<PolicyListCommand, object>
    {
        private readonly IPolicyDapperService _service;
        public PolicyListQueryHandler(IUnitOfWork uow, IPolicyDapperService service) : base(uow)
        {
            _service = service;
        }

        public async Task<IResult<object>> Handle(PolicyListCommand query, CancellationToken cancellationToken = default)
        {
            return await _service.GetList(query).ResultAsync();
        }
    }
}
