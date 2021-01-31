using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Entities;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.CharacterRolls
{
    public class AbilitySavingThrowApiTest : IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _factory;

        public AbilitySavingThrowApiTest(TestClientFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Makes_strength_saving_throw_with_proficiency()
        {
            await _factory.CharacterRoll()
                .GivenACharacter(new CharacterEntity
                {
                    Id = 1,
                    Strength = 16,
                    StrengthSaveProficiency = true
                })
                .WhenIRollFor("strength/save")
                .ThenTheRollIs1d20Plus(5);
        }

        [Fact]
        public async Task Makes_strength_saving_throw_without_proficiency()
        {
            await _factory.CharacterRoll()
                .GivenACharacter(new CharacterEntity
                {
                    Id = 1,
                    Strength = 16,
                    StrengthSaveProficiency = false
                })
                .WhenIRollFor("strength/save")
                .ThenTheRollIs1d20Plus(3);
        }

        [Fact]
        public async Task Returns_404_When_Character_Id_Not_Valid()
        {
            await _factory.CharacterRoll()
                .GivenNoCharacters()
                .WhenIRollFor("strength/save")
                .ThenTheApiReturnsNotFound();
        }

        [Theory]
        [InlineData(10, 12, 14, 16, 18, 20, "strength", false, 0)]
        [InlineData(10, 12, 14, 16, 18, 20, "dexterity", false, 1)]
        [InlineData(10, 12, 14, 16, 18, 20, "constitution", false, 2)]
        [InlineData(10, 12, 14, 16, 18, 20, "intelligence", false, 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "wisdom", false, 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "charisma", false, 5)]
        [InlineData(10, 12, 14, 16, 18, 20, "strength", true, 2)]
        [InlineData(10, 12, 14, 16, 18, 20, "dexterity", true, 3)]
        [InlineData(10, 12, 14, 16, 18, 20, "constitution", true, 4)]
        [InlineData(10, 12, 14, 16, 18, 20, "intelligence", true, 5)]
        [InlineData(10, 12, 14, 16, 18, 20, "wisdom", true, 6)]
        [InlineData(10, 12, 14, 16, 18, 20, "charisma", true, 7)]
        public async Task Checks_all_abilities(int strength, int dexterity,
            int constitution, int intelligence, int wisdom, int charisma,
            string abilityToTest, bool hasProficiency, int expectedModifier)
        {
            await _factory.CharacterRoll()
                .GivenACharacter(new CharacterEntity
                {
                    Id = 1,
                    Strength = strength,
                    Dexterity = dexterity,
                    Constitution = constitution,
                    Intelligence = intelligence,
                    Wisdom = wisdom,
                    Charisma = charisma,
                    StrengthSaveProficiency = hasProficiency,
                    DexteritySaveProficiency = hasProficiency,
                    ConstitutionSaveProficiency = hasProficiency,
                    IntelligenceSaveProficiency = hasProficiency,
                    WisdomSaveProficiency = hasProficiency,
                    CharismaSaveProficiency = hasProficiency
                })
                .WhenIRollFor($"{abilityToTest}/save")
                .ThenTheRollIs1d20Plus(expectedModifier);
        }

        [Fact]
        public async Task Returns_404_for_invalid_ability_name()
        {
            await _factory.CharacterRoll()
                .GivenACharacter(new CharacterEntity
                {
                    Id = 1,
                    Strength = 15,
                    Dexterity = 15,
                    Constitution = 15,
                    Intelligence = 15,
                    Wisdom = 15,
                    Charisma = 15
                })
                .WhenIRollFor("excellence/save")
                .ThenTheApiReturnsNotFound();
        }

        [Fact]
        public async Task Proficiency_modifier_changes_with_character_level()
        {
            await _factory.CharacterRoll()
                .GivenACharacter(new CharacterEntity
                {
                    Id = 1,
                    Constitution = 16,
                    ConstitutionSaveProficiency = true,
                    ExperiencePoints = 6500
                })
                .WhenIRollFor("constitution/save")
                .ThenTheRollIs1d20Plus(6);
        }
    }
}
