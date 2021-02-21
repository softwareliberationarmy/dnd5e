using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class RollWithRequestHandlerTests
    {
        [Fact]
        public async Task Rolls_With_Advantage()
        {
            var mocker = new AutoMocker();
            var input = "1d20";
            mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, With.Advantage))
                .ReturnsAsync(new RollResponse(input, With.Advantage, 10, 11));
            var target = mocker.CreateInstance<RollWithRequest.Handler>();
            var result = await target.Handle(new RollWithRequest(input, "advantage"), CancellationToken.None);

            result.Result.Should().Be(11);
        }

        [Fact]
        public async Task Rolls_With_Disadvantage()
        {
            var mocker = new AutoMocker();
            var input = "1d20";
            mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, With.Disadvantage))
                .ReturnsAsync(new RollResponse(input, With.Disadvantage, 10, 11));
            var target = mocker.CreateInstance<RollWithRequest.Handler>();
            var result = await target.Handle(new RollWithRequest(input, "disadvantage"), CancellationToken.None);

            result.Result.Should().Be(10);
        }

        [Fact]
        public async Task Throws_Argument_Exception_For_Invalid_AdvantageType()
        {
            var mocker = new AutoMocker();
            var target = mocker.CreateInstance<RollWithRequest.Handler>();
            await target.Invoking(h => h.Handle(new RollWithRequest("1d20", "capriciously"), CancellationToken.None))
                .Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
