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
        [Fact]
        public async void Returns_ability_check_roll_based_on_score()
        {
            var target = new Character
            {
                Strength = 16
            };

            Assert.Equal("1d20+3", target.GetAbilityRoll("strength"));
        }
    }
}
