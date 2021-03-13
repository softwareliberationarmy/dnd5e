using System;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.Common;
using DnD_5e.Api.Services;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using MediatR;

namespace DnD_5e.Api.RequestHandlers.Characters
{
    public static class CharacterSavingThrow
    {
        public class Request: IRequest<RollResponse>
        {
            public int CharacterId { get; }
            public string Ability { get; }

            public Request(int characterId, string ability)
            {
                CharacterId = characterId;
                Ability = ability;
            }
        }

        public class Handler: IRequestHandler<Request, RollResponse>
        {
            private readonly CharacterRollParser _rollParser;
            private readonly ICharacterRepository _repository;
            private readonly DieRoller _roller;

            public Handler(CharacterRollParser rollParser, ICharacterRepository repository, DieRoller roller)
            {
                _rollParser = rollParser;
                _repository = repository;
                _roller = roller;
            }

            public async Task<RollResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var req = _rollParser.ParseRequest(request.Ability, isSave: true);
                    var character = _repository.GetById(request.CharacterId);

                    if (character == null)
                    {
                        throw new NotFoundException("Character not found");
                    }
                    var roll = character.GetRoll(req);
                    return await _roller.Roll(roll);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw  new NotFoundException($"Ability {request.Ability} not found");
                }
            }
        }
    }
}