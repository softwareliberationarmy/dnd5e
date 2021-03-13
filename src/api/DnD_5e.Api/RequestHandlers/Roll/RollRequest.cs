using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;
using DnD_5e.Domain.DiceRolls;
using MediatR;

// ReSharper disable ClassNeverInstantiated.Global

namespace DnD_5e.Api.RequestHandlers.Roll
{
    public class RollRequest: IRequest<RollResponse>
    {
        private string Input { get; }

        public RollRequest(string input)
        {
            Input = input;
        }

        public class Handler: IRequestHandler<RollRequest, RollResponse>
        {
            private readonly DieRoller _roller;

            public Handler(DieRoller roller)
            {
                _roller = roller;
            }

            public async Task<RollResponse> Handle(RollRequest request, CancellationToken cancellationToken)
            {
                return await _roller.Roll(request.Input);
            }
        }
    }
}