using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DXGame.Common.Persistence.MongoDB
{
    public static class Extensions
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var settings = c.GetService<IOptions<MongoDBSettings>>();
                return new MongoClient(settings.Value.ConnectionString);
            });
            services.AddScoped<IMongoDatabase>(c =>
            {
                var settings = c.GetService<IOptions<MongoDBSettings>>();
                var client = c.GetService<MongoClient>();
                return client.GetDatabase(settings.Value.Database);
            });
            ConventionRegistry.Register("DXGameConventions", new DXGameConventions(), _ => true);
        }

        private class DXGameConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions 
                => new List<IConvention>
                    {
                        new IgnoreExtraElementsConvention(true),
                        new EnumRepresentationConvention(BsonType.String),
                        new CamelCaseElementNameConvention(),
                    };
        }
    }
}