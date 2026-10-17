using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductAttributeOptionDetailCommand : BaseDto<int>, ICommand
{
    public ProductAttributeOptionDetailCommand(int id) => Id = id;
}

