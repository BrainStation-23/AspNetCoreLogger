using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Logger.Loggers
{
    public static class LogOptionExtension
    {
        private static readonly List<string> ProviderType = new List<string> { "MSSql", "File", "CosmosDb", "MongoDb" };
        private static readonly List<string> LogType = new List<string> { "Sql", "Error", "Request", "Audit" };
        private static readonly List<string> Mode = new List<string> { "Min", "Full" };

        public static Tuple<bool, string> Valid(IConfiguration configuration)
        {
            StringBuilder sb = new StringBuilder();

            bool valid = false;
            var logOption = configuration.GetSection(LogOption.Name).Get<LogOption>();

            var logTypeValid = LogType.MustContain(logOption.LogType);
            if (logTypeValid == false) sb.Append($"Global Log type is not valid. Available Items - {LogType}, Current Items - {logOption.LogType}, ").Append(Environment.NewLine);

            var providerTypeValid = ProviderType.MustContain(logOption.ProviderType);
            if (providerTypeValid == false) sb.Append($"Provider type is not valid. Available Items - {ProviderType}, Current Items - {logOption.ProviderType}, ").Append(Environment.NewLine);

            var modeValid = Mode.Select(s => s.ToLower()).Contains(logOption.Mode.ToLower());
            if (modeValid == false) sb.Append($"Global Log type is not valid. Available Items - {Mode}, Current Items - {logOption.Mode}, ").Append(Environment.NewLine);

            var mssqlLogTypeValid = LogType.MustContain(logOption.Provider.MSSql.LogType);
            if (mssqlLogTypeValid == false) sb.Append($"MSSql Log type is not valid. Available Items - {LogType}, Current Items - {logOption.Provider.MSSql.LogType}, ").Append(Environment.NewLine);

            var cosmosDbLogTypeValid = LogType.MustContain(logOption.Provider.CosmosDb.LogType);
            if (cosmosDbLogTypeValid == false) sb.Append($"CosmosDb Log type is not valid. Available Items - {LogType}, Current Items - {logOption.Provider.CosmosDb.LogType}, ").Append(Environment.NewLine);

            var fileLogTypeValid = LogType.MustContain(logOption.Provider.File.LogType);
            if (fileLogTypeValid == false) sb.Append($"File Log type is not valid. Available Items - {LogType}, Current Items - {logOption.Provider.File.LogType}, ").Append(Environment.NewLine);

            var mongoLogTypeValid = LogType.MustContain(logOption.Provider.Mongo.LogType);
            if (mongoLogTypeValid == false) sb.Append($"Mongo Log type is not valid. Available Items - {LogType}, Current Items - {logOption.Provider.Mongo.LogType}, ").Append(Environment.NewLine);

            if (logTypeValid
                && providerTypeValid
                && modeValid
                && mssqlLogTypeValid
                && cosmosDbLogTypeValid
                && fileLogTypeValid
                && mongoLogTypeValid)
                valid = true;

            return Tuple.Create(valid, sb.ToString());
        }

        public static bool MustContain(this List<string> a, List<string> b)
        {
            return a.All(c => b.Select(s => s.ToLower()).Contains(c.ToLower()));
        }
    }
}
