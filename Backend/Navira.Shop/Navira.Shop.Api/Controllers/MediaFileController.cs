using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Media;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Security;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Display(Name = "مدیریت فایل", Description = "مدیریت فایل ها")]
    [CustomAuthorize(AuthenticationSchemes = "Bearer")]
    //[Permission("{ControllerName}", AppConsts.SystemBaseInformationName, "{ControllerTitle}")]
    //[Menu(AppConsts.SystemBaseInformationName, AppConsts.SystemBaseInformationTitle)]
    public class MediaFileController : ControllerBase
    {
        #region variables 

        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor 

        public MediaFileController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }

        #endregion


        #region Register

        /// <summary>
        /// ثبت اطلاعات  
        /// </summary>
        /// <param name="command">
        /// مشخصات  
        /// </param>
        [HttpPost]
        [Permission("Create", "ایجاد")]
        [Consumes("multipart/form-data")]
        public virtual async Task<IActionResult> Post([FromForm] UploadMediaRequest request) =>
             await _bus.Send(new MediaFileRegisterCommand(request)).ApiResultAsync();

        #endregion


    }
}
