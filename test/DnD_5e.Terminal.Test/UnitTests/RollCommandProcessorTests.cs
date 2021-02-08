using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using DnD_5e.Terminal.Roll;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Terminal.Test.UnitTests
{
    public class RollCommandProcessorTests
    {
        [InlineData("roll    2d4+2")]
        [InlineData("ROLL  initiative")]
        [InlineData("Roll intelligence")]
        [Theory]
        public void Matches_ReturnsTrue_When_Roll_Request(string input)
        {
            var mocker = new AutoMocker();
            var target = mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeTrue();
        }

        [InlineData("Rool drool")]
        [InlineData("Roleplay fish")]
        [InlineData("LLOR backwards")]
        [Theory]
        public void Matches_ReturnsFalse_When_Not_Roll_Request(string input)
        {
            var mocker = new AutoMocker();
            var target = mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeFalse();
        }

        [Fact]
        public async Task Process_Calls_DndApi_For_RollResult()
        {
            var expectedRoll = 20;
            var mocker = new AutoMocker();
            mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll("1d20")).Returns(Task.FromResult(
                new RollResponse{ Result = expectedRoll}));

            var target = mocker.CreateInstance<RollCommandProcessor>();
            await target.Process($"roll 1d20");

            mocker.Verify<IDndApi>(api => api.FreeRoll("1d20"), Times.Once);
        }
    }
}
