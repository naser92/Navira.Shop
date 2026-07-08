using Navira.Shop.Application.Catalog.DTOs;
using Navira.Shop.Core.Bus;

namespace Navira.Shop.Application.Catalog.Commands
{
    public class UpdateCategoryCommand : UpdateCategoryDto, ICommand
    {
        public int Id { get; set; }
    }

}
