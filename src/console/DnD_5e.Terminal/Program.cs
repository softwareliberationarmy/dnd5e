using System;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DnD_5e.Terminal
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, and welcome. This is D&D.");

            await using (var provider = Bootstrapper.RegisterServices())
            {
                var mediator = provider.GetRequiredService<IMediator>();

                bool result;
                do
                {
                    Console.Write("DnD >> ");
                    var input = Console.ReadLine();
                    result = await mediator.Send(new InputRequest(input));

                } while (result);
            }

            Console.WriteLine("Thank you for playing.");
        }

    }
}


