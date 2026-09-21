using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryDeleteCommand : BaseDto<int>, ICommand
    {

        public CategoryDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
