using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog;

public class ProductUpdateCommandHandler : CommandHandler, ICommandHandler<ProductUpdateCommand>
{
    private readonly IProductWriteRepository _service;
    public ProductUpdateCommandHandler(IProductWriteRepository service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public async Task<IResult> Handle(ProductUpdateCommand command, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(command.Id);
        if (entity is null) return await Result.FailAsync("Record not found.");
        command.Map(entity);
        return await Result.SuccessAsync("اطلاعات ویرایش شد");
    }
}
