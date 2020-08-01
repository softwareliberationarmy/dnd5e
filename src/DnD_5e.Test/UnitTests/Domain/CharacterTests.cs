using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DnD_5e.Domain.Roleplay;
using Xunit;

namespace DnD_5e.Test.UnitTests.Domain
{
    public class CharacterTests
    {
        [Theory]
        [InlineData(10, "1d20")]
        [InlineData(11, "1d20")]
        [InlineData(12, "1d20+1")]
        [InlineData(13, "1d20+1")]
        [InlineData(14, "1d20+2")]
        [InlineData(15, "1d20+2")]
        [InlineData(16, "1d20+3")]
        [InlineData(17, "1d20+3")]
        [InlineData(18, "1d20+4")]
        [InlineData(19, "1d20+4")]
        public async void Returns_ability_check_roll_based_on_score(int score, string expectedRoll)
        {
            var target = new Character
            {
                Strength = score
            };

            Assert.Equal(expectedRoll, target.GetAbilityRoll("strength"));
        }
    }
}
