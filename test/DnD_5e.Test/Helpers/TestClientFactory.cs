using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DnD_5e.Test.Helpers
{
    public class TestClientFactory : WebApplicationFactory<Api.Startup>
    {
        private readonly string _databaseName = Guid.NewGuid().ToString();
        private string _nameIdentifier;
        public Dictionary<string, string> ConfigurationInfo { get; } = new Dictionary<string, string>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureTestServices(services =>
            {
                if (_nameIdentifier != null)
                {
                    var evaluator = new FakePolicyEvaluator(_nameIdentifier);
                    services.AddSingleton<IPolicyEvaluator>(evaluator);
                }
            });
            builder.ConfigureServices(services =>
            {
                RegisterInMemoryDatabase(services);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CharacterDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestClientFactory>>();

                    db.Database.EnsureCreated();
                }
            });
            builder.ConfigureAppConfiguration(AddConfigurationValues);
        }

        private void AddConfigurationValues(WebHostBuilderContext context, IConfigurationBuilder bldr)
        {
            bldr.AddInMemoryCollection(ConfigurationInfo);
        }

        private void RegisterInMemoryDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<CharacterDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<CharacterDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });
        }

        public async Task SetupCharacters(params CharacterEntity[] characters)
        {
            var options = new DbContextOptionsBuilder<CharacterDbContext>()
                .UseInMemoryDatabase(_databaseName).Options;

            await using (var context = new CharacterDbContext(options))
            {
                var existing = context.Character.ToList();
                foreach (var character in existing)
                {
                    context.Character.Remove(character);
                }
                await context.Character.AddRangeAsync(characters);
                await context.SaveChangesAsync();
            }
        }

        public CharacterRollTestHelper CharacterRoll()
        {
            return new CharacterRollTestHelper(this);
        }

        public TestClientFactory WithUserNameIdentifier(string nameIdentifier)
        {
            _nameIdentifier = nameIdentifier;
            return this;
        }

        public async Task SetupUser(int userId, string userName)
        {
            var options = new DbContextOptionsBuilder<CharacterDbContext>()
                .UseInMemoryDatabase(_databaseName).Options;

            await using (var context = new CharacterDbContext(options))
            {
                var existing = context.User.ToList();
                foreach (var user in existing)
                {
                    context.User.Remove(user);
                }
                await context.User.AddAsync(new UserEntity { Id = userId, Name = userName });
                await context.SaveChangesAsync();
            }
        }
    }
}