using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Common.Communication;
using DXGame.Common.Communication.RabbitMQ;
using DXGame.Common.DependencyInjection;
using DXGame.Common.Helpers;
using DXGame.Common.Persistence.MongoDB;
using DXGame.ReadModel.Infrastructure;
using DXGame.ReadModel.Infrastructure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DXGame.ReadModel
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
            services.AddRawRabbit(Configuration);
            services.AddMongoDB(Configuration);
            services.AddAssemblyMessageHandlers();
            services.AddLogging();
            services.AddScoped<IMessageBus, RabbitMQMessageBus>();
            services.AddScoped<IHandler, Handler>();
            services.AddScoped<IProjectionRepository, MongoDBProjectionRepository>();
            services.AddScoped<IProjectionService, ProjectionService>();
            services.AddScoped<AutoMapper.IMapper, AutoMapper.Mapper>();
            services.AddScoped<IMapper, Mapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            
            loggerFactory.AddConsole();
        }
    }
}
