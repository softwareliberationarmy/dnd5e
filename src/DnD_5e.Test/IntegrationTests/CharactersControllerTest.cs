using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DnD_5e.Test.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string DatabaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RegisterInMemoryDatabase(services);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CharacterDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();
                }
            });
        }

        private void RegisterInMemoryDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<CharacterDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<CharacterDbContext>(options => { options.UseInMemoryDatabase(DatabaseName); });
        }

        public async Task SetupCharacters(params CharacterEntity[] characters)
        {
            var options = new DbContextOptionsBuilder<CharacterDbContext>()
                .UseInMemoryDatabase(DatabaseName).Options;

            using (var context = new CharacterDbContext(options))
            {
                context.Characters.AddRange(characters);
                await context.SaveChangesAsync();
            }
        }
    }

    public class CharactersControllerTest : IClassFixture<CustomWebApplicationFactory<Api.Startup>>
    {
        private readonly CustomWebApplicationFactory<Api.Startup> _factory;

        public CharactersControllerTest(CustomWebApplicationFactory<Api.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(16, 2)]
        public async Task Makes_character_strength_roll_with_right_modifier(int strengthScore, int expectedModifier)
        {
            await _factory.SetupCharacters(new CharacterEntity
            {
                Id = 1,
                Strength = strengthScore
            });

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/characters/1/roll/strength");

            var minReturnValue = 1 + expectedModifier;
            var maxReturnValue = 20 + expectedModifier;

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            Assert.True(roll <= maxReturnValue && roll >= minReturnValue);
        }

    }
}
