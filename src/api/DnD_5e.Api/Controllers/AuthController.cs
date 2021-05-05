using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the Auth0 settings needed to authenticate a user
        /// </summary>
        /// <response code="200">Settings were successfully recovered</response>
        /// <response code="500">An error is encountered while obtaining the Auth0 settings</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetAuthValuesRequest.Response>> GetAuthConfigInfo()
        {
            return await _mediator.Send(new GetAuthValuesRequest(), CancellationToken.None);
        }
    }
}
