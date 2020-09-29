using System.Runtime.InteropServices.ComTypes;
using DnD_5e.Domain.CharacterRolls;
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
            [InlineData(12, "1d20p1")]
            [InlineData(13, "1d20p1")]
            [InlineData(14, "1d20p2")]
            [InlineData(15, "1d20p2")]
            [InlineData(16, "1d20p3")]
            [InlineData(17, "1d20p3")]
            [InlineData(18, "1d20p4")]
            [InlineData(19, "1d20p4")]
            public void Returns_ability_check_roll_based_on_score(int score, string expectedRoll)
            {
                var target = new Character(new Ability(score, false),
                    null, null, null, null, null, null);

                target.GetRoll(new CharacterRollRequest(Ability.Type.Strength))
                    .Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Strength, "1d20")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Dexterity, "1d20p1")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Constitution, "1d20p2")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Intelligence, "1d20p3")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Wisdom, "1d20p4")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Charisma, "1d20p5")]
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
                    new Ability(charisma, false), null
                );
                target.GetRoll(new CharacterRollRequest(requestedAbility))
                    .Should().Be(expectedRoll);
            }
        }

        public class AbilitySavingThrows : CharacterTests
        {
            [Theory]
            [InlineData(10, false, "1d20")]
            [InlineData(10, true, "1d20p2")]
            [InlineData(11, false, "1d20")]
            [InlineData(11, true, "1d20p2")]
            [InlineData(12, false, "1d20p1")]
            [InlineData(12, true, "1d20p3")]
            [InlineData(13, false, "1d20p1")]
            [InlineData(13, true, "1d20p3")]
            [InlineData(14, false, "1d20p2")]
            [InlineData(14, true, "1d20p4")]
            [InlineData(15, false, "1d20p2")]
            [InlineData(15, true, "1d20p4")]
            [InlineData(16, false, "1d20p3")]
            [InlineData(16, true, "1d20p5")]
            [InlineData(17, false, "1d20p3")]
            [InlineData(17, true, "1d20p5")]
            [InlineData(18, false, "1d20p4")]
            [InlineData(18, true, "1d20p6")]
            [InlineData(19, false, "1d20p4")]
            [InlineData(19, true, "1d20p6")]
            public void Returns_ability_saving_throw_based_on_score_and_proficiency(int score, bool isProficient, string expectedRoll)
            {
                var target = new Character(new Ability(score, isProficient),
                    null, null, null, null, null, null);

                target.GetRoll(new CharacterRollRequest(Ability.Type.Strength, true))
                    .Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Strength, "1d20p2")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Dexterity, "1d20p3")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Constitution, "1d20p4")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Intelligence, "1d20p5")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Wisdom, "1d20p6")]
            [InlineData(10, 12, 14, 16, 18, 20, Ability.Type.Charisma, "1d20p7")]
            public void Checks_the_correct_ability_score(int strength, int dexterity,
                int constitution, int intelligence, int wisdom, int charisma,
                Ability.Type requestedAbility, string expectedRoll)
            {
                var target = new Character(new Ability(strength, true),
                    new Ability(dexterity, true), new Ability(constitution, true),
                    new Ability(intelligence, true), new Ability(wisdom, true),
                    new Ability(charisma, true), null);

                target.GetRoll(new CharacterRollRequest(requestedAbility, true))
                    .Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(6500, "1d20p6")]
            [InlineData(48000, "1d20p7")]
            [InlineData(120000, "1d20p8")]
            [InlineData(225000, "1d20p9")]
            public void Adds_correct_proficiency_based_on_character_level(int experiencePoints, string expectedRoll)
            {
                var target = new Character(null, null, new Ability(16, true), null, null, null, null,
                    experiencePoints);

                target.GetRoll(new CharacterRollRequest(Ability.Type.Constitution, true))
                    .Should().Be(expectedRoll);
            }
        }

        public class SkillChecks : CharacterTests
        {
            [Fact]
            public void Uses_parent_ability_for_roll()
            {
                var roll = new CharacterRollRequest(Skill.Type.Athletics, Ability.Type.Strength);
                var character = new Character(new Ability(16, false), null, null, null, null, null, null);

                character.GetRoll(roll).Should().Be("1d20p3");
            }

            [Fact]
            public void Includes_proficiency_bonus_when_user_proficient_in_skill()
            {
                var roll = new CharacterRollRequest(Skill.Type.Athletics, Ability.Type.Dexterity);
                var character = new Character(null, new Ability(20, false), null, null, null, null,
                    new[] { Skill.Type.Athletics });

                character.GetRoll(roll).Should().Be("1d20p7");
            }
        }

        public class InitiativeRoll : CharacterTests
        {
            [Fact]
            public void Uses_dexterity_for_initiative_roll()
            {
                var roll = new CharacterRollRequest(RollTypeEnum.Initiative);
                var character = new Character(null, new Ability(14, false), null, null, null, null, null);

                character.GetRoll(roll).Should().Be("1d20p2");
            }

        }

    }
}
