using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.Services;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _repository;
        private readonly DieRoller _roller;
        private readonly CharacterRollParser _rollParser;
        private readonly IMediator _mediator;

        public CharactersController(ICharacterRepository repository, DieRoller roller, CharacterRollParser rollParser, IMediator mediator)
        {
            _repository = repository;
            _roller = roller;
            _rollParser = rollParser;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyCharacters()
        {
            var characters = await _mediator.Send(new GetCharactersByOwnerRequest(User?.Identity?.Name), CancellationToken.None);
            return Ok(characters.ToArray());
        }

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{rollType}")]
        public async Task<ActionResult<RollResponse>> MakeCharacterRoll(int id, string rollType)
        {
            try
            {
                var request = _rollParser.ParseRequest(rollType);
                var character = _repository.GetById(id);

                if (character == null)
                {
                    return NotFound();
                }
                var roll = character.GetRoll(request);
                return await _roller.Roll(roll);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Ability {rollType} not found");
            }
        }

        // GET api/<CharactersController>/5/roll/strength/save
        [HttpGet("{id}/roll/{ability}/save")]
        public async Task<ActionResult<RollResponse>> MakeSavingThrow(int id, string ability)
        {
            try
            {
                var request = _rollParser.ParseRequest(ability, isSave: true);
                var character = _repository.GetById(id);

                if (character == null)
                {
                    return NotFound();
                }
                var roll = character.GetRoll(request);

                return await _roller.Roll(roll);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Ability {ability} not found");
            }
        }
    }
}
