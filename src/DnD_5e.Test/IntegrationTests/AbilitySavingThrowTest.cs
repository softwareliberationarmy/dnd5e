using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Domain.Roleplay;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests
{
    public class AbilitySavingThrowTest : IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _factory;

        public AbilitySavingThrowTest(TestClientFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Makes_strength_saving_throw_with_proficiency()
        {
            await _factory.SetupCharacters(new CharacterEntity
            {
                Id = 1,
                Strength = 16
            });

            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/characters/1/rollsave/strength");
            var expectedModifier = 5;   //ability modifier + proficiency at 1st level (2)
            var minReturnValue = 1 + expectedModifier;
            var maxReturnValue = 20 + expectedModifier;

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            roll.Should().BeInRange(minReturnValue, maxReturnValue, "Expected strength saving throw to be within bounds");
        }

        //TODO: test out character not found scenario
        //TODO: test out ability not found
        //TODO: add support for other abilities
        //TODO: add unit tests for character 
        //TODO: move logic for getting the modifier from the character to ability

        //[Fact]
        //public async Task Returns_404_When_Character_Id_Not_Valid()
        //{
        //    await _factory.SetupCharacters();   //clears out all characters and inserts none

        //    var client = _factory.CreateClient();

        //    var response = await client.GetAsync("api/characters/1/roll/strength");

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        //[Theory]
        //[InlineData(10, 12, 14, 16, 18, 20, "strength", 0)]
        //[InlineData(10, 12, 14, 16, 18, 20, "dexterity", 1)]
        //[InlineData(10, 12, 14, 16, 18, 20, "constitution", 2)]
        //[InlineData(10, 12, 14, 16, 18, 20, "intelligence", 3)]
        //[InlineData(10, 12, 14, 16, 18, 20, "wisdom", 4)]
        //[InlineData(10, 12, 14, 16, 18, 20, "charisma", 5)]
        //public async Task Checks_all_abilities(int strength, int dexterity,
        //    int constitution, int intelligence, int wisdom, int charisma,
        //    string abilityToTest, int expectedModifier)
        //{
        //    await _factory.SetupCharacters(new CharacterEntity
        //    {
        //        Id = 1,
        //        Strength = strength,
        //        Dexterity = dexterity,
        //        Constitution = constitution,
        //        Intelligence = intelligence,
        //        Wisdom = wisdom,
        //        Charisma = charisma
        //    });

        //    var client = _factory.CreateClient();

        //    var response = await client.GetAsync($"api/characters/1/roll/{abilityToTest}");

        //    var minReturnValue = 1 + expectedModifier;
        //    var maxReturnValue = 20 + expectedModifier;

        //    response.EnsureSuccessStatusCode();
        //    var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
        //    roll.Should().BeInRange(minReturnValue, maxReturnValue,
        //        $"{abilityToTest} roll must be within the expected bounds");
        //}

        //[Fact]
        //public async Task Returns_404_for_invalid_ability_name()
        //{
        //    await _factory.SetupCharacters(new CharacterEntity
        //    {
        //        Id = 1, Strength = 15, Dexterity = 15, Constitution = 15, 
        //        Intelligence = 15, Wisdom = 15, Charisma = 15
        //    });

        //    var client = _factory.CreateClient();

        //    var response = await client.GetAsync("api/characters/1/roll/efficiency");

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}
    }
}
