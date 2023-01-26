using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Providers.Files;
using WebApp.Logger.Loggers.Providers;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Loggers.Providers.Mongos;

namespace WebApp.Logger.Providers.Mongos.Configurations
{
    public static class MongoDependency
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            //var logOption1 = new LogOption();
            //configuration.Bind(LogOption.Name, logOption1);

            //var logOption = configuration.GetSection(LogOption.Name).Get<LogOption>();
            //services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            //services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));


            services.AddScoped<IExceptionLogRepository, MongoExceptionLogRepository>();
            services.AddScoped<IRouteLogRepository, MongoRouteLogRepository>();
            services.AddScoped<IAuditLogRepository, MongoAuditLogRepository>();
            services.AddScoped<ISqlLogRepository, MongoSqlLogRepository>();
            //services.AddScoped<IDashboardRepository, MongoDashboardRepository>();
        }
    }

    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }

    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }

    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }

    [BsonCollection("people")]
    public class Person : Document
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
