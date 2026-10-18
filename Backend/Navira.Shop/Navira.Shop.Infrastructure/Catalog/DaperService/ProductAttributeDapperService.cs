using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Dapper;
using Navira.Shop.Domain.Catalog;
using NaviraShop.Core.Mq;
using System.Data;

namespace Navira.Shop.Infrastructure.Catalog
{
    public class ProductAttributeDapperService : DapperBaseService, IProductAttributeDapperService
    {
        public ProductAttributeDapperService([FromKeyedServices("DapperConnection")] IDbConnection dbConnection, IPublisher publisher) : base(dbConnection, publisher)
        {
        }

        public async Task<ProductAttributesResponseDto> GetAttribut(int productId)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId));

            var data = await QueryMultiple(
                "sp_GetProductAttributes",
                new { ProductId = productId },
                reader => new
                {
                    Product = reader
                        .Read<ProductAttributesResponseDto>()
                        .FirstOrDefault(),

                    Attributes = reader
                        .Read<ProductAttributeValueResponseDto>()
                        .ToList(),

                    SelectedOptions = reader
                        .Read<ProductAttributeSelectedOptionDto>()
                        .ToList()
                });

            if (data?.Product is null)
                throw new KeyNotFoundException("محصول یافت نشد.");

            var result = data.Product;

            var optionsLookup = data.SelectedOptions
                .ToLookup(x => x.ProductAttributeValueId);

            foreach (var attribute in data.Attributes)
            {
                attribute.SelectedOptions = optionsLookup[attribute.Id]
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .Select(x => new ProductAttributeSelectedOptionDto
                    {
                        Id = x.Id,
                        Value = x.Value,
                        Code = x.Code,
                        SortOrder = x.SortOrder,
                        IsActive = x.IsActive
                    })
                    .ToList();

                attribute.ProductAttributeOptionIds =
                    attribute.ValueType == AttributeValueType.MultiOption
                        ? attribute.SelectedOptions.Select(x => x.Id).ToList()
                        : new List<int>();
            }

            result.Attributes = data.Attributes;

            return result;
        }

        public async Task<object> GetList(ProductAttributeGetFilterDto filter) =>
            await QueryMultiple(
                  @$"SP_ProductAttribute_list", new DynamicParameters(filter)
                  , (data) =>
                  {
                      return new
                      {
                          data = data.Read<ProductAttributeDto>().ToList(),
                          totalCount = data.ReadFirst<int>()
                      };
                  });
    }
}
