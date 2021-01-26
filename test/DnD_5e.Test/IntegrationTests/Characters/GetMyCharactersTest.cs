using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.Characters
{
    public class GetMyCharactersTest
    {
        [Fact]
        public async Task ReturnsJustMyCharacters()
        {
            var expectedId = 25;
            var userId = 27;
            var nameIdentifier = "google|123456789";
            using var factory = new TestClientFactory().WithUser(nameIdentifier);

            await factory.SetupUser(27, nameIdentifier);
            await factory.SetupCharacters(
                new CharacterEntity {Id = expectedId, UserId = userId, Name = "Fred"},
                new CharacterEntity {Id = expectedId + 1, UserId = userId + 1, Name = "Bill" },
                new CharacterEntity {Id = expectedId - 1, UserId = userId - 1, Name = "Pense" }
            );

            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            var response = await client.GetAsync("api/characters");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var characters = (CharacterResponse[])JsonSerializer.Deserialize(json, typeof(CharacterResponse[]),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //should return just my characters
            characters.Single().Id.Should().Be(expectedId);
        }
    }

    public class CharacterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
