using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeOptionDeleteCommand : BaseDto<int>, ICommand
    {

        public ProductAttributeOptionDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
