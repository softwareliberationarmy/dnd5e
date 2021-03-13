using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DnD_5e.Api.Common;
using DnD_5e.Api.RequestHandlers;
using DnD_5e.Api.RequestHandlers.Characters;
using DnD_5e.Api.Services;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Xunit;

namespace DnD_5e.Test.Api.UnitTests.Api.RequestHandlers
{
    public class CharacterRollRequestHandlerTests: TestBase
    {
        [Fact]
        public async Task ShouldParseRequestAndRollForCharacter()
        {
            var characterId = 12;
            var rollType = "strength";
            var expected = new RollResponse("1d20+5", 17);
            Mocker.Use(new CharacterRollParser()); //no dependencies
            Mocker.GetMock<ICharacterRepository>().Setup(r => r.GetById(characterId))
                .Returns(Fixture.Create<Character>());
            Mocker.GetMock<DieRoller>().Setup(r => r.Roll(It.IsAny<string>(), null)).Returns(Task.FromResult(expected));

            var target = Mocker.CreateInstance<CharacterRoll.Handler>();
            var result = await target.Handle(new CharacterRoll.Request(characterId, rollType), CancellationToken.None);

            result.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCharacterNotFound()
        {
            var characterId = 12;
            Mocker.Use(new CharacterRollParser()); //no dependencies
            Mocker.GetMock<ICharacterRepository>().Setup(r => r.GetById(characterId))
                .Returns((Character)null);

            var target = Mocker.CreateInstance<CharacterRoll.Handler>();
            (await target.Invoking(
                    t => t.Handle(new CharacterRoll.Request(characterId, "strength"), CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>()).WithMessage("Character not found");
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenAbilityNotFound()
        {
            var characterId = 12;
            Mocker.Use(new CharacterRollParser()); //no dependencies
            Mocker.GetMock<ICharacterRepository>().Setup(r => r.GetById(characterId))
                .Returns(Fixture.Create<Character>());

            var target = Mocker.CreateInstance<CharacterRoll.Handler>();
            (await target.Invoking(
                    t => t.Handle(new CharacterRoll.Request(characterId, "parcheesi"), CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>()).WithMessage("Ability parcheesi not found");

        }

    }
}
