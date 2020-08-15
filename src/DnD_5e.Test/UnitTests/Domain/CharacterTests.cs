using System;
using DnD_5e.Domain.Roleplay;
using DnD_5e.Test.UnitTests.Api.Services;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.UnitTests.Domain
{
    public class CharacterTests
    {
        public class AbilityChecks : CharacterTests
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
            public void Returns_ability_check_roll_based_on_score(int score, string expectedRoll)
            {
                var target = new Character(new Ability(score, false),
                    null, null, null, null, null);

                target.GetRoll(new CharacterRollRequest(Ability.Type.Strength))
                    .Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Strength, "1d20")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Dexterity, "1d20+1")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Constitution, "1d20+2")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Intelligence, "1d20+3")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Wisdom, "1d20+4")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Charisma, "1d20+5")]
            public void Checks_the_correct_ability_score(int strength, int dexterity,
                int constitution, int intelligence, int wisdom, int charisma,
                Ability.Type requestedAbility, string expectedRoll)
            {
                var target = new Character(
                    new Ability(strength, false),
                    new Ability(dexterity, false),
                    new Ability(constitution, false),
                    new Ability(intelligence, false),
                    new Ability(wisdom, false),
                    new Ability(charisma, false)
                );
                target.GetRoll(new CharacterRollRequest(requestedAbility))
                    .Should().Be(expectedRoll);
            }
        }

        public class AbilitySavingThrows : CharacterTests
        {
            [Theory]
            [InlineData(10, false, "1d20")]
            [InlineData(10, true, "1d20+2")]
            [InlineData(11, false, "1d20")]
            [InlineData(11, true, "1d20+2")]
            [InlineData(12, false, "1d20+1")]
            [InlineData(12, true, "1d20+3")]
            [InlineData(13, false, "1d20+1")]
            [InlineData(13, true, "1d20+3")]
            [InlineData(14, false, "1d20+2")]
            [InlineData(14, true, "1d20+4")]
            [InlineData(15, false, "1d20+2")]
            [InlineData(15, true, "1d20+4")]
            [InlineData(16, false, "1d20+3")]
            [InlineData(16, true, "1d20+5")]
            [InlineData(17, false, "1d20+3")]
            [InlineData(17, true, "1d20+5")]
            [InlineData(18, false, "1d20+4")]
            [InlineData(18, true, "1d20+6")]
            [InlineData(19, false, "1d20+4")]
            [InlineData(19, true, "1d20+6")]
            public void Returns_ability_saving_throw_based_on_score_and_proficiency(int score, bool isProficient, string expectedRoll)
            {
                var target = new Character(new Ability(score, isProficient),
                    null, null, null, null, null);

                target.GetRoll(new CharacterRollRequest(Ability.Type.Strength, true))
                    .Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Strength, "1d20+2")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Dexterity, "1d20+3")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Constitution, "1d20+4")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Intelligence, "1d20+5")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Wisdom, "1d20+6")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Charisma, "1d20+7")]
            public void Checks_the_correct_ability_score(int strength, int dexterity,
                int constitution, int intelligence, int wisdom, int charisma,
                Ability.Type requestedAbility, string expectedRoll)
            {
                var target = new Character(new Ability(strength, true),
                    new Ability(dexterity, true), new Ability(constitution, true),
                    new Ability(intelligence, true), new Ability(wisdom, true),
                    new Ability(charisma, true));

                target.GetRoll(new CharacterRollRequest(requestedAbility, true))
                    .Should().Be(expectedRoll);
            }
        }
    }
}
