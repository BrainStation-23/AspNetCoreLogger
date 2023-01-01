using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

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
            if (logTypeValid == false) sb.Add($"Global Log type is not valid. Available Items - {logTypes.PrintList()} Current Items - {logOption.LogType.PrintList()} ");

            var providerTypeValid = providerTypes.Select(s => s.ToLower()).Contains(logOption.ProviderType.ToLower());
            if (providerTypeValid == false) sb.Add($"Provider type is not valid. Available Items - {providerTypes.PrintList()} Current Items - {logOption.ProviderType}, ");

            var modeValid = modes.Select(s => s.ToLower()).Contains(logOption.Mode.ToLower());
            if (modeValid == false) sb.Add($"Global Log type is not valid. Available Items - {modes.PrintList()} Current Items - {logOption.Mode}, ");

            var isValidRetention = logOption.Retention.IsValidRetention();
            if (isValidRetention == false) sb.Add($"Global Retention is not valid. Current Items - {logOption.Retention}, ");

            var requestHttpVerbValid = httpVerbs.MustContain(logOption.Log.Request.HttpVerbs);
            if (requestHttpVerbValid == false) sb.Add($"Log.Request.HttpVerbs is not valid. Available Items - {httpVerbs.PrintList()} Current Items - {logOption.Log.Request.HttpVerbs.PrintList()} ");

            var requestModeValid = modes.Select(s => s.ToLower()).Contains(logOption.Log.Request.Mode.ToLower());
            if (requestModeValid == false) sb.Add($"Request mode type is not valid. Available Items - {modes.PrintList()} Current Items - {logOption.Log.Request.Mode}, ");

            var sqlModeValid = modes.Select(s => s.ToLower()).Contains(logOption.Log.Sql.Mode.ToLower());
            if (sqlModeValid == false) sb.Add($"Sql mode type is not valid. Available Items - {modes.PrintList()} Current Items - {logOption.Log.Sql.Mode}, ");

            var errorHttpVerbValid = httpVerbs.MustContain(logOption.Log.Error.HttpVerbs);
            if (errorHttpVerbValid == false) sb.Add($"Log.Error.HttpVerbs is not valid. Available Items - {httpVerbs.PrintList()} Current Items - {logOption.Log.Error.HttpVerbs.PrintList()}");

            var mssqlLogTypeValid = logTypes.MustContain(logOption.Provider.MSSql.LogType);
            if (mssqlLogTypeValid == false) sb.Add($"MSSql Log type is not valid. Available Items - {logTypes.PrintList()} Current Items - {logOption.Provider.MSSql.LogType.PrintList()} ");

            var cosmosDbLogTypeValid = logTypes.MustContain(logOption.Provider.CosmosDb.LogType);
            if (cosmosDbLogTypeValid == false) sb.Add($"CosmosDb Log type is not valid. Available Items - {logTypes.PrintList()}Current Items - {logOption.Provider.CosmosDb.LogType.PrintList()} ");

            var fileLogTypeValid = logTypes.MustContain(logOption.Provider.File.LogType);
            if (fileLogTypeValid == false) sb.Add($"File Log type is not valid. Available Items - {logTypes.PrintList()} Current Items - {logOption.Provider.File.LogType.PrintList()} ");

            var mongoLogTypeValid = logTypes.MustContain(logOption.Provider.Mongo.LogType);
            if (mongoLogTypeValid == false) sb.Add($"Mongo Log type is not valid. Available Items - {logTypes.PrintList()} Current Items - {logOption.Provider.Mongo.LogType.PrintList()} ");

            var providerIsValid = logOption.IsProviderConfigValid();
            if (providerIsValid == false) sb.Add($"ProviderType is {logOption.ProviderType}. But {logOption.ProviderType} configuration is not valid., ");

            if (logTypeValid
                && providerTypeValid
                && modeValid
                && mssqlLogTypeValid
                && cosmosDbLogTypeValid
                && fileLogTypeValid
                && mongoLogTypeValid
                && providerIsValid
                && sqlModeValid
                && requestHttpVerbValid
                && requestModeValid
                && errorHttpVerbValid)
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

            if (logOptions.IgnoreHttpVerbs.MustContain(context.Request.Method))
                return true;

            var url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            if (logOptions.Log.Request.IgnoreRequests.Any(r => url.ToLower().Contains(r.ToLower())))
                return true;

            var ignoreEndPoints = logOptions.IgnoreEndPoints ?? new List<string> { };
            if (ignoreEndPoints.Any(r => url.ToLower().Contains(r.ToLower())))
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

            if (source == null)
                return false;

            if ((source.Count == 0) && contain.Count > 0)
                return false;

            if (source.Count < contain.Count)
                return false;

            if (contain.Count == 0)
                return true;

            return contain.All(c => source.Select(s => s.ToLower()).Contains(c.ToLower()));
        }
        /// <summary>
        /// passing contain string must be available in source list
        /// </summary>
        /// <param name="source"></param>
        /// <param name="contain"></param>
        /// <returns></returns>
        public static bool MustContain(this List<string> source, string contain)
        {
            if (contain == null)
                return true;

            if (source == null)
                return false;

            if (source.Count == 0)
                return false;

            return source.Select(s => s.ToLower()).Contains(contain.ToLower());
        }
        public static string PrintList<T>(this List<T> list)
        {
            string outPutString = "";
            foreach (var a in list) { outPutString += a.ToString() + ","; }
            return outPutString;
        }

        /// <summary>
        ///check validity of given provider type in our app configuration
        /// </summary>
        /// <param name="logOption"></param>
        /// <returns></returns>
        public static bool IsProviderConfigValid(this LogOption logOption)
        {
            if (logOption.ProviderType == "MSSql")
            {
                var mssql = logOption.Provider.MSSql;
                var validLogType = logOption.LogType.MustContain(mssql.LogType);
                if (!validLogType || mssql.Retention.IsNullOrEmpty() || mssql.Server.IsNullOrEmpty() || mssql.Username.IsNullOrEmpty() || mssql.Password.IsNullOrEmpty() || mssql.Port <= 0) return false;
                return true;
            }
            else if (logOption.ProviderType == "CosmosDb")
            {
                var cmdb = logOption.Provider.CosmosDb;
                var validLogType = logOption.LogType.MustContain(cmdb.LogType);
                if (!validLogType || cmdb.Retention.IsNullOrEmpty() || cmdb.AccountUrl.IsNullOrEmpty() || cmdb.Key.IsNullOrEmpty() || cmdb.DatabaseName.IsNullOrEmpty()) return false;
                return true;
            }
            else if (logOption.ProviderType == "File")
            {
                var file = logOption.Provider.File;
                var validLogType = logOption.LogType.MustContain(file.LogType);
                if (!validLogType || file.Retention.IsNullOrEmpty()) return false;
                return true;
            }
            else if (logOption.ProviderType == "Mongo")
            {
                var mongo = logOption.Provider.Mongo;
                var validLogType = logOption.LogType.MustContain(mongo.LogType);
                if (!validLogType || mongo.Retention.IsNullOrEmpty() || mongo.Port <= 0 || mongo.Username.IsNullOrEmpty() || mongo.Password.IsNullOrEmpty() || mongo.Server.IsNullOrEmpty() || mongo.DatabaseName.IsNullOrEmpty() || mongo.DatabaseName1.IsNullOrEmpty() || mongo.ConnectionString.IsNullOrEmpty()) return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SkipErrorLog(ErrorModel errorModel, LogOption logOptions)
        {
            bool skip = false;

            if (errorModel.Url.Contains("/Log/", StringComparison.InvariantCultureIgnoreCase))
                skip = true;

            if (!logOptions.LogType.MustContain(LogType.Error.ToString()))
                skip = true;

            return skip;
        }

        public static ErrorModel PrepareErrorModel(this ErrorModel errorModel, LogOption logOptions)
        {
            var errorLogOptions = logOptions.Log.Error;

            var ignoreColumns = errorLogOptions.IgnoreColumns.ToList(errorLogOptions.EnableIgnore);
            var maskColumns = errorLogOptions.MaskColumns.ToList(errorLogOptions.EnableMask);

            errorModel = errorModel.ToFilter<ErrorModel>(ignoreColumns.ToArray(), maskColumns.ToArray());

            return errorModel;
        }
        public static bool SkipRequestLog(RequestModel requestModel, LogOption logOptions)
        {
            bool skip = false;

            if (requestModel.Url.Contains("/Log/", StringComparison.InvariantCultureIgnoreCase))
                skip = true;

            if (!logOptions.LogType.MustContain(LogType.Request.ToString()))
                skip = true;

            return skip;
        }
        public static RequestModel PrepareRequestModel(this RequestModel requestModel, LogOption logOptions)
        {
            var requestLogOptions = logOptions.Log.Request;

            var ignoreColumns = requestLogOptions.IgnoreColumns.ToList(requestLogOptions.EnableIgnore);
            var maskColumns = requestLogOptions.MaskColumns.ToList(requestLogOptions.EnableMask);
            requestModel = requestModel.ToFilter<RequestModel>(ignoreColumns.ToArray(), maskColumns.ToArray());

            return requestModel;
        }

        public static bool SkipSqlLog(SqlModel sqlModel, LogOption logOptions)
        {
            bool skip = false;

            if (sqlModel.Url.Contains("/Log/", StringComparison.InvariantCultureIgnoreCase))
                skip = true;

            if (!logOptions.LogType.MustContain(LogType.Sql.ToString()))
                skip = true;

            return skip;
        }

        public static SqlModel PrepareSqlModel(this SqlModel sqlModel, LogOption logOptions)
        {
            var sqlLogOptions = logOptions.Log.Sql;

            var ignoreColumns = sqlLogOptions.IgnoreColumns.ToList(sqlLogOptions.EnableIgnore);
            var maskColumns = sqlLogOptions.MaskColumns.ToList(sqlLogOptions.EnableMask);
            sqlModel = sqlModel.ToFilter<SqlModel>(ignoreColumns.ToArray(), maskColumns.ToArray());

            return sqlModel;
        }

        public static List<AuditModel> PrepareAuditModel(this List<AuditModel> auditModels, LogOption logOptions)
        {
            if (logOptions.LogType.MustContain("Audit") != true)
                return new List<AuditModel> { };

            var auditLogOption = logOptions.Log.Audit;

            var ignoreColumns = auditLogOption.IgnoreColumns.ToList(auditLogOption.EnableIgnore);
            var maskColumns = auditLogOption.MaskColumns.ToList(auditLogOption.EnableMask);
            var ignoreTables = auditLogOption.IgnoreTables.ToList(auditLogOption.EnableIgnoreTable);
            var ignoreShcemaNames = auditLogOption.IgnoreSchemas.ToList(auditLogOption.EnableIgnoreSchema);

            auditModels.Remove(auditModels.FirstOrDefault(s => ignoreShcemaNames.MustContain(s.SchemaName)));

            auditModels.Remove(auditModels.FirstOrDefault(s => ignoreTables.MustContain(s.TableName)));

            auditModels.ForEach(m =>
            {
                if (ignoreShcemaNames.MustContain(m.SchemaName))
                    m = null;

                if (ignoreTables.MustContain(m.TableName))
                    m = null;

                if (m != null)
                {
                    m.OldValues = m.OldValues.ToFilter(ignoreColumns.ToArray(), maskColumns.ToArray());
                    m.NewValues = m.NewValues.ToFilter(ignoreColumns.ToArray(), maskColumns.ToArray());
                }
            });

            return auditModels;
        }

        public static List<AuditModel> SerializeAuditModel(this List<AuditModel> auditModels)
        {
            auditModels.ForEach(auditModel =>
            {
                auditModel.PrimaryKey = auditModel.PrimaryKey == null ? null : JsonConvert.SerializeObject(auditModel.PrimaryKey);
                auditModel.OldValues = auditModel.OldValues == null ? null : JsonConvert.SerializeObject(auditModel.OldValues);
                auditModel.NewValues = auditModel.NewValues == null ? null : JsonConvert.SerializeObject(auditModel.NewValues);
                auditModel.AffectedColumns = auditModel.AffectedColumns == null ? null : JsonConvert.SerializeObject(auditModel.AffectedColumns);
            });

            return auditModels;
        }

        public static SqlModel SerializeSqlModel(this SqlModel sqlModel)
        {
            sqlModel.Event = sqlModel.Event == null ? null : JsonConvert.SerializeObject(sqlModel.Event);
            sqlModel.Connection = sqlModel.Connection == null ? null : JsonConvert.SerializeObject(sqlModel.Connection);
            sqlModel.Command = sqlModel.Command == null ? null : JsonConvert.SerializeObject(sqlModel.Command);

            return sqlModel;
        }
        public static RequestModel DeserializeRequestModel(this RequestModel requestModel)
        {
            requestModel.Body = requestModel.Body == null ? null : JsonConvert.DeserializeObject<object>(requestModel.Body.ToString());
            requestModel.Response = requestModel.Response == null ? null : JsonConvert.DeserializeObject<object>(requestModel.Response.ToString());
            requestModel.RequestHeaders = requestModel.RequestHeaders == null ? null : JsonConvert.DeserializeObject<object>(requestModel.RequestHeaders.ToString());
            requestModel.ResponseHeaders = requestModel.ResponseHeaders == null ? null : JsonConvert.DeserializeObject<object>(requestModel.ResponseHeaders.ToString());
            return requestModel;
        }

        public static ErrorModel DeserializeErrorModel(this ErrorModel errorModel)
        {
            errorModel.Body = errorModel.Body == null ? null : JsonConvert.DeserializeObject<object>(errorModel.Body.ToString());
            errorModel.Response = errorModel.Response == null ? null : JsonConvert.DeserializeObject<object>(errorModel.Response.ToString());
            errorModel.RequestHeaders = errorModel.RequestHeaders == null ? null : JsonConvert.DeserializeObject<object>(errorModel.RequestHeaders.ToString());
            errorModel.ResponseHeaders = errorModel.ResponseHeaders == null ? null : JsonConvert.DeserializeObject<object>(errorModel.ResponseHeaders.ToString());
            return errorModel;
        }

        public static AuditModel PrepareAuditModel(this AuditModel auditModel, LogOption logOptions)
        {
            if (logOptions.LogType.MustContain("Audit") != true)
                return null;

            var auditLogOption = logOptions.Log.Audit;

            var ignoreColumns = auditLogOption.IgnoreColumns.ToList(auditLogOption.EnableIgnore);
            var maskColumns = auditLogOption.MaskColumns.ToList(auditLogOption.EnableMask);
            var ignoreTables = auditLogOption.IgnoreTables.ToList(auditLogOption.EnableIgnoreTable);
            var ignoreShcemaNames = auditLogOption.IgnoreSchemas.ToList(auditLogOption.EnableIgnoreSchema);

            if (ignoreShcemaNames.MustContain(auditModel.SchemaName))
                return null;

            if (ignoreTables.MustContain(auditModel.TableName))
                return null;

            var oldValues = auditModel.OldValues.ToFilter(ignoreColumns.ToArray(), maskColumns.ToArray());
            var newValues = auditModel.NewValues.ToFilter(ignoreColumns.ToArray(), maskColumns.ToArray());

            //auditModel.OldValues = JsonConvert.SerializeObject(oldValues);
            //auditModel.NewValues = JsonConvert.SerializeObject(newValues);

            return auditModel;
        }

        public static List<string> ToList(this List<string> list, bool enableFlag)
        {
            list = enableFlag ? (list ?? new List<string> { }) : new List<string> { };

            return list;
        }
    }
}
