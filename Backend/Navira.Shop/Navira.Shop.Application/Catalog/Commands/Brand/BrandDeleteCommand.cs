using Navira.Shop.Core.Bus;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class BrandDeleteCommand : BaseDto<int>, ICommand
    {

        public BrandDeleteCommand(int id)
        {
            Id = id;
        }

    }
}
