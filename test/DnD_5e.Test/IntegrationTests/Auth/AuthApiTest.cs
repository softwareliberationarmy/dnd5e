using System;
using System.Text.Json;
using System.Threading.Tasks;
using DnD_5e.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.IntegrationTests.Auth
{
    public class AuthApiTest: IDisposable
    {
        private readonly TestClientFactory _factory;

        public AuthApiTest()
        {
            _factory = new TestClientFactory();
        }

        [Fact]
        public async Task GetReturnsConfigValues()
        {
            _factory.ConfigurationInfo.Clear();
            _factory.ConfigurationInfo.Add("Auth:Domain", "domain");
            _factory.ConfigurationInfo.Add("Auth:ClientId", "clientid");
            _factory.ConfigurationInfo.Add("Auth:Audience", "audience");
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/auth");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var values = (ValuesResponseObject)JsonSerializer.Deserialize(responseString, typeof(ValuesResponseObject),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            values.Domain.Should().Be("domain");
            values.ClientId.Should().Be("clientid");
            values.Audience.Should().Be("audience");
        }

        [Fact]
        public async Task StillReturnsResultEvenWhenNoConfigValues()
        {
            _factory.ConfigurationInfo.Clear();
            var client = _factory.CreateClient();

            var response = await client.GetAsync("api/auth");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var values = (ValuesResponseObject) JsonSerializer.Deserialize(responseString, typeof(ValuesResponseObject),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            values.Domain.Should().BeNullOrEmpty();
            values.ClientId.Should().BeNullOrEmpty();
            values.Audience.Should().BeNullOrEmpty();
        }

        private class ValuesResponseObject
        {
            public string Domain { get; set; }
            public string ClientId { get; set; }
            public string Audience { get; set; }
        }

        public void Dispose()
        {
            _factory?.Dispose();
        }
    }
}
