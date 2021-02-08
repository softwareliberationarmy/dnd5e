using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace DnD_5e.Test.UnitTests.Domain
{
    public class DiceRollingTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly DieRoller _target = new DieRoller();

        public DiceRollingTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("1d6", 1, 6)]
        [InlineData("2d6", 2, 12)]
        [InlineData("1d4", 1, 4)]
        [InlineData("1d6p1", 2, 7)]
        [InlineData("1d20m1", 0, 19)]
        [InlineData("5d8", 5, 40)]
        public async Task Dice_Roll_Falls_In_Range(string diceRollRequest, int minValue, int maxValue)
        {
            var rollResponse = await _target.Roll(diceRollRequest);
            var roll = rollResponse.Result;
            _testOutputHelper.WriteLine($"Rolls a {roll}");
            roll.Should().BeInRange(minValue, maxValue, "Roll must fall within expected roll range");
            rollResponse.RequestedRoll.Should().Be(diceRollRequest);
            rollResponse.Rolls.Length.Should().Be(1);
        }

        [Fact]
        public async Task Dice_Rolls_Are_Sufficiently_Distributed()
        {
            Dictionary<int,int> results = new Dictionary<int, int>();
            for (int i = 0; i < 100; i++)
            {
                var roll = (await _target.Roll("1d20")).Result;
                if (results.ContainsKey(roll))
                {
                    results[roll]++;
                }
                else
                {
                    results[roll] = 1;
                }
            }

            results.Count.Should().BeGreaterOrEqualTo(3);
            results.Count.Should().BeLessOrEqualTo(20);
        }

        [Theory]
        [InlineData("2d4p2")]
        [InlineData("1d20")]
        [InlineData("1d20m1")]
        public async Task Roll_Result_Returns_Requested_Roll(string requestString)
        {
            var result = await _target.Roll(requestString);
            result.RequestedRoll.Should().Be(requestString);
        }

        [Theory]
        [InlineData("1d3")]
        [InlineData("6d10p6")]
        public async Task With_Advantage_Returns_Max_Of_Two_Rolls(string requestString)
        {
            var result = await _target.Roll(requestString, With.Advantage);
            result.Rolls.Length.Should().Be(2);
            result.RequestedRoll.Should().Be(requestString);
            result.Result.Should().Be(result.Rolls.Max());
        }

        [Theory]
        [InlineData("1d3")]
        [InlineData("6d10p6")]
        public async Task With_Disadvantage_Returns_Min_Of_Two_Rolls(string requestString)
        {
            var result = await _target.Roll(requestString, With.Disadvantage);
            result.Rolls.Length.Should().Be(2);
            result.RequestedRoll.Should().Be(requestString);
            result.Result.Should().Be(result.Rolls.Min());
        }
    }
}
