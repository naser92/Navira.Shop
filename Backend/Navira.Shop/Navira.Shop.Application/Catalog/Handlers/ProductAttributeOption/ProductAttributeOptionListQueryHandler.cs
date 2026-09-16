using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeOptionListQueryHandler : CommandHandler, IQueryHandler<ProductAttributeOptionListCommand, object>
    {
        private readonly IProductAttributeOptionDapperService _productAttributeOptionDapperService;
        public ProductAttributeOptionListQueryHandler(IUnitOfWork uow, IProductAttributeOptionDapperService productAttributeOptionDapperService) : base(uow)
        {
            _productAttributeOptionDapperService = productAttributeOptionDapperService;
        }

        public async Task<IResult<object>> Handle(ProductAttributeOptionListCommand query, CancellationToken cancellationToken = default)
        {
            return await _productAttributeOptionDapperService.GetList(query).ResultAsync();
        }
    }
}
