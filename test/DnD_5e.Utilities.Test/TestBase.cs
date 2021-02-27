using AutoFixture;
using Moq.AutoMock;

namespace DnD_5e.Utilities.Test
{
    public class TestBase
    {
        protected AutoMocker Mocker { get; } = new AutoMocker();
        protected Fixture Fixture { get; } = new Fixture();
    }
}
