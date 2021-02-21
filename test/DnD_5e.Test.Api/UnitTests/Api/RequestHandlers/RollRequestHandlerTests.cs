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
    public class RollRequestHandlerTests
    {
        [Fact]
        public async Task Gets_Result_From_Die_Roller()
        {
            var input = "1d20";
            var expected = 2;
            var mocker = new AutoMocker();
            mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, null)).ReturnsAsync(new RollResponse(input, expected));
            var target = mocker.CreateInstance<RollRequest.Handler>();
            var result = await target.Handle(new RollRequest(input), CancellationToken.None);

            result.Result.Should().Be(expected);
        }
    }
}
