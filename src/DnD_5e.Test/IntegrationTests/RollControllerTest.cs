using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Text.Json;
using DnD_5e.Test.Helpers;

namespace DnD_5e.Test.IntegrationTests
{

    public class RollControllerTest: IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _factory;

        public RollControllerTest(TestClientFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("1d20", 1, 20)]
        [InlineData("2d20", 2, 40)]
        [InlineData("1d4+8", 9, 12)]
        public async Task GetReturnsRandomDiceRoll(string input, int minReturnValue, int maxReturnValue)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/roll/{input}");

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            Assert.True(roll <= maxReturnValue && roll >= minReturnValue);
        }

        [Fact]
        public async Task GetWithoutRequestStringReturns1d20Roll()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll");

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            Assert.True(roll <= 20 && roll >= 1);
        }

        [Fact]
        public async Task GetWithInvalidStringReturnsErrorCode()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll/tide");

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
