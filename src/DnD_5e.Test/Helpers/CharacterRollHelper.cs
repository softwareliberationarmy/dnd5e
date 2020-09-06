using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using FluentAssertions;

namespace DnD_5e.Test.Helpers
{
    public class CharacterRollHelper
    {
        private readonly TestClientFactory _clientFactory;
        private CharacterEntity _characterEntity;
        private string _rollType;

        public CharacterRollHelper(TestClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public CharacterRollHelper GivenACharacter(CharacterEntity characterEntity)
        {
            _characterEntity = characterEntity;
            return this;
        }

        public CharacterRollHelper WhenIRollFor(string rollType)
        {
            _rollType = rollType;
            return this;
        }

        public async Task ThenTheRollIs1d20Plus(int expectedModifier)
        {
            await _clientFactory.SetupCharacters(_characterEntity);

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"api/characters/1/roll/{_rollType}");

            var minReturnValue = 1 + expectedModifier;
            var maxReturnValue = 20 + expectedModifier;

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            roll.Should().BeInRange(minReturnValue, maxReturnValue, "Expected initiative roll to use player's dexterity");
        }
    }
}