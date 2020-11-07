using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DnD_5e.Api.StartupServices
{
    public static class DatabaseStartup
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CharacterDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
