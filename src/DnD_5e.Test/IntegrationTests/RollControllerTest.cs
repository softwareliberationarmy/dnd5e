﻿using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace DnD_5e.Test.IntegrationTests
{

    public class RollControllerTest: IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        private readonly WebApplicationFactory<Api.Startup> _factory;

        public RollControllerTest(WebApplicationFactory<Api.Startup> factory)
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
    }
}
