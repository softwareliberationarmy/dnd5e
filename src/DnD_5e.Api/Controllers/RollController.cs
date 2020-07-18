using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DnD_5e.Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<int> Get()
        {
            //if no roll request is provided, assume the user wants a 1d20 roll
            return await Get("1d20");
        }

        // GET api/<RollController>/1d20
        [HttpGet("{rollRequest}")]
        public async Task<int> Get(string rollRequest)
        {
            return await _roller.Roll(rollRequest);
        }
    }
}
