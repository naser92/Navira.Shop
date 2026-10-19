using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductDeleteCommand : BaseDto<int>, ICommand
{
    public ProductDeleteCommand(int id) => Id = id;
}
