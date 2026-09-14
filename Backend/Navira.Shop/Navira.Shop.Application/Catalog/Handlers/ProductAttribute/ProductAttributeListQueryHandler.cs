using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeListQueryHandler : CommandHandler, IQueryHandler<ProductAttributeListCommand, object>
    {
        private readonly IProductAttributeDapperService _productAttributeDapperService;
        public ProductAttributeListQueryHandler(IUnitOfWork uow, IProductAttributeDapperService productAttributeDapperService) : base(uow)
        {
            _productAttributeDapperService = productAttributeDapperService;
        }

        public async Task<IResult<object>> Handle(ProductAttributeListCommand query, CancellationToken cancellationToken = default)
        {
            return await _productAttributeDapperService.GetList(query).ResultAsync();
        }
    }
}
