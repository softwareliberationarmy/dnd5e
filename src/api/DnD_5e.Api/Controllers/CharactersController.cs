using System;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Api.Services;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DnD_5e.Api.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly CharacterRepository _repository;
        private readonly DieRoller _roller;
        private readonly CharacterRollParser _rollParser;

        public CharactersController(CharacterRepository repository, DieRoller roller, CharacterRollParser rollParser)
        {
            _repository = repository;
            _roller = roller;
            _rollParser = rollParser;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyCharacters()
        {
            var characters = _repository.GetByOwner(User.Identity.Name);
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
