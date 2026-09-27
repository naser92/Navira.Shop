using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductImageRegisterCommandHandler : CommandHandler, ICommandHandler<ProductImageRegisterCommand>
    {
        private readonly IProductQueryService _productQueryService;
        private readonly IProductImageWriteRepository _repository;

        public ProductImageRegisterCommandHandler(
            IProductImageWriteRepository repository, IUnitOfWork uow, IProductQueryService productQueryService) : base(uow)
        {
            _repository = repository;
            _productQueryService = productQueryService;
        }

        public async Task<IResult> Handle(ProductImageRegisterCommand command, CancellationToken cancellationToken = default)
        {
            if (!await _productQueryService.IsExist(command.ProductId, command.ProductVariantId))
                return await Result.FailAsync("محصول  وجود ندارد");

            var entiy = command.Map<ProductImage>();
            await _repository.Insert(entiy);

            return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
        }
    }
}
