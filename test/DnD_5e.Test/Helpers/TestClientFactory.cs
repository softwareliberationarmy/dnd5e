using System;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DnD_5e.Test.Helpers
{
    public class TestClientFactory : WebApplicationFactory<Api.Startup>
    {
        private readonly string _databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
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
    }
}