using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.RequestHandlers.Roll;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class RollRequestHandlerTests: TestBase
    {
        [Fact]
        public async Task Gets_Result_From_Die_Roller()
        {
            var input = "1d20";
            var expected = 2;
            Mocker.GetMock<DieRoller>().Setup(r => r.Roll(input, null)).ReturnsAsync(new RollResponse(input, expected));
            var target = Mocker.CreateInstance<RollRequest.Handler>();
            var result = await target.Handle(new RollRequest(input), CancellationToken.None);

            result.Result.Should().Be(expected);
        }
    }
}
