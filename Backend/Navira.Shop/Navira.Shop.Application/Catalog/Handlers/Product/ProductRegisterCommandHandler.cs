using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductRegisterCommandHandler : CommandHandler, ICommandHandler<ProductRegisterCommand>
    {
        private readonly IProductWriteRepository _repository;

        public ProductRegisterCommandHandler(IProductWriteRepository repository, IUnitOfWork uow) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(ProductRegisterCommand command, CancellationToken cancellationToken = default)
        {
            var slug = command.Slug.IsNullOrWhiteSpace() ? Slug.Generate(command.Name) : Slug.Create(command.Slug);
            var entiy = Product.Create(command.Name, slug, command.ShortDescription, command.Description, command.BrandId.Value, command.CategoryId, command.TaxCategoryId);
            await _repository.Insert(entiy);
            await Uow.Commit();
            return entiy.Id.SuccessResult("اطلاعات با موفقیت ثبت شد");
        }
    }
}
