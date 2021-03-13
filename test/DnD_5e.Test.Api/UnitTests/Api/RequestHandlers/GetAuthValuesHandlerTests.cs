using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.Security;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class GetAuthValuesHandlerTests: TestBase
    {
        [Fact]
        public async Task Handle_Returns_Options_Values()
        {
            var settings = Fixture.Create<Auth0Settings>();
            Mocker.GetMock<IOptions<Auth0Settings>>().SetupGet(s => s.Value).Returns(settings);

            var target = Mocker.CreateInstance<GetAuthValuesRequest.Handler>();
            var result = await target.Handle(new GetAuthValuesRequest(), CancellationToken.None);

            result.Domain.Should().Be(settings.Domain);
            result.ClientId.Should().Be(settings.ClientId);
            result.Audience.Should().Be(settings.Audience);
        }
    }
}
