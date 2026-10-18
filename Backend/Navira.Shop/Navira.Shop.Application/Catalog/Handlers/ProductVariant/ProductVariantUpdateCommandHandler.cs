using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantUpdateCommandHandler : CommandHandler, ICommandHandler<ProductVariantUpdateCommand>
{
    private readonly IProductVariantWriteRepository _service;
    public ProductVariantUpdateCommandHandler(IProductVariantWriteRepository service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public async Task<IResult> Handle(ProductVariantUpdateCommand command, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(command.Id);
        if (entity is null) return await Result.FailAsync("اطلاعات پیدا نشد");
        command.Map(entity);
        return await Result.SuccessAsync("ویرایش اطلاعات انجام شد");
    }
}
