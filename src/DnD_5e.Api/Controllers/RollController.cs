﻿using System;
using System.Threading.Tasks;
using DnD_5e.Domain.DiceRolls;
using Microsoft.AspNetCore.Mvc;

namespace DnD_5e.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<int>> Roll1d20()
        {
            //if no roll request is provided, assume the user wants a 1d20 roll
            return await RollDice("1d20");
        }

        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}")]
        public async Task<ActionResult<int>> RollDice(string rollRequest)
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
    }
}