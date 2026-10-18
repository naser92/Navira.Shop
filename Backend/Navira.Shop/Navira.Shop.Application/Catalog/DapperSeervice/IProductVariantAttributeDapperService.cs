using Navira.Shop.Core.Dapper;

namespace Navira.Shop.Application.Catalog
{
    public interface IProductVariantAttributeDapperService : IDapperService
    {
        Task<List<ProductVariantAttributeValueResponseDto>> GetAllAttribut(int productId);
    }
}
