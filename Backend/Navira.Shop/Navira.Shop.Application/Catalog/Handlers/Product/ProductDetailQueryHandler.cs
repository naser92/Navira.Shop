using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog;

public class ProductDetailQueryHandler : CommandHandler, IQueryHandler<ProductDetailCommand, ProductDto>
{
    private readonly IProductQueryService _service;
    public ProductDetailQueryHandler(IProductQueryService service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public Task<IResult<ProductDto>> Handle(ProductDetailCommand command, CancellationToken cancellationToken = default) => _service.Get<ProductDto>(command.Id);
}