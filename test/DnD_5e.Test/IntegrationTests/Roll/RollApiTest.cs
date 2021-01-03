using System.Net;
using System.Threading.Tasks;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.Roll
{

    public class RollApiTest : IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _factory;

        public RollApiTest(TestClientFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("1d20", 1, 20)]
        [InlineData("2d20", 2, 40)]
        [InlineData("1d4p8", 9, 12)]
        [InlineData("1d4m1", 0, 3)]
        public async Task GetReturnsRandomDiceRoll(string input, int minReturnValue, int maxReturnValue)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/roll/{input}");

            response.EnsureSuccessStatusCode();
            var roll = TestRollResponse.FromJson(await response.Content.ReadAsStringAsync());
            roll.Result.Should().BeInRange(minReturnValue, maxReturnValue);
            roll.Rolls.Length.Should().Be(1);
            roll.Rolls[0].Should().Be(roll.Result);
            roll.RequestedRoll.Should().Be(input);
        }

        [Fact]
        public async Task GetWithoutRequestStringReturns1d20Roll()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll");

            response.EnsureSuccessStatusCode();
            var roll = TestRollResponse.FromJson(await response.Content.ReadAsStringAsync());
            roll.Rolls.Length.Should().Be(1);
            roll.Rolls[0].Should().Be(roll.Result);
            roll.Result.Should().BeInRange(1, 20);
            roll.RequestedRoll.Should().Be("1d20");
        }

        [Fact]
        public async Task GetWithInvalidStringReturnsErrorCode()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/roll/tide");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RollWithAdvantageReturnsGreaterOfTwoRolls()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/roll/1d20/advantage");

            response.EnsureSuccessStatusCode();
            var rollResponse = TestRollResponse.FromJson(await response.Content.ReadAsStringAsync());
            rollResponse.Rolls.Length.Should().Be(2);
            rollResponse.Rolls.Should().Contain(rollResponse.Result);
            foreach (var roll in rollResponse.Rolls)
            {
                rollResponse.Result.Should().BeGreaterOrEqualTo(roll);
            }
            rollResponse.RequestedRoll.Should().Be("1d20");
        }

        [Fact]
        public async Task RollWithDisadvantageReturnsLesserOfTwoRolls()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/roll/1d20/disadvantage");

            response.EnsureSuccessStatusCode();
            var rollResponse = TestRollResponse.FromJson(await response.Content.ReadAsStringAsync());
            rollResponse.Rolls.Length.Should().Be(2);
            rollResponse.Rolls.Should().Contain(rollResponse.Result);
            foreach (var roll in rollResponse.Rolls)
            {
                rollResponse.Result.Should().BeLessOrEqualTo(roll);
            }
            rollResponse.RequestedRoll.Should().Be("1d20");
        }

        [Fact]
        public async Task RollWithUnknownRollTypeReturnsBadRequest()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"api/roll/1d20/blindfolded");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
