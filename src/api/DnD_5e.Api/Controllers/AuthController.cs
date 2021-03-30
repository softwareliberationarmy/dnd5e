﻿using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
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
        [HttpGet]
        public async Task<ActionResult<GetAuthValuesRequest.Response>> GetAuthConfigInfo()
        {
            return await _mediator.Send(new GetAuthValuesRequest(), CancellationToken.None);
        }
    }
}
