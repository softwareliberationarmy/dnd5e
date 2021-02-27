using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common.Application;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Test.Terminal.UnitTests.Common.Application
{
    public class InputRequestHandlerTests: TestBase
    {
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
            var target = Mocker.CreateInstance<InputRequestHandler>();
            var result = await target.Handle(new InputRequest(entry), CancellationToken.None);

            result.Should().BeFalse();
        }

        [InlineData("Roll 2d4+2")]
        [InlineData("Something else")]
        [InlineData("Exit++")]
        [Theory]
        public async Task ReturnsTrueWhenNotAnExitWord(string entry)
        {
            var target = Mocker.CreateInstance<InputRequestHandler>();
            var result = await target.Handle(new InputRequest(entry), CancellationToken.None);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DelegatesInputToFirstMatchingCommandProcessor()
        {
            string expectedCommand = "roll 2d4";
            string actualCommand = null;
            var processor = Mock.Of<ICommandProcessor>(p =>
                p.Matches(It.IsAny<string>()) == true 
            );
            Mock.Get(processor).Setup(p => p.Process(It.IsAny<string>()))
                .Callback((string s) => { actualCommand = s; });
            Mocker.Use(typeof(IEnumerable<ICommandProcessor>), new[] { processor });
            var target = Mocker.CreateInstance<InputRequestHandler>();

            await target.Handle(new InputRequest(expectedCommand), CancellationToken.None);

            actualCommand.Should().Be(expectedCommand);
        }
    }

}
