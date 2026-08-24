using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Application.Handlers
{
    public class CreateCategoryCommandHandler : CommandHandler, ICommandHandler<CreateCategoryCommand>
    {

        public CreateCategoryCommandHandler(IUnitOfWork uow) : base(uow)
        {

        }

        public Task<IResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public async Task<IResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken = default)
        //{
        //    //try
        //    //{
        //    //    var category = Categories.Create(
        //    //                        command.Name,
        //    //                        command.Slug,
        //    //                        command.TaxCategoryId,
        //    //                        command.ParentCategoryId);

        //    //    category.UpdateDescription(command.Description);
        //    //    category.SetDisplayOrder(command.DisplayOrder);

        //    //    await _repository.Insert(category);
        //    //    return Result.Success("Category created successfully.");
        //    //}
        //    //catch (DomainException ex)
        //    //{
        //    //    return Result.Fail(ex.Message);
        //    //}

        //}
    }
}
