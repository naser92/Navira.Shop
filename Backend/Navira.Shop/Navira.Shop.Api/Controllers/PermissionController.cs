using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "دسترسی", Description = "مدیریت دسترسی ها")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    [Permission("Controller", "دسترسی", "Identity.Access")]
    [Menu("Identity.Access", "مدیریت دسترسی")]
    public class PermissionController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;
        public PermissionController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        [HttpPost]
        [Permission("List", "دسترسی ها")]
        [Menu("Permission.List", "دسترسی ها", Action = "access")]
        public virtual async Task<IActionResult> List()
        {
            return Ok();

        }
    }
}
