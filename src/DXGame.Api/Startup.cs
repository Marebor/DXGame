using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Infrastructure;
using DXGame.Api.Infrastructure.Abstract;
using DXGame.Common.Communication;
using DXGame.Common.Communication.RabbitMQ;
using DXGame.Common.Persistence.MongoDB;
using DXGame.ReadModel.Infrastructure;
using DXGame.ReadModel.Infrastructure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DXGame.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
            services.AddRabbitMQ(Configuration);
            services.AddMongoDB(Configuration);
            services.AddLogging();
            services.AddScoped<IActionResultHelper, ActionResultHelper>();
            services.AddScoped<IBroadcaster, SignalRBroadcaster>();
            services.AddScoped<IProjectionService, ProjectionService>();
            services.AddScoped<IProjectionRepository, MongoDBProjectionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSignalR(routes =>  
            {  
                routes.MapHub<DXGameHub>("/" + nameof(DXGameHub));  
            });

            loggerFactory.AddConsole();
        }
    }
}
