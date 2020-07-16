using System.Threading.Tasks;
using DnD_5e.Domain;
using Xunit;
using Xunit.Abstractions;

namespace DnD_5e.Test.Domain
{
    public class DiceRollingTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DiceRollingTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("1d6", 1, 6)]
        [InlineData("2d6", 2, 12)]
        [InlineData("1d4", 1, 4)]
        [InlineData("1d6+1", 2, 7)]
        [InlineData("1d20-1", 0, 19)]
        [InlineData("5d8", 5, 40)]
        public async Task Dice_Roll_Falls_In_Range(string diceRollRequest, int minValue, int maxValue)
        {
            var target = new DieRoller();
            var roll = await target.Roll(diceRollRequest);
            _testOutputHelper.WriteLine($"Rolls a {roll}");
            Assert.True(roll >= minValue && roll <= maxValue);
        }
    }
}
