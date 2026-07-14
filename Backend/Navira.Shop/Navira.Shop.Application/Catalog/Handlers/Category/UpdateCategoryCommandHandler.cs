using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Application.Catalog.Repository;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Handlers
{
    public class UpdateCategoryCommandHandler : CommandHandler, ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _repository;
        public UpdateCategoryCommandHandler(IUnitOfWork uow, ICategoryRepository repository) : base(uow)
        {

            _repository = repository;
        }

        public async Task<IResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var category = await _repository.Get(command.Id);

            if (category is null)
                return Result.Fail("دسته بندی وجود ندارد");

            try
            {
                category.Rename(command.Name, command.Slug);
                category.UpdateDescription(command.Description);
                category.ChangeTaxCategory(command.TaxCategoryId);
                category.MoveUnder(command.ParentCategoryId);
                category.SetDisplayOrder(command.DisplayOrder);

                if (command.IsActive) category.Activate(); else category.Deactivate();
                return Result.Success("دسته بندی بروز شد");

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }
    }
}
