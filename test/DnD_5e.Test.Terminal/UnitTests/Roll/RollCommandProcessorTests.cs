using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common.Interfaces;
using DnD_5e.Terminal.Common.IO;
using DnD_5e.Terminal.Roll;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Xunit;

namespace DnD_5e.Test.Terminal.UnitTests.Roll
{
    public class RollCommandProcessorTests: TestBase
    {
        [InlineData("roll    2d4+2")]
        [InlineData("ROLL  initiative")]
        [InlineData("Roll intelligence")]
        [Theory]
        public void Matches_ReturnsTrue_When_Roll_Request(string input)
        {
            var target = Mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeTrue();
        }

        [InlineData("Rool drool")]
        [InlineData("Roleplay fish")]
        [InlineData("LLOR backwards")]
        [Theory]
        public void Matches_ReturnsFalse_When_Not_Roll_Request(string input)
        {
            var target = Mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeFalse();
        }

        [Fact]
        public async Task Process_Calls_DndApi_For_RollResult()
        {
            var request = "1d20";
            var expectedRoll = 20;
            Mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll(request)).Returns(Task.FromResult(
                new RollResponse{ Result = expectedRoll}));

            var target = Mocker.CreateInstance<RollCommandProcessor>();
            await target.Process($"roll {request}");

            Mocker.Verify<IDndApi>(api => api.FreeRoll(request), Times.Once);
            Mocker.Verify<IOutputWriter>(w => w.WriteLine("20"));
        }

        [Fact]
        public async Task Process_Handles_Api_Exception()
        {
            var errorMessage = "An error occurred";
            Mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll(It.IsAny<string>()))
                .Throws(new ApiException(errorMessage));
            var target = Mocker.CreateInstance<RollCommandProcessor>();

            await target.Process("roll 3d6");

            Mocker.Verify<IOutputWriter>(writer => writer.WriteLine(errorMessage));
        }

        [Fact]
        public async Task Process_Shows_Http_Status_Code_If_Inner_Exception()
        {
            Mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll(It.IsAny<string>()))
                .Throws(new ApiException("Some error", new HttpRequestException("Something bad", null, HttpStatusCode.Gone)));
            var target = Mocker.CreateInstance<RollCommandProcessor>();

            await target.Process("roll 3d6");

            Mocker.Verify<IOutputWriter>(writer => writer.WriteLine(It.Is<string>(s => s.Contains("410"))));
        }
    }
}
