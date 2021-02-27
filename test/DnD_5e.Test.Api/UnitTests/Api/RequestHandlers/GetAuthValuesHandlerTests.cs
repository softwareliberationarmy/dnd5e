using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.Security;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class GetAuthValuesHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_Options_Values()
        {
            var mocker = new AutoMocker();
            var fixture = new Fixture();
            var settings = fixture.Create<Auth0Settings>();
            mocker.GetMock<IOptions<Auth0Settings>>().SetupGet(s => s.Value).Returns(settings);

            var target = mocker.CreateInstance<GetAuthValues.Handler>();
            var result = await target.Handle(new GetAuthValues.Request(), CancellationToken.None);

            result.Domain.Should().Be(settings.Domain);
            result.ClientId.Should().Be(settings.ClientId);
            result.Audience.Should().Be(settings.Audience);
        }
    }
}
