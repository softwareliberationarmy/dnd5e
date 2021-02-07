#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using MediatR;

namespace DnD_5e.Terminal
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
        private static string[] _exitWords = new[] { "q", "quit", "exit" };

        public InputRequestHandler(ICommandProcessor[] processors)
        {
            _processors = processors;
        }

        public async Task<bool> Handle(InputRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine("User wrote {0}", request.Input);
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
