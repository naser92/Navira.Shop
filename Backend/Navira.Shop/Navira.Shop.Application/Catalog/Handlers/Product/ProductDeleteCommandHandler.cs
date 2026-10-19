using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog;

public class ProductDeleteCommandHandler : CommandHandler, ICommandHandler<ProductDeleteCommand>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductVariantWriteRepository _productVariantWriteRepository;
    private readonly IProductAttributeValueWriteRepository _productAttributeValueWriteRepository;
    private readonly IProductVariantAttributeValueWriteRepository _productVariantAttributeValueWriteRepository;
    public ProductDeleteCommandHandler(IProductWriteRepository productWriteRepository, IUnitOfWork uow, IProductVariantWriteRepository productVariantWriteRepository, IProductAttributeValueWriteRepository productAttributeValueWriteRepository, IProductVariantAttributeValueWriteRepository productVariantAttributeValueWriteRepository) : base(uow)
    {
        _productWriteRepository = productWriteRepository;
        _productVariantWriteRepository = productVariantWriteRepository;
        _productAttributeValueWriteRepository = productAttributeValueWriteRepository;
        _productVariantAttributeValueWriteRepository = productVariantAttributeValueWriteRepository;
    }
    public async Task<IResult> Handle(ProductDeleteCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productWriteRepository.Get(command.Id);

        if (product is null) return await Result.FailAsync("محصول پیدا نشد");

        var variant = await _productVariantWriteRepository.GetAll(x => x.Where(i => i.ProductId == command.Id));

        var attProduct = await _productAttributeValueWriteRepository.GetAll(x => x.Where(i => i.ProductId == command.Id));
        var attVariant = await _productVariantAttributeValueWriteRepository.GetAll(x => x.Where(i => (variant.Select(w => w.Id).ToList()).Contains(i.ProductVariantId)));

        await _productVariantAttributeValueWriteRepository.Delete(attVariant);
        await _productAttributeValueWriteRepository.Delete(attProduct);
        await _productVariantWriteRepository.Delete(variant);
        await _productWriteRepository.Delete(product);
        return await Result.SuccessAsync("محصول حذف شد");
    }
}