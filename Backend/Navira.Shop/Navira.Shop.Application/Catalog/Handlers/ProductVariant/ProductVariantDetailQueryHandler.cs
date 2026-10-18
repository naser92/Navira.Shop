using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantDetailQueryHandler : CommandHandler, IQueryHandler<ProductVariantDetailCommand, ProductVariantDto>
{
    private readonly IProductVariantQueryService _service;
    public ProductVariantDetailQueryHandler(IProductVariantQueryService service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public Task<IResult<ProductVariantDto>> Handle(ProductVariantDetailCommand command, CancellationToken cancellationToken = default) =>
        _service.Get<ProductVariantDto>(command.Id);
}
