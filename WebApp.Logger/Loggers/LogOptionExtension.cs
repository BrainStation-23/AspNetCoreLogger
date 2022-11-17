using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Loggers
{
    public static class LogOptionExtension
    {
        public static Tuple<bool, string> Valid(IConfiguration configuration)
        {
            List<string> sb = new List<string>();

            bool valid = false;
            var logOption = configuration.GetSection(LogOption.Name).Get<LogOption>();

            var logTypes = EnumExtension.EnumList<LogType>();
            var modes = EnumExtension.EnumList<Mode>();
            var providerTypes = EnumExtension.EnumList<ProviderType>();
            var httpVerbs = EnumExtension.EnumList<HttpVerb>();


            var logTypeValid = logTypes.MustContain(logOption.LogType);
            if (logTypeValid == false) sb.Append($"Global Log type is not valid. Available Items - {logTypes}, Current Items - {logOption.LogType}, ").Append(Environment.NewLine);

            var providerTypeValid = providerTypes.Select(s => s.ToLower()).Contains(logOption.ProviderType.ToLower());
            if (providerTypeValid == false) sb.Append($"Provider type is not valid. Available Items - {providerTypes}, Current Items - {logOption.ProviderType}, ").Append(Environment.NewLine);

            var modeValid = modes.Select(s => s.ToLower()).Contains(logOption.Mode.ToLower());
            if (modeValid == false) sb.Append($"Global Log type is not valid. Available Items - {modes}, Current Items - {logOption.Mode}, ").Append(Environment.NewLine);

            var isValidRetention = logOption.Retention.IsValidRetention();
            if (isValidRetention == false) sb.Append($"Global Retention is not valid. Current Items - {logOption.Retention}, ").Append(Environment.NewLine);

            var requestHttpVerb = httpVerbs.MustContain(logOption.Log.Request.HttpVerbs);
            if (requestHttpVerb == false) sb.Append($"Log.Request.HttpVerbs is not valid. Available Items - {httpVerbs}, Current Items - {logOption.Log.Request.HttpVerbs}, ").Append(Environment.NewLine);

            var errorHttpVerb = httpVerbs.MustContain(logOption.Log.Error.HttpVerbs);
            if (errorHttpVerb == false) sb.Append($"Log.Error.HttpVerbs is not valid. Available Items - {httpVerbs}, Current Items - {logOption.Log.Error.HttpVerbs}, ").Append(Environment.NewLine);

            var mssqlLogTypeValid = logTypes.MustContain(logOption.Provider.MSSql.LogType);
            if (mssqlLogTypeValid == false) sb.Append($"MSSql Log type is not valid. Available Items - {logTypes}, Current Items - {logOption.Provider.MSSql.LogType}, ").Append(Environment.NewLine);

            var cosmosDbLogTypeValid = logTypes.MustContain(logOption.Provider.CosmosDb.LogType);
            if (cosmosDbLogTypeValid == false) sb.Append($"CosmosDb Log type is not valid. Available Items - {logTypes}, Current Items - {logOption.Provider.CosmosDb.LogType}, ").Append(Environment.NewLine);

            var fileLogTypeValid = logTypes.MustContain(logOption.Provider.File.LogType);
            if (fileLogTypeValid == false) sb.Append($"File Log type is not valid. Available Items - {logTypes}, Current Items - {logOption.Provider.File.LogType}, ").Append(Environment.NewLine);

            var mongoLogTypeValid = logTypes.MustContain(logOption.Provider.Mongo.LogType);
            if (mongoLogTypeValid == false) sb.Append($"Mongo Log type is not valid. Available Items - {logTypes}, Current Items - {logOption.Provider.Mongo.LogType}, ").Append(Environment.NewLine);

            if (logTypeValid
                && providerTypeValid
                && modeValid
                && mssqlLogTypeValid
                && cosmosDbLogTypeValid
                && fileLogTypeValid
                && mongoLogTypeValid)
                valid = true;

            return Tuple.Create(valid, string.Join(", ", sb));
        }

        /// <summary>
        /// Log Options - Log - Request - HttpVerbs listed verbs will execute, not listed will be skip
        /// Log Options - Log - Request - Ignoreable columns list will be skip
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="logOptions">LogOption</param>
        /// <returns></returns>
        public static bool SkipRequest(HttpContext context, LogOption logOptions)
        {
            bool skip = false;

            if (logOptions.Log.Request.HttpVerbs.NotContains(context.Request.Method))
                return true;

            var url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            if (logOptions.Log.Request.IgnoreRequests.Any(r => url.Contains(r)))
                return true;

            return skip;
        }

        /// <summary>
        /// passing contain list items must be available in source list 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="contain"></param>
        /// <returns></returns>
        public static bool MustContain(this List<string> source, List<string> contain)
        {
            if (contain == null)
                return true;

            if ((source == null | source.Count == 0) && contain.Count > 0)
                return false;

            if (source.Count < contain.Count)
                return false;

            if (contain.Count == 0)
                return true;

            return source.All(c => contain.Select(s => s.ToLower()).Contains(c.ToLower()));
        }
    }
}
