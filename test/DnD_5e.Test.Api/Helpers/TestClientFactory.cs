using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Entities;
using DnD_5e.Test.Helpers.ApiSecurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DnD_5e.Test.Helpers
{
    public class TestClientFactory : WebApplicationFactory<DnD_5e.Api.Startup>
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
                    FakeOutAuthenticationAndBypassAuthorization(services);
                }
            });
            builder.ConfigureServices(services =>
            {
                RegisterInMemoryDatabase(services);

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CharacterDbContext>();
                
                db.Database.EnsureCreated();
            });
            builder.ConfigureAppConfiguration(AddConfigurationValues);
        }

        private void FakeOutAuthenticationAndBypassAuthorization(IServiceCollection services)
        {
            //allows us to bypass authorization and to pass in a custom ClaimsPrincipal to the controller method
            var fakePolicyEvaluator = new FakePolicyEvaluator(_nameIdentifier);
            services.AddSingleton<IPolicyEvaluator>(fakePolicyEvaluator);
            services.AddSingleton(fakePolicyEvaluator);

            //allows us to point authentication by default to our mock authentication handler
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = FakePolicyEvaluator.TestScheme;
                x.DefaultScheme = FakePolicyEvaluator.TestScheme;
            }).AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                FakePolicyEvaluator.TestScheme, options => { });
        }

        private void AddConfigurationValues(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddInMemoryCollection(ConfigurationInfo);
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

            await using var context = new CharacterDbContext(options);
            var existing = context.Character.ToList();
            foreach (var character in existing)
            {
                context.Character.Remove(character);
            }
            await context.Character.AddRangeAsync(characters);
            await context.SaveChangesAsync();
        }

        public CharacterRollTestHelper CharacterRoll()
        {
            return new CharacterRollTestHelper(this);
        }

        public TestClientFactory WithUser(string nameIdentifier)
        {
            //NOTE: additional claim KVPs can be added to this method
            _nameIdentifier = nameIdentifier;
            return this;
        }

        public async Task SetupDatabase(UserEntity[] users, CharacterEntity[] characters)
        {
            var options = new DbContextOptionsBuilder<CharacterDbContext>()
                .UseInMemoryDatabase(_databaseName).Options;

            await using var context = new CharacterDbContext(options);
            var existingUsers = context.User.ToList();
            foreach (var user in existingUsers)
            {
                context.User.Remove(user);
            }
            await context.User.AddRangeAsync(users);

            var existingChars = context.Character.ToList();
            foreach (var character in existingChars)
            {
                context.Character.Remove(character);
            }
            await context.Character.AddRangeAsync(characters);
            
            await context.SaveChangesAsync();
        }
    }
}