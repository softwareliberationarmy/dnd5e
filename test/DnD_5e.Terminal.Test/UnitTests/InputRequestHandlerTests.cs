using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace DnD_5e.Terminal.Test.UnitTests
{
    public class InputRequestHandlerTests
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
            var mocker = new AutoMocker();
            var target = mocker.CreateInstance<InputRequestHandler>();
            var result = await target.Handle(new InputRequest(entry), CancellationToken.None);

            result.Should().BeFalse();
        }
    }
}
