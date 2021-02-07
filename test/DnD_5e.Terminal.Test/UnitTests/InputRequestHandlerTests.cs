using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Terminal.Test.UnitTests
{
    public class InputRequestHandlerTests
    {
        private AutoMocker _mocker;

        public InputRequestHandlerTests()
        {
            _mocker = new AutoMocker();
        }

        [InlineData("q")]
        [InlineData("Quit")]
        [InlineData("quit")]
        [InlineData("QUIT")]
        [InlineData("Q")]
        [InlineData("exit")]
        [InlineData("EXIT")]
        [InlineData("Exit")]
        [Theory]
        public async Task ReturnsFalseWhenUserEntersExitWord(string entry)
        {
            var target = _mocker.CreateInstance<InputRequestHandler>();
            var result = await target.Handle(new InputRequest(entry), CancellationToken.None);

            result.Should().BeFalse();
        }

        [InlineData("Roll 2d4+2")]
        [InlineData("Something else")]
        [InlineData("Exit++")]
        [Theory]
        public async Task ReturnsTrueWhenNotAnExitWord(string entry)
        {
            var target = _mocker.CreateInstance<InputRequestHandler>();
            var result = await target.Handle(new InputRequest(entry), CancellationToken.None);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DelegatesInputToFirstCommandProcessor()
        {
            string expectedCommand = "roll 2d4";
            string actualCommand = null;
            var processor = Mock.Of<ICommandProcessor>(p =>
                p.Matches(It.IsAny<string>()) == true 
            );
            Mock.Get(processor).Setup(p => p.Process(It.IsAny<string>()))
                .Callback((string s) => { actualCommand = s; });
            _mocker.Use(typeof(ICommandProcessor[]), new[] { processor });
            var target = _mocker.CreateInstance<InputRequestHandler>();

            await target.Handle(new InputRequest(expectedCommand), CancellationToken.None);

            actualCommand.Should().Be(expectedCommand);
        }
    }

}
