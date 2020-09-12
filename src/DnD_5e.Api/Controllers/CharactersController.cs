using System;
using System.Threading.Tasks;
using DnD_5e.Api.Services;
using DnD_5e.Domain.DiceRolls;
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
        private readonly RollTypeParser _rollParser;

        public CharactersController(CharacterRepository repository, DieRoller roller, RollTypeParser rollParser)
        {
            _repository = repository;
            _roller = roller;
            _rollParser = rollParser;
        }

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{rollType}")]
        public async Task<ActionResult<int>> MakeCharacterRoll(int id, string rollType)
        {
            try
            {
                var request = _rollParser.ParseRequest(rollType);
                var character = await _repository.GetById(id);

                if (character == null)
                {
                    return NotFound();
                }
                var roll = character.GetRoll(request);
                return (await _roller.Roll(roll)).Result;
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Ability {rollType} not found");
            }
        }

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{ability}/save")]
        public async Task<ActionResult<int>> MakeSavingThrow(int id, string ability)
        {
            try
            {
                var request = _rollParser.ParseRequest(ability, isSave: true);
                var character = await _repository.GetById(id);

                if (character == null)
                {
                    return NotFound();
                }
                var roll = character.GetRoll(request);

                return (await _roller.Roll(roll)).Result;
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
