using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductVariantRegisterCommandHandler : CommandHandler, ICommandHandler<ProductVariantRegisterCommand>
    {
        private readonly IProductVariantWriteRepository _repository;

        public ProductVariantRegisterCommandHandler(
            IProductVariantWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(ProductVariantRegisterCommand command, CancellationToken cancellationToken = default)
        {
            try
            {

                var entiy = ProductVariant.Create(command.ProductId, command.Sku, command.Price, command.CostPrice, command.IsActive);

                if (command.Options.Any())
                {
                    foreach (var item in command.Options)
                    {
                        entiy.AddOption(item.ProductAttributeId, item.ProductAttributeOptionId);
                    }
                }

                await _repository.Insert(entiy);
                return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                throw new ResultException(ex.Message);
            }
        }
    }
}
