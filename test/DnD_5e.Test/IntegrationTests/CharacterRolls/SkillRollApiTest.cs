using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Entities;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.CharacterRolls
{
    public class SkillRollApiTest : IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _factory;

        public SkillRollApiTest(TestClientFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(16, 3)]
        [InlineData(17, 3)]
        [InlineData(10, 0)]
        [InlineData(11, 0)]
        public async Task Makes_character_athletics_roll_with_right_modifier(int strengthScore, int expectedModifier)
        {
            await _factory.CharacterRoll().GivenACharacter(new CharacterEntity
            {
                Id = 1,
                Strength = strengthScore
            }).WhenIRollFor("athletics")
                .ThenTheRollIs1d20Plus(expectedModifier);
        }

        [Fact]
        public async Task Returns_404_When_Character_Id_Not_Valid()
        {
            await _factory.CharacterRoll().GivenNoCharacters()
                .WhenIRollFor("athletics")
                .ThenTheApiReturnsNotFound();  
        }

        [Theory]
        [InlineData(10, 12, 14, 16, 18, 20, "athletics", 0)]
        [InlineData(10, 12, 14, 16, 18, 20, "acrobatics", 1)]
        [InlineData(10, 12, 14, 16, 18, 20, "sleight of hand", 1)]
        [InlineData(10, 12, 14, 16, 18, 20, "stealth", 1)]
        [InlineData(10, 12, 14, 16, 18, 20, "arcana", 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "history", 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "INVESTIGATION", 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "Nature", 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "Religion", 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "animal HANDLING", 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "insight", 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "MEDIcine", 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "perception", 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "survival", 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "deception", 5)]
        [InlineData(10, 12, 14, 16, 18, 20, "persuasion", 5)]
        [InlineData(10, 12, 14, 16, 18, 20, "performance", 5)]
        [InlineData(10, 12, 14, 16, 18, 20, "intimidation", 5)]
        public async Task Checks_all_skills(int strength, int dexterity,
            int constitution, int intelligence, int wisdom, int charisma,
            string skillToTest, int expectedModifier)
        {
            await _factory.CharacterRoll().GivenACharacter(new CharacterEntity
            {
                Id = 1,
                Strength = strength,
                Dexterity = dexterity,
                Constitution = constitution,
                Intelligence = intelligence,
                Wisdom = wisdom,
                Charisma = charisma
            }).WhenIRollFor(skillToTest)
                .ThenTheRollIs1d20Plus(expectedModifier);
        }

        [Fact]
        public async Task Returns_404_for_invalid_skill_name()
        {
            await _factory.CharacterRoll().GivenACharacter(new CharacterEntity
            {
                Id = 1,
                Strength = 15,
                Dexterity = 15,
                Constitution = 15,
                Intelligence = 15,
                Wisdom = 15,
                Charisma = 15
            }).WhenIRollFor("small talk").ThenTheApiReturnsNotFound();
        }

        [Fact]
        public async Task Uses_proficiency_modifier_if_proficient()
        {
            await _factory.CharacterRoll().GivenACharacter(new CharacterEntity
            {
                Id = 1,
                Strength = 16,
                SkillProficiencies = new List<SkillProficiencyEntity>
                {
                    new SkillProficiencyEntity { Type = (int)Skill.Type.Athletics }
                }
            }).WhenIRollFor("athletics").ThenTheRollIs1d20Plus(5);
        }
    }
}
