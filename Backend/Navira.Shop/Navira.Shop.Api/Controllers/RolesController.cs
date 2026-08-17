using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "نقش", Description = "مدیریت نقش ها")]
    //[CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("Controller", "دسترسی", "Identity.Access")]
    //[Menu("Identity.Access", "مدیریت دسترسی")]
    public class RolesController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;
        public RolesController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        [HttpGet]
        //[Permission("List", "دسترسی ها")]
        //[Menu("Permission.List", "دسترسی ها", Action = "access")]
        public virtual async Task<IActionResult> List() =>
            await _queryBus.Send<GetListRolesCommand, IReadOnlyList<KeycloakRoleDto>>(new GetListRolesCommand()).ApiResultAsync();

    }
}
