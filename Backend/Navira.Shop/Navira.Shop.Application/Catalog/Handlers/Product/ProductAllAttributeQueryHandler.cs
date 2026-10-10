using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalogt
{
    public class ProductAllAttributeQueryHandler : CommandHandler, IQueryHandler<ProductAllAttributeCommand, object>
    {
        private readonly IProductAttributeDapperService _productAttributeDapperService;
        public ProductAllAttributeQueryHandler(IUnitOfWork uow, IProductAttributeDapperService productAttributeDapperService) : base(uow)
        {
            _productAttributeDapperService = productAttributeDapperService;
        }

        public async Task<IResult<object>> Handle(ProductAllAttributeCommand query, CancellationToken cancellationToken = default)
        {
            return await _productAttributeDapperService.GetAttribut(query.ProductId).ResultAsync();
        }
    }
}
