#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DnD_5e.Terminal.Common.Application
{
    public class InputRequest : IRequest<bool>
    {
        public string Input { get; }

        public InputRequest(string? input)
        {
            Input = input ?? "";
        }
    }

    public class InputRequestHandler : IRequestHandler<InputRequest, bool>
    {
        private readonly ICommandProcessor[] _processors;
        private static readonly string[] _exitWords = new[] { "q", "quit", "exit" };

        public InputRequestHandler(IEnumerable<ICommandProcessor> processors)
        {
            _processors = processors.ToArray();
        }

        public async Task<bool> Handle(InputRequest request, CancellationToken cancellationToken)
        {
            if (_exitWords.Contains(request.Input.Trim().ToLower()))
            {
                return false;
            }

            var commandProcessor = _processors.FirstOrDefault(p => p.Matches(request.Input));
            if (commandProcessor != null)
            {
                await commandProcessor.Process(request.Input);
            }

            return true;
        }
    }
}
