using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Identity;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Services;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "identity", Description = "تنظیمات")]
    public class ConfigController : ControllerBase
    {
        private readonly IPublishMenuAndPermissionService _permissionService;
        private readonly IBus _bus;
        public ConfigController(IPublishMenuAndPermissionService permissionService, IBus bus)
        {
            _permissionService = permissionService;
            _bus = bus;
        }

        [HttpGet("PublishPermissions/{name}")]
        public async Task<IActionResult> PublishPermissions(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                return Ok("نام کنترولر را وارد کنید");
            }
            var data = await _permissionService.BuildPermissionAndMenu(name);
            await _bus.Send(new RegisterPermissionAndMenuCommand { data = data });
            return Ok(JsonConvert.SerializeObject(data));
        }
    }
}
