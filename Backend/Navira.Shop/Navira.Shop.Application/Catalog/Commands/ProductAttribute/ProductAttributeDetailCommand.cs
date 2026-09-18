using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeDetailCommand : BaseDto<int>, ICommand
    {
        public ProductAttributeDetailCommand(int id)
        {
            Id = id;
        }
    }
}
