using DnD_5e.Api.Services;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Test.IntegrationTests;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.UnitTests.Api.Services
{
    public class RollTypeParserTests
    {
        private CharacterRollParser _target = new CharacterRollParser();

        [Theory]
        [InlineData("strength", Ability.Type.Strength)]
        [InlineData("dexterity", Ability.Type.Dexterity)]
        [InlineData("constitution", Ability.Type.Constitution)]
        [InlineData("intelligence", Ability.Type.Intelligence)]
        [InlineData("wisdom", Ability.Type.Wisdom)]
        [InlineData("charisma", Ability.Type.Charisma)]
        public void Parses_Ability_To_Enum(string input, Ability.Type expectedEnum)
        {
            var request = _target.ParseRequest(input);
            request.AbilityType.Should().Be(expectedEnum);
            request.RollType.Should().Be(RollTypeEnum.AbilityCheck);
        }

        [Theory]
        [InlineData("strength", Ability.Type.Strength)]
        [InlineData("dexterity", Ability.Type.Dexterity)]
        [InlineData("constitution", Ability.Type.Constitution)]
        [InlineData("intelligence", Ability.Type.Intelligence)]
        [InlineData("wisdom", Ability.Type.Wisdom)]
        [InlineData("charisma", Ability.Type.Charisma)]
        public void Parses_Ability_Save_To_Right_Roll_Type(string input, Ability.Type expectedEnum)
        {
            var request = _target.ParseRequest(input, true);
            request.AbilityType.Should().Be(expectedEnum);
            request.RollType.Should().Be(RollTypeEnum.AbilitySavingThrow);
        }

        [Theory]
        [InlineData("acrobatics", Skill.Type.Acrobatics, Ability.Type.Dexterity)]
        [InlineData("animal handling", Skill.Type.AnimalHandling, Ability.Type.Wisdom)]
        [InlineData("arcana", Skill.Type.Arcana, Ability.Type.Intelligence)]
        [InlineData("athletics", Skill.Type.Athletics, Ability.Type.Strength)]
        [InlineData("deception", Skill.Type.Deception, Ability.Type.Charisma)]
        [InlineData("history", Skill.Type.History, Ability.Type.Intelligence)]
        [InlineData("insight", Skill.Type.Insight, Ability.Type.Wisdom)]
        [InlineData("intimidation", Skill.Type.Intimidation, Ability.Type.Charisma)]
        [InlineData("investigation", Skill.Type.Investigation, Ability.Type.Intelligence)]
        [InlineData("medicine", Skill.Type.Medicine, Ability.Type.Wisdom)]
        [InlineData("nature", Skill.Type.Nature, Ability.Type.Intelligence)]
        [InlineData("perception", Skill.Type.Perception, Ability.Type.Wisdom)]
        [InlineData("performance", Skill.Type.Performance, Ability.Type.Charisma)]
        [InlineData("persuasion", Skill.Type.Persuasion, Ability.Type.Charisma)]
        [InlineData("religion", Skill.Type.Religion, Ability.Type.Intelligence)]
        [InlineData("sleight of hand", Skill.Type.SleightOfHand, Ability.Type.Dexterity)]
        [InlineData("stealth", Skill.Type.Stealth, Ability.Type.Dexterity)]
        [InlineData("survival", Skill.Type.Survival, Ability.Type.Wisdom)]
        public void Parses_Skill_To_Enum(string input, Skill.Type expectedSkillType, Ability.Type expectedAbilityType)
        {
            var request = _target.ParseRequest(input);
            request.SkillType.Should().Be(expectedSkillType);
            request.AbilityType.Should().Be(expectedAbilityType);
            request.RollType.Should().Be(RollTypeEnum.SkillCheck);
        }

        [Fact]
        public void Parses_Initiative()
        {
            
            var result = _target.ParseRequest("initiative");

            result.RollType.Should().Be(RollTypeEnum.Initiative);
        }
    }
}
