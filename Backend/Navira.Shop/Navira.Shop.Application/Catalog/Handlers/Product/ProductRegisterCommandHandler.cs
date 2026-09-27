using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductRegisterCommandHandler : CommandHandler, ICommandHandler<ProductRegisterCommand, int>
    {
        private readonly IProductWriteRepository _repository;

        public ProductRegisterCommandHandler(
            IProductWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(ProductRegisterCommand command, CancellationToken cancellationToken = default)
        {
            var entiy = Product.Create(command.Name, command.Slug, command.ShortDescription, command.Description, command.BrandId.Value, command.CategoryId, command.TaxCategoryId);
            await _repository.Insert(entiy);
            return entiy.Id.SuccessResult("اطلاعات با موفقیت ثبت شد");
        }
    }
}
