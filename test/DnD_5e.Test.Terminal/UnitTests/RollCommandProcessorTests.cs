﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using DnD_5e.Terminal.Common.Interfaces;
using DnD_5e.Terminal.Common.IO;
using DnD_5e.Terminal.Roll;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Terminal.Test.UnitTests
{
    public class RollCommandProcessorTests
    {
        private AutoMocker _mocker;

        public RollCommandProcessorTests()
        {
            _mocker = new AutoMocker();
        }

        [InlineData("roll    2d4+2")]
        [InlineData("ROLL  initiative")]
        [InlineData("Roll intelligence")]
        [Theory]
        public void Matches_ReturnsTrue_When_Roll_Request(string input)
        {
            var target = _mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeTrue();
        }

        [InlineData("Rool drool")]
        [InlineData("Roleplay fish")]
        [InlineData("LLOR backwards")]
        [Theory]
        public void Matches_ReturnsFalse_When_Not_Roll_Request(string input)
        {
            var target = _mocker.CreateInstance<RollCommandProcessor>();
            target.Matches(input).Should().BeFalse();
        }

        [Fact]
        public async Task Process_Calls_DndApi_For_RollResult()
        {
            var request = "1d20";
            var expectedRoll = 20;
            _mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll(request)).Returns(Task.FromResult(
                new RollResponse{ Result = expectedRoll}));

            var target = _mocker.CreateInstance<RollCommandProcessor>();
            await target.Process($"roll {request}");

            _mocker.Verify<IDndApi>(api => api.FreeRoll(request), Times.Once);
            _mocker.Verify<IOutputWriter>(w => w.WriteLine("20"));
        }

        [Fact]
        public async Task Process_Handles_Api_Exception()
        {
            var errorMessage = "An error occurred";
            _mocker.GetMock<IDndApi>().Setup(api => api.FreeRoll(It.IsAny<string>()))
                .Throws(new ApiException(errorMessage));
            var target = _mocker.CreateInstance<RollCommandProcessor>();

            await target.Process("roll 3d6");

            _mocker.Verify<IOutputWriter>(writer => writer.WriteLine(errorMessage));
        }
    }
}
