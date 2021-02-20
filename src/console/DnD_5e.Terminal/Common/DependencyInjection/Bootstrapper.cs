using System;
using System.Reflection;
using DnD_5e.Terminal.Common.Application;
using DnD_5e.Terminal.Common.Interfaces;
using DnD_5e.Terminal.Common.IO;
using DnD_5e.Terminal.Roll;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DnD_5e.Terminal.Common.DependencyInjection
{
    public static class Bootstrapper
    {
        public static IHost BuildHost()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    services.AddTransient<ICommandProcessor, RollCommandProcessor>();
                    services.AddTransient<IDndApi, DndApi>();
                    services.AddSingleton<IOutputWriter,ConsoleOutputWriter>();
                    services.AddHttpClient<DndApi>();
                }).UseConsoleLifetime();

            var host = builder.Build();
            return host;
        }
    }
}