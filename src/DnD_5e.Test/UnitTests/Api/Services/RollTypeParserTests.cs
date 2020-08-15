using DnD_5e.Api.Services;
using DnD_5e.Domain.Roleplay;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.UnitTests.Api.Services
{
    public class RollTypeParserTests
    {
        [Theory]
        [InlineData("strength", Ability.Type.Strength)]
        [InlineData("dexterity", Ability.Type.Dexterity)]
        [InlineData("constitution", Ability.Type.Constitution)]
        [InlineData("intelligence", Ability.Type.Intelligence)]
        [InlineData("wisdom", Ability.Type.Wisdom)]
        [InlineData("charisma", Ability.Type.Charisma)]
        public void Parses_Ability_To_Enum(string input, Ability.Type expectedEnum)
        {
            var target = new RollTypeParser();
            var result = target.ParseRequest(input);
            result.AbilityType.Should().Be(expectedEnum);
        }
    }
}
