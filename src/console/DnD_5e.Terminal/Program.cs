using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DnD_5e.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, and welcome. This is D&D.");

            await using (var provider = RegisterServices())
            {
                var mediator = provider.GetService<IMediator>();

                bool result = true;
                do
                {
                    Console.Write("DnD >> ");
                    var input = Console.ReadLine();
                    result = await mediator.Send(new InputRequest(input));

                } while (result);
            }

            Console.WriteLine("Thank you for playing.");
        }

        private static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services.BuildServiceProvider(true);
        }
    }

    public class InputRequest: IRequest<bool>
    {
        public string Input { get; }

        public InputRequest(string? input)
        {
            Input = input ?? "";
        }
    }

    public class InputRequestHandler : IRequestHandler<InputRequest,bool>
    {
        private static string[] _exitWords = new[] {"q", "quit", "exit"};

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


