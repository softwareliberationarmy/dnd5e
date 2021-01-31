using System;
using DnD_5e.Api.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DnD_5e.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Auth0Settings _settings;

        public AuthController(IOptions<Auth0Settings> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public IActionResult GetAuthConfigInfo()
        {
            try
            {
                return Ok(new
                {
                    Domain = _settings.Domain,
                    ClientId = _settings.ClientId,
                    Audience = _settings.Audience
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
