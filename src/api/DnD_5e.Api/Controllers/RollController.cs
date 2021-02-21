using System;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/roll")]
    [ApiController]
    public class RollController : ControllerBase
    {
        private readonly DieRoller _roller;
        private readonly IMediator _mediator;

        public RollController(DieRoller roller, IMediator mediator)
        {
            _roller = roller;
            _mediator = mediator;
        }

        // GET: api/<RollController>
        [HttpGet]
        public async Task<ActionResult<RollResponse>> Roll1d20(string rollRequest)
        {
            //if no roll request is provided, assume the user wants a 1d20 roll
            return await _mediator.Send(new RollRequest(rollRequest ?? "1d20"));
        }

        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}")]
        public async Task<ActionResult<RollResponse>> RollDice(string rollRequest)
        {
            try
            {
                return await _mediator.Send(new RollRequest(rollRequest));
            }
            catch (FormatException)
            {
                return BadRequest();
            }
        }

        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}/{rollWith}")]
        public async Task<ActionResult<RollResponse>> RollWith(string rollRequest, string rollWith = "")
        {
            try
            {
                switch (rollWith.ToLower())
                {
                    case "advantage":
                        return await _roller.Roll(rollRequest, With.Advantage);
                    case "disadvantage":
                        return await _roller.Roll(rollRequest, With.Disadvantage);
                    default:
                        return BadRequest();
                }
            }
            catch (FormatException)
            {
                return BadRequest();
            }
        }
    }
}
