using System;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class RollWithRequestHandlerTests: TestBase
    {
        [Fact]
        public async Task Rolls_With_Advantage()
        {
            var input = "1d20";
            Mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, With.Advantage))
                .ReturnsAsync(new RollResponse(input, With.Advantage, 10, 11));
            var target = Mocker.CreateInstance<RollWithRequest.Handler>();
            var result = await target.Handle(new RollWithRequest(input, "advantage"), CancellationToken.None);

            result.Result.Should().Be(11);
        }

        [Fact]
        public async Task Rolls_With_Disadvantage()
        {
            var input = "1d20";
            Mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, With.Disadvantage))
                .ReturnsAsync(new RollResponse(input, With.Disadvantage, 10, 11));
            var target = Mocker.CreateInstance<RollWithRequest.Handler>();
            var result = await target.Handle(new RollWithRequest(input, "disadvantage"), CancellationToken.None);

            result.Result.Should().Be(10);
        }

        [Fact]
        public async Task Throws_Argument_Exception_For_Invalid_AdvantageType()
        {
            var target = Mocker.CreateInstance<RollWithRequest.Handler>();
            await target.Invoking(h => h.Handle(new RollWithRequest("1d20", "capriciously"), CancellationToken.None))
                .Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
