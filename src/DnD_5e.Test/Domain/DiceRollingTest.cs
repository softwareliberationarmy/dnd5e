using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DnD_5e.Test.Domain
{
    public class DiceRollingTest
    {
        [Theory]
        [InlineData("1d6", 1, 6)]
        public async Task Dice_Roll_Falls_In_Range(string diceRollRequest, int minValue, int maxValue)
        {
            var target = new DieRoller();
            var roll = await target.Roll(diceRollRequest);
            Assert.True(roll >= minValue && roll <= maxValue);
        }
    }

    public class DieRoller
    {
        public async Task<int> Roll(string requestString)
        {
            var parsedRequest = await Parse(requestString);
            return await ProcessRoll(parsedRequest);
        }

        private async Task<int> ProcessRoll(DiceRollRequest parsedRequest)
        {
            throw new NotImplementedException();
        }

        private async Task<DiceRollRequest> Parse(string requestString)
        {
            throw new NotImplementedException();
        }
    }

    internal class DiceRollRequest
    {
        public int Quantity { get; set; }
        public int Sides { get; set; }
        public int Modifier { get; set; }
    }
}
