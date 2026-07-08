using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Catalog.Commands
{
    public class DeleteCategoryCommand : ICommand
    {
        public int Id { get; set; }
    }

}
