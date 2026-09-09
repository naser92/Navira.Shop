using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog
{
    public class BrandListCommandHandler : CommandHandler, IQueryHandler<BrandListCommand, object>
    {
        private readonly IBrandDapperService _dapperService;
        public BrandListCommandHandler(IUnitOfWork uow, IBrandDapperService dapperService) : base(uow)
        {
            _dapperService = dapperService;
        }

        public async Task<IResult<object>> Handle(BrandListCommand query, CancellationToken cancellationToken = default)
        {
            return await _dapperService.GetList(query).ResultAsync();
        }
    }
}
