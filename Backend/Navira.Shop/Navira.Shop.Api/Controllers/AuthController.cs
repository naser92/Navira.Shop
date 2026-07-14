using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Application.Auth;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Web;
using System.ComponentModel.DataAnnotations;

namespace Navira.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Display(Name = "identity", Description = "احراز هویت")]

    public class AuthController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IQueryBus _queryBus;

        public AuthController(IBus bus, IQueryBus queryBus)
        {
            _bus = bus;
            _queryBus = queryBus;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            return await _queryBus.Send<LoginCommand, AuthTokenDto>(command, cancellationToken).ApiResultAsync();
        }
    }
}
