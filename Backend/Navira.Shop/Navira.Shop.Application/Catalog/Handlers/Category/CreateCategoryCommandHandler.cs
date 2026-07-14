using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Application.Catalog.Repository;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Application.Handlers
{
    public class CreateCategoryCommandHandler : CommandHandler, ICommandHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _repository;
        public CreateCategoryCommandHandler(IUnitOfWork uow, ICategoryRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(CreateCategoryCommand command)
        {
            try
            {
                var category = Category.Create(
                                    command.Name,
                                    command.Slug,
                                    command.TaxCategoryId,
                                    command.ParentCategoryId);

                category.UpdateDescription(command.Description);
                category.SetDisplayOrder(command.DisplayOrder);

                await _repository.Insert(category);
                return Result.Success("Category created successfully.");
            }
            catch (DomainException ex)
            {
                return Result.Fail(ex.Message);
            }

        }
    }
}
