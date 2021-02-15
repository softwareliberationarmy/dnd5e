using System;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using DnD_5e.Terminal.Common.Application;
using DnD_5e.Terminal.Common.DependencyInjection;
using DnD_5e.Terminal.Common.IO;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DnD_5e.Terminal
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, and welcome. This is D&D.");
            var host = Bootstrapper.BuildHost();

            using (var serviceScope = host.Services.CreateScope())
            {
                var outputWriter = serviceScope.ServiceProvider.GetRequiredService<IOutputWriter>();
                bool result;
                do
                {
                    Console.Write(outputWriter.Prefix);
                    var input = Console.ReadLine();
                    var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
                    result = await mediator.Send(new InputRequest(input));

                } while (result);
            }

            Console.WriteLine("Thank you for playing.");
        }

    }
}


