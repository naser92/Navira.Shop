using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog;

public class ProductAttributeOptionDetailQueryHandler : CommandHandler, IQueryHandler<ProductAttributeOptionDetailCommand, ProductAttributeOptionDto>
{
    private readonly IProductAttributeOptionQueryService _service;
    public ProductAttributeOptionDetailQueryHandler(IProductAttributeOptionQueryService service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public Task<IResult<ProductAttributeOptionDto>> Handle(ProductAttributeOptionDetailCommand command, CancellationToken cancellationToken = default) => _service.Get<ProductAttributeOptionDto>(command.Id);
}
