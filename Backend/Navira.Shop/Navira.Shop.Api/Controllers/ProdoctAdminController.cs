using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
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


        [HttpPost]
        [Permission("Register", "ایجاد")]
        [Menu("ProdoctAdmin.Register", "ایجاد محصول", Action = "products/new")]
        public virtual async Task<IActionResult> Register() =>
          Ok();

    }
}
