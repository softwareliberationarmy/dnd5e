using System;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers.Roll;
using DnD_5e.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/roll")]
    [ApiController]
    public class RollController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RollController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Makes a random 1d20 roll
        /// </summary>
        /// <returns>a random roll result</returns>
        // GET: api/<RollController>
        [HttpGet]
        public async Task<ActionResult<RollResponse>> Roll1d20()
        {
            //if no roll request is provided, assume the user wants a 1d20 roll
            return await _mediator.Send(new RollRequest("1d20"));
        }

        /// <summary>
        /// Makes a random roll using the roll request passed in
        /// </summary>
        /// <remarks>
        /// Canonically, D&amp;D modifiers use the plus and minus symbol. Due to URL issues,
        /// if you want to pass in a modifier to your roll, you must use p and m for 'plus' and 'minus'.
        /// </remarks>
        /// <param name="rollRequest">the requested roll</param>
        /// <returns>a random roll result</returns>
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

        /// <summary>
        /// Makes a random roll with advantage or disadvantage using the roll request passed in
        /// </summary>
        /// <param name="rollRequest">the requested roll</param>
        /// <param name="rollWith">'advantage' or 'disadvantage'</param>
        /// <returns>two random roll results, and an indicator of which one to use</returns>
        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}/{rollWith}")]
        public async Task<ActionResult<RollResponse>> RollWith(string rollRequest, string rollWith = "")
        {
            try
            {
                return await _mediator.Send(new RollWithRequest(rollRequest, rollWith));
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is FormatException)
            {
                return BadRequest();
            }
        }
    }
}
