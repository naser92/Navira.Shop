using System;
using Navira.Shop.Core.Core.Caching;
using Navira.Shop.Core.Data;
using Navira.Shop.Query.DataContext;
using Navira.Shop.Query.DataContext.DataModels;
using Navira.Shop.QueryService.Contracts.Services;
using Navira.Shop.QueryService.Contracts.Repositories;

namespace Navira.Shop.QueryService.Repositories
{
    public class ProductVariantAttributeValueQueryRepository: QueryRepository<ProductVariantAttributeValueModel, long>, IProductVariantAttributeValueQueryRepository
    {
        
        public ProductVariantAttributeValueQueryRepository(IStaticCacheManager staticCachManager, QueryDbContext context) : base(staticCachManager, context)
        {
        }

    }

}

