using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantDetailCommand : BaseDto<int>, ICommand
{
    public ProductVariantDetailCommand(int id) => Id = id;
}
