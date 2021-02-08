using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Entities;
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
            var nameIdentifier = "google|123456789";
            using var factory = new TestClientFactory().WithUser(nameIdentifier);

            var userDbRecord = new UserEntity{ Id = 27, Name = nameIdentifier};
            await factory.SetupDatabase(
                users: new[] {userDbRecord},
                characters: new[]
                {
                    new CharacterEntity { Id = expectedId, Owner = userDbRecord, Name = "Fred" },
                    new CharacterEntity { Id = expectedId + 1, Owner = userDbRecord, Name = "Bill" },
                    new CharacterEntity { Id = expectedId - 1, Owner = userDbRecord, Name = "Olaf" }
                });

            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            var response = await client.GetAsync("api/characters");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var characters = (CharacterResponse[])JsonSerializer.Deserialize(json, typeof(CharacterResponse[]),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //should return just my characters
            characters.Sum(i => i.Id).Should().Be(expectedId * 3);
            
        }
    }

    public class CharacterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
