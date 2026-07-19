using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Application.Catalog.Repository;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Handlers
{
    public class DeleteCategoryCommandHandler : CommandHandler, ICommandHandler<DeleteCategoryCommand>
    {
        private readonly ICategoriesRepository _repository;
        public DeleteCategoryCommandHandler(IUnitOfWork uow, ICategoriesRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = await _repository.Get(command.Id);

                if (category is null)
                    return Result.Fail("این دسته وجود ندارد");

                await _repository.Delete(category);

                return Result.Success("دسته حذف شد");

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }


    }
}
