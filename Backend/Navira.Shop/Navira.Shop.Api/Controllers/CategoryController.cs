using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Catalog.Commands;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "دسته ها", Description = "مدیریت دسته ها")]
    public class CategoryController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        public CategoryController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            return await _bus.Send(command).ApiResultAsync();

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
        {
            command.Id = id;
            return await _bus.Send(command).ApiResultAsync();

        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _bus.Send(new DeleteCategoryCommand { Id = id }).ApiResultAsync();
        }




    }
}
