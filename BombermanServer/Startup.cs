using BombermanServer.Configurations;
using BombermanServer.Mediator;
using BombermanServer.Services;
using BombermanServer.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BombermanServer
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.Configure<MapConfiguration>(Configuration.GetSection("AppSettings"));

            services.AddSingleton<IPlayerService, PlayerService>();
            services.AddSingleton<IBombService, BombService>();
            services.AddSingleton<IMapService, MapService>();
            // services.AddSingleton<IMapService, MapGeneratorAdapter>();
            services.AddSingleton<IEnemyMovementService, EnemyMovementService>();
            services.AddSingleton<IPlayerDeathMediator, PlayerDeathMediator>();
            services.AddSingleton<IRespawnService, RespawnService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.ConfigureHubs(); // Implementation in Configurations / HubConfiguration.cs
            app.ApplicationServices.GetService<IEnemyMovementService>();
        }
    }
}
