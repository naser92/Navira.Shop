using Navira.Shop.Core.Service;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public interface IProductAttributeValuePolicy : IBaseService
    {
        AttributeValueValidationResult Validate(ProductAttribute definition, ProductAttributeValueRegisterOrUpdateDto input, ProductAttributeValue? current, IReadOnlyDictionary<int, ProductAttributeOption> optionsById);
    }
}
