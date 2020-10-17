using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.CharacterRolls
{
    public class InitiativeRollApiTest: IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _clientFactory;

        public InitiativeRollApiTest(TestClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Fact]
        public async Task Initiative_roll_uses_character_dexterity()
        {
            await _clientFactory.CharacterRoll().GivenACharacter(new CharacterEntity
            {
                Id = 1, Dexterity = 14
            })
                .WhenIRollFor("initiative")
                .ThenTheRollIs1d20Plus(2);
        }
    }
}
