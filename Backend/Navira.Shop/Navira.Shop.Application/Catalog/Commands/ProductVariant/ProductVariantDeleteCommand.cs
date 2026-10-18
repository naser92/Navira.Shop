using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog;

public class ProductVariantDeleteCommand : BaseDto<int>, ICommand
{
    public ProductVariantDeleteCommand(int id) => Id = id;
}