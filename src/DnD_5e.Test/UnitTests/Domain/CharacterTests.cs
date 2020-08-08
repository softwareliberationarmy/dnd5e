﻿using System;
using DnD_5e.Domain.Roleplay;
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
                var target = new Character
                {
                    Strength = new Ability(score, false)
                };

                target.GetAbilityRoll("strength").Should().Be(expectedRoll);
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, "strength", "1d20")]
            [InlineData(10, 12, 14, 16, 18, 20, "dexterity", "1d20+1")]
            [InlineData(10, 12, 14, 16, 18, 20, "constitution", "1d20+2")]
            [InlineData(10, 12, 14, 16, 18, 20, "intelligence", "1d20+3")]
            [InlineData(10, 12, 14, 16, 18, 20, "wisdom", "1d20+4")]
            [InlineData(10, 12, 14, 16, 18, 20, "charisma", "1d20+5")]
            public void Checks_the_correct_ability_score(int strength, int dexterity,
                int constitution, int intelligence, int wisdom, int charisma,
                string requestedAbility, string expectedRoll)
            {
                var target = new Character
                {
                    Strength = new Ability(strength, false),
                    Dexterity = new Ability(dexterity, false),
                    Constitution = new Ability(constitution, false),
                    Intelligence = new Ability(intelligence, false),
                    Wisdom = new Ability(wisdom, false),
                    Charisma = new Ability(charisma, false)
                };

                target.GetAbilityRoll(requestedAbility).Should().Be(expectedRoll);
            }

            [Fact]
            public void Throws_exception_if_invalid_ability_score_given()
            {
                var target = new Character();
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => target.GetAbilityRoll("jump roping"));
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
                var target = new Character
                {
                    Strength = new Ability(score, isProficient)
                };

                target.GetSavingThrow("strength").Should().Be(expectedRoll);
            }

            [Fact]
            public void Throws_exception_if_invalid_ability_score_given()
            {
                var target = new Character();
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => target.GetSavingThrow("jump roping"));
            }

            [Theory]
            [InlineData(10, 12, 14, 16, 18, 20, "strength", "1d20+2")]
            [InlineData(10, 12, 14, 16, 18, 20, "dexterity", "1d20+3")]
            [InlineData(10, 12, 14, 16, 18, 20, "constitution", "1d20+4")]
            [InlineData(10, 12, 14, 16, 18, 20, "intelligence", "1d20+5")]
            [InlineData(10, 12, 14, 16, 18, 20, "wisdom", "1d20+6")]
            [InlineData(10, 12, 14, 16, 18, 20, "charisma", "1d20+7")]
            public void Checks_the_correct_ability_score(int strength, int dexterity,
                int constitution, int intelligence, int wisdom, int charisma,
                string requestedAbility, string expectedRoll)
            {
                var target = new Character
                {
                    Strength = new Ability(strength, true),
                    Dexterity = new Ability(dexterity, true),
                    Constitution = new Ability(constitution, true),
                    Intelligence = new Ability(intelligence, true),
                    Wisdom = new Ability(wisdom, true),
                    Charisma = new Ability(charisma, true)
                };

                target.GetSavingThrow(requestedAbility).Should().Be(expectedRoll);
            }
        }
    }
}
