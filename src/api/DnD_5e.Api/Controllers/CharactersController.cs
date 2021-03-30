using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.Common;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.RequestHandlers.Characters;
using DnD_5e.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CharactersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets an authenticated user's characters
        /// </summary>
        /// <returns>An array of characters belonging to the caller.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyCharacters()
        {
            var characters = await _mediator.Send(new GetCharactersByOwnerRequest(User?.Identity?.Name), CancellationToken.None);
            return Ok(characters.ToArray());
        }

        /// <summary>
        /// Makes an ability, skill, or initiative roll on behalf of a character
        /// </summary>
        /// <param name="id">the character ID</param>
        /// <param name="rollType">the name of the ability or skill being requested,
        /// or initiative</param>
        /// <returns>a roll result</returns>
        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{rollType}")]
        public async Task<ActionResult<RollResponse>> MakeCharacterRoll(int id, string rollType)
        {
            try
            {
                return await _mediator.Send(new CharacterRoll.Request(id, rollType), CancellationToken.None);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Makes an ability saving throw on behalf of a character 
        /// </summary>
        /// <param name="id">the character ID</param>
        /// <param name="ability">the name of the ability being saved</param>
        /// <returns>a roll result</returns>
        // GET api/<CharactersController>/5/roll/strength/save
        [HttpGet("{id}/roll/{ability}/save")]
        public async Task<ActionResult<RollResponse>> MakeSavingThrow(int id, string ability)
        {
            try
            {
                return await _mediator.Send(new CharacterSavingThrow.Request(id, ability), CancellationToken.None);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
