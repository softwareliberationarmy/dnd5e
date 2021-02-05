using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DnD_5e.Terminal
{
    public static class Bootstrapper
    {
        public static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services.BuildServiceProvider(true);
        }
    }
}