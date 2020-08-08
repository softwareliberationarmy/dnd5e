using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.Roll
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
            roll.Should().BeInRange(minReturnValue, maxReturnValue);
        }

        [Fact]
        public async Task GetWithoutRequestStringReturns1d20Roll()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll");

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            roll.Should().BeInRange(1, 20);
        }

        [Fact]
        public async Task GetWithInvalidStringReturnsErrorCode()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll/tide");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
