using System;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/roll")]
    [ApiController]
    public class RollController : ControllerBase
    {
        private readonly DieRoller _roller;

        public RollController(DieRoller roller)
        {
            _roller = roller;
        }

        // GET: api/<RollController>
        [HttpGet]
        public async Task<ActionResult<RollResponse>> Roll1d20(string rollRequest)
        {
            //if no roll request is provided, assume the user wants a 1d20 roll
            return await RollDice(rollRequest ?? "1d20");
        }

        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}")]
        public async Task<ActionResult<RollResponse>> RollDice(string rollRequest)
        {
            try
            {
                return await _roller.Roll(rollRequest);
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
