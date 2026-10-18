using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Dapper;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductVariantAttributeDapperService : DapperBaseService, IProductVariantAttributeDapperService
    {
        public ProductVariantAttributeDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<List<ProductVariantAttributeValueResponseDto>> GetAllAttribut(int productId)
        {
            var data = await QueryMultiple(
                   @$"sp_GetProductVariantAttributes",
                   new { ProductId = productId },
                    reader => new
                    {
                        Variant = reader.Read<ProductVariantAttributeValueResponseDto>().ToList(),
                        Attribut = reader.Read<ProductVariantAllAttributeValueDto>().ToList(),
                    });

            var result = data.Variant;

            if (!data.Variant.Any() || !data.Attribut.Any())
                return result;

            var attributLookup = data.Attribut.ToLookup(x => x.ProductVariantId);

            foreach (var item in result)
            {
                item.Attributes = attributLookup[item.Id]
                    .Select(x => x).ToList();
            }

            return result;

        }
    }
}
