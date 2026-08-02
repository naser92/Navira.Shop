using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Domain;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.Security;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Handlers
{
    public class CreateCategoryCommandHandler : CommandHandler, ICommandHandler<CreateCategoryCommand>
    {
        private readonly ICategoriesRepository _repository;
        private readonly IAppEngin _appEngin;
        public CreateCategoryCommandHandler(IUnitOfWork uow, ICategoriesRepository repository, IAppEngin appEngin) : base(uow)
        {
            _repository = repository;
            _appEngin = appEngin;
        }

        public async Task<IResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = Categories.Create(
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
