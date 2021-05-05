using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using DnD_5e.Api.Security;
using DnD_5e.Api.Services;
using DnD_5e.Api.StartupServices;
using DnD_5e.Domain.DiceRolls;
using DnD_5e.Infrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DnD_5e.Api
{
    public class Startup
    {
        private readonly string LocalCors = "AllowLocalhostCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Auth0Settings>(Configuration.GetSection(Auth0Settings.ConfigSection));
            services.AddCors(options =>
            {
                options.AddPolicy(LocalCors,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000");
                        builder.AllowAnyHeader();
                    });
            });
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<DieRoller>();
            services.AddSingleton<CharacterRollParser>();
            services.AddScoped<ICharacterRepository,CharacterRepository>();
            services.AddDbContext<CharacterDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("Auth0Settings:Authority");
                options.Audience = Configuration.GetValue<string>("Auth0Settings:Audience");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "D&D 5e API",
                    Description = "An API exploring the D&D domain",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kerry Patrick",
                        Email = "themanfromsql at gmail"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License Type: GPL-3.0",
                        Url = new Uri("https://opensource.org/licenses/GPL-3.0"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DnD 5e API, v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(LocalCors);
            app.UseAuthorization();

            if (env.EnvironmentName != "Testing")
            {
                app.UpdateDatabase();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
