using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductDetailCommand : BaseDto<int>, ICommand
{
    public ProductDetailCommand(int id) => Id = id;
}

