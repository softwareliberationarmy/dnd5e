using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess.Entities;
using DnD_5e.Test.Helpers;
using DnD_5e.Utilities.Test;
using Xunit;

namespace DnD_5e.Test.Api.IntegrationTests.CharacterRolls
{
    public class InitiativeRollApiTest: TestBase, IClassFixture<TestClientFactory>
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
