using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DnD_5e.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetAuthConfigInfo()
        {
            return Ok(new
            {
                Domain = _config.GetValue<string>("Auth:Domain"),
                ClientId = _config.GetValue<string>("Auth:ClientId"),
                Audience = _config.GetValue<string>("Auth:Audience")
            });
        }
    }
}
