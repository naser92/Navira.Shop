using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Handlers
{
    public class DeleteCategoryCommandHandler : CommandHandler, ICommandHandler<DeleteCategoryCommand>
    {

        public DeleteCategoryCommandHandler(IUnitOfWork uow) : base(uow)
        {

        }

        public Task<IResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public async Task<IResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var category = await _repository.Get(command.Id);

        //        if (category is null)
        //            return Result.Fail("این دسته وجود ندارد");

        //        await _repository.Delete(category);

        //        return Result.Success("دسته حذف شد");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Result.Fail(ex.Message);
        //    }
        //}


    }
}
