using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Display(Name = "identity", Description = "تنظیمات")]
    public class ConfigController : ControllerBase
    {
        //private readonly IPublishMenuAndPermissionService _permissionService;

        //[HttpGet("PublishPermissions/{name}")]
        //public IActionResult PublishPermissions(string name)
        //{
        //    if (name.IsNullOrWhiteSpace())
        //    {
        //        return Fail("نام کنترولر را وارد کنید");
        //    }
        //    _permissionService.BuildPermissionAndMenu(name);
        //    return Success("انجام شد.");
        //}
    }
}
