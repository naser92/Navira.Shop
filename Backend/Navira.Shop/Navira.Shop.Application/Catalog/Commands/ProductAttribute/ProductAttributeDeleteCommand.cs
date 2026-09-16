using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeDeleteCommand : BaseDto<int>, ICommand
    {

        public ProductAttributeDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
