using System;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using MediatR;

namespace DnD_5e.Api.RequestHandlers.Roll
{
    public class RollWithRequest: IRequest<RollResponse>
    {
        private string Request { get; }
        private string AdvantageType { get; }

        public RollWithRequest(string request, string advantageType)
        {
            Request = request;
            AdvantageType = advantageType;
        }

        public class Handler : IRequestHandler<RollWithRequest, RollResponse>
        {
            private readonly DieRoller _roller;

            public Handler(DieRoller roller)
            {
                _roller = roller;
            }

            public async Task<RollResponse> Handle(RollWithRequest request, CancellationToken cancellationToken)
            {
                switch (request.AdvantageType.ToLower())
                {
                    case "advantage":
                        return await _roller.Roll(request.Request, With.Advantage);
                    case "disadvantage":
                        return await _roller.Roll(request.Request, With.Disadvantage);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(request.AdvantageType));
                }
            }
        }
    }
}