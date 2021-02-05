#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private static string[] _exitWords = new[] { "q", "quit", "exit" };

        public async Task<bool> Handle(InputRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine("User wrote {0}", request.Input);
            if (_exitWords.Contains(request.Input.Trim().ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
