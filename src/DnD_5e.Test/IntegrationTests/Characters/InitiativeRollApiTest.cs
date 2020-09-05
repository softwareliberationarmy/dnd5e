﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.Characters
{
    public class InitiativeRollApiTest: IClassFixture<TestClientFactory>
    {
        private readonly TestClientFactory _clientFactory;

        public InitiativeRollApiTest(TestClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Fact]
        public async Task Initiative_roll_uses_character_dexterity()
        {
            await _clientFactory.SetupCharacters(new CharacterEntity
            {
                Id = 1, Dexterity = 14
            });

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync("api/characters/1/roll/initiative");

            var expectedModifier = 2;
            var minReturnValue = 1 + expectedModifier;
            var maxReturnValue = 20 + expectedModifier;

            response.EnsureSuccessStatusCode();
            var roll = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            roll.Should().BeInRange(minReturnValue, maxReturnValue, "Expected initiative roll to use player's dexterity");
        }
    }
}
