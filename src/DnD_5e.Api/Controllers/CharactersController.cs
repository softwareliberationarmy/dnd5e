using System;
using System.Threading.Tasks;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Domain.Roleplay;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DnD_5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly CharacterRepository _repository;
        private readonly DieRoller _roller;

        public CharactersController(CharacterRepository repository, DieRoller roller)
        {
            _repository = repository;
            _roller = roller;
        }

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{rollType}")]
        public async Task<ActionResult<int>> MakeAbilityCheck(int id, string rollType)
        {
            var character = _repository.GetById(id);

            if (character == null)
            {
                return NotFound();
            }
            
            if (Ability.Type.TryParse(rollType, true, out Ability.Type abilityType))
            {
                var roll = character.GetRoll(new CharacterRollRequest(abilityType));
                return await _roller.Roll(roll);
            }
            else
            {
                return NotFound($"Ability {rollType} not found");
            }
            throw new NotImplementedException("TODO: implement skill checks");
        }

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/rollsave/{ability}")]
        public async Task<ActionResult<int>> MakeSavingThrow(int id, string ability)
        {
            var character = _repository.GetById(id);

            if (character == null)
            {
                return NotFound();
            }

            try
            {
                var roll = character.GetSavingThrow(ability);

                return await _roller.Roll(roll);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Ability {ability} not found");
            }
        }

        //// GET: api/<CharactersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<CharactersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<CharactersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CharactersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CharactersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
