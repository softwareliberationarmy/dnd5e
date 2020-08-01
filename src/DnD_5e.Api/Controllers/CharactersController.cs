using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DnD_5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly CharacterDbContext _db;
        private readonly DieRoller _roller;

        public CharactersController(CharacterDbContext db, DieRoller roller)
        {
            _db = db;
            _roller = roller;
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

        // GET api/<CharactersController>/5/roll/strength
        [HttpGet("{id}/roll/{ability}")]
        public async Task<ActionResult<int>> MakeAbilityCheck(int id, string ability)
        {
            var character = _db.Characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
            {
                return NotFound();
            }
            var strength = character.Strength;
            var modifier = (strength - 10) / 2;
            var roll = "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier: "");
            return await _roller.Roll(roll);
        }

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
