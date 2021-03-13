using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Pocos;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class GetCharactersByOwnerRequestHandlerTests: TestBase
    {
        [Fact]
        public async Task ShouldPassUserNameToRepository()
        {
            var expected = new List<CharacterListPoco>();
            Mocker.GetMock<ICharacterRepository>().Setup(repo => repo.GetByOwner("Fred")).Returns(expected);
            
            var target = Mocker.CreateInstance<GetCharactersByOwnerRequest.Handler>();
            var result = await target.Handle(new GetCharactersByOwnerRequest("Fred"), CancellationToken.None);

            result.Should().Equal(expected);
        }
    }
}
