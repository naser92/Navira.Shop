using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantDeleteCommandHandler : CommandHandler, ICommandHandler<ProductVariantDeleteCommand>
{
    private readonly IProductVariantWriteRepository _service;
    public ProductVariantDeleteCommandHandler(IProductVariantWriteRepository service, IUnitOfWork uow) : base(uow)
    {
        _service = service;
    }
    public async Task<IResult> Handle(ProductVariantDeleteCommand command, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(command.Id);
        if (entity is null) return await Result.FailAsync("Record not found.");
        await _service.Delete(entity);
        return await Result.SuccessAsync("?????? ?? ?????? ????? ??");
    }
}
