using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeDetailQueryHandler : CommandHandler, IQueryHandler<ProductAttributeDetailCommand, ProductAttributeDto>
    {
        private readonly IProductAttributeQueryService _Service;
        public ProductAttributeDetailQueryHandler(IUnitOfWork uow, IProductAttributeQueryService service) : base(uow)
        {
            _Service = service;
        }

        public async Task<IResult<ProductAttributeDto>> Handle(ProductAttributeDetailCommand query, CancellationToken cancellationToken = default)
        {
            return await _Service.Get<ProductAttributeDto>(query.Id);
        }
    }
}
