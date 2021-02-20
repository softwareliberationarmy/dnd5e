using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;
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

            var userDbRecord = new UserEntity { Id = 27, Name = nameIdentifier };
            await factory.SetupDatabase(
                users: new[] { userDbRecord },
                characters: new[]
                {
                    new CharacterEntity { Id = expectedId, Owner = userDbRecord, Name = "Gandalf",
                        Class = Class.Wizard, Race = Race.Human},
                    new CharacterEntity { Id = expectedId + 1, Owner = userDbRecord, Name = "Gimli",
                        Class = Class.Fighter, Race = Race.Dwarf},
                    new CharacterEntity { Id = expectedId - 1, Owner = userDbRecord, Name = "Legolas",
                        Class = Class.Ranger, Race=Race.Elf}
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
            string.Join("", characters.Select(c => c.Class)).Should().Be("WizardFighterRanger");
            string.Join("", characters.Select(c => c.Race)).Should().Be("HumanDwarfElf");
        }

        [Fact]
        public async Task ReturnsRightCharacterLevels()
        {
            var expectedId = 25;
            var nameIdentifier = "google|123456789";
            using var factory = new TestClientFactory().WithUser(nameIdentifier);

            var userDbRecord = new UserEntity { Id = 27, Name = nameIdentifier };
            await factory.SetupDatabase(
                users: new[] { userDbRecord },
                characters: new[]
                {
                    new CharacterEntity { Id = expectedId, Owner = userDbRecord, Name = "Gandalf",
                        Class = Class.Wizard, Race = Race.Human, ExperiencePoints = 299},
                    new CharacterEntity { Id = expectedId + 1, Owner = userDbRecord, Name = "Gimli",
                        Class = Class.Fighter, Race = Race.Dwarf, ExperiencePoints = 899},
                    new CharacterEntity { Id = expectedId - 1, Owner = userDbRecord, Name = "Legolas",
                        Class = Class.Ranger, Race=Race.Elf, ExperiencePoints = 356000}
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
            string.Join("", characters.Select(c => c.Class)).Should().Be("WizardFighterRanger");
            string.Join("", characters.Select(c => c.Race)).Should().Be("HumanDwarfElf");
            characters.Select(c => c.Level).ToArray().Should().BeEquivalentTo(new[] {1, 2, 20});
        }
    }

    public class CharacterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public int Level { get; set; }
    }
}
