using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Catalog;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "محصولات", Description = "مدیریت مدیریت محصولات")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    [Permission("Controller", "محصولات", "catalog.product")]
    [Menu("catalog.product", "مدیریت محصول")]
    public class ProdoctAdminController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;
        public ProdoctAdminController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        //[HttpGet]
        ////[Permission("List", "دسترسی ها")]
        //[Menu("Permission.List", "دسترسی ها", Action = "product/list")]
        //public virtual async Task<IActionResult> List() =>
        //   Ok();

        [HttpGet("{id}")]
        [Permission("Detail", "جزئیات")]
        public async Task<IActionResult> Get(int id) =>
          await _queryBus.Send<ProductDetailCommand, object>(new ProductDetailCommand(id)).ApiResultAsync();

        [HttpPost]
        [Permission("Register", "ایجاد")]
        [Menu("ProdoctAdmin.Register", "ایجاد محصول", Action = "products/new")]
        public virtual async Task<IActionResult> Register(ProductRegisterCommand command) =>
          await _bus.Send(command).ApiResultAsync();

        [HttpPut]
        [Permission("Update", "ویرایش")]
        public async Task<IActionResult> Put([FromBody] ProductUpdateCommand command) =>
           await _bus.Send(command).ApiResultAsync();

        [HttpGet]
        [Route("{productId}/attributes")]
        [Permission("ListAttributes", "لیست ویژگی")]
        public virtual async Task<IActionResult> Attributes([FromRoute] int productId) =>
            await _queryBus.Send<ProductAllAttributeCommand, object>(new ProductAllAttributeCommand { ProductId = productId }).ApiResultAsync();

        [HttpPut]
        [Route("{productId}/attributes")]
        [Permission("EditAttributes", "بروزرسانی ویژگی ها")]
        public virtual async Task<IActionResult> EditAttributes([FromRoute] int productId, [FromBody] ProductAllAttributeEditCommand command)
        {
            command.ProductId = productId;
            return await _bus.Send(command).ApiResultAsync();
        }
    }
}
