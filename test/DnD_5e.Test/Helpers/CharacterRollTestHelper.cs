using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using FluentAssertions;

namespace DnD_5e.Test.Helpers
{
    public class CharacterRollTestHelper
    {
        private readonly TestClientFactory _clientFactory;
        private CharacterEntity _characterEntity;
        private string _rollType;

        public CharacterRollTestHelper(TestClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public CharacterRollTestHelper GivenNoCharacters()
        {
            _characterEntity = null;
            return this;
        }

        public CharacterRollTestHelper GivenACharacter(CharacterEntity characterEntity)
        {
            _characterEntity = characterEntity;
            return this;
        }

        public CharacterRollTestHelper WhenIRollFor(string rollType)
        {
            _rollType = rollType;
            return this;
        }

        public async Task ThenTheRollIs1d20Plus(int expectedModifier)
        {
            var characters = _characterEntity == null ? new CharacterEntity[0] : new[] { _characterEntity };
            await _clientFactory.SetupCharacters(characters);

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"api/characters/1/roll/{_rollType}");

            var minReturnValue = 1 + expectedModifier;
            var maxReturnValue = 20 + expectedModifier;

            response.EnsureSuccessStatusCode();
            var roll = TestRollResponse.FromJson(await response.Content.ReadAsStringAsync());
            roll.Result.Should().BeInRange(minReturnValue, maxReturnValue, $"Expected {_rollType} roll to be within expected bounds");
        }

        public async Task ThenTheApiReturnsNotFound()
        {
            var characters = _characterEntity == null ? new CharacterEntity[0] : new[] {_characterEntity};
            await _clientFactory.SetupCharacters(characters);

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"api/characters/1/roll/{_rollType}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}