using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantAllAttributeQueryHandler : CommandHandler, IQueryHandler<ProductVariantAllAttributeCommand, object>
{
    private readonly IProductVariantAttributeDapperService _productVariantAttributeDapperService;
    public ProductVariantAllAttributeQueryHandler(IUnitOfWork uow, IProductVariantAttributeDapperService productVariantAttributeDapperService) : base(uow)
    {
        _productVariantAttributeDapperService = productVariantAttributeDapperService;
    }

    public async Task<IResult<object>> Handle(ProductVariantAllAttributeCommand query, CancellationToken cancellationToken = default) =>
             await _productVariantAttributeDapperService.GetAllAttribut(query.ProductId).ResultAsync();
}
