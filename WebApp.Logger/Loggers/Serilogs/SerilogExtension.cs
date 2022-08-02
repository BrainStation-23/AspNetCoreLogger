using AspCoreLog.Loggers.Serilogs.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace WebApp.Logger.Loggers.Serilogs
{
    public static class SerilogExtension
    {
        public static ILogger CreateBootstrap()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
        }

        public static void SelfLog()
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));      // If the console is not available
                                                                                // Serilog.Debugging.SelfLog.Enable(Console.Error);
        }

        public static void Clear() => Log.CloseAndFlush();

        public static LoggerConfiguration UseEnricher(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Enrich.FromLogContext()                    // used for dynamically add/remove properties, using LogContext
                                                            //.Enrich.WithMachineName()                 // https://github.com/serilog/serilog-enrichers-environment
                                                            //.Enrich.WithProcessId()
                                                            //.Enrich.WithProcessName()                 // https://github.com/serilog/serilog-enrichers-process
                                                            //.Enrich.WithThreadId()
                                                            //.Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "MyDefault")
                                                            //.Enrich.WithThreadName()                  // https://github.com/serilog/serilog-enrichers-thread
                                                            //.Enrich.WithClientIp()
                                                            //.Enrich.WithClientAgent()                 // https://github.com/mo-esmp/serilog-enrichers-clientinfo
                                                            //.Enrich.WithExceptionDetails()            // https://github.com/RehanSaeed/Serilog.Exceptions
                                                            // https://github.com/serilog-web/classic
                                                            // .Enrich.WithSensitiveDataMasking()       // https://github.com/serilog-contrib/Serilog.Enrichers.Sensitive
                .Enrich.WithCorrelationId()                 // https://github.com/ekmsystems/serilog-enrichers-correlation-id; // services.AddHttpContextAccessor();
                .Enrich.With<SerilogContextEnricher>()
                .Enrich.With<RemovePropertiesEnricher>()
                .Enrich.With<UtcTimestampEnricher>()
                .Enrich.WithProperty("MyIdentifier", Guid.NewGuid());

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseConsole(this LoggerConfiguration loggerConfiguration)
        {
            //loggerConfiguration.WriteTo.Console();

            loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseDebug(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.Debug();

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseFile(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.File("Loggers/Serilogs/Log.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                fileSizeLimitBytes: null,                           // default limit is 1GB, null = unlimited
                rollingInterval: RollingInterval.Day,               //
                retainedFileCountLimit: null,                       // default limit is 31 files, null = unlimited
                rollOnFileSizeLimit: true,                            // if file size limit is reached, the file is rolled by log,txt, log_001.txt, log_002.txt etc
                buffered: true);                                    // not flush event after write

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseAsync(this LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.Async(f => f.File("Loggers/Serilogs/Log.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                fileSizeLimitBytes: null,                           // default limit is 1GB, null = unlimited
                rollingInterval: RollingInterval.Day,               //
                retainedFileCountLimit: null,                       // default limit is 31 files, null = unlimited
                rollOnFileSizeLimit: true,                            // if file size limit is reached, the file is rolled by log,txt, log_001.txt, log_002.txt etc
                buffered: true));                                    // not flush event after write

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseSeq(this LoggerConfiguration loggerConfiguration)
        {
            //var path = "http://localhost:5341";
            //loggerConfiguration.WriteTo.Seq(path, apiKey: "hTm23m3s89nsT6ptHfxx");

            return loggerConfiguration;
        }

        public static LoggerConfiguration UseMsSql(this LoggerConfiguration loggerConfiguration, HostBuilderContext hostBuilderContext)
        {
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.AdditionalColumns = new Collection<SqlColumn> {
                                new SqlColumn {ColumnName = "ApplicationName", DataType = SqlDbType.VarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "MyIdentifier", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestUrl", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestType", DataType = SqlDbType.Int, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestBody", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestResponse", DataType = SqlDbType.Text, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestDate", DataType = SqlDbType.DateTime, AllowNull = true}
            };
            columnOptions.Properties.ExcludeAdditionalProperties = false;
            columnOptions.LogEvent.ExcludeAdditionalProperties = false;

            loggerConfiguration.WriteTo.MSSqlServer(connectionString: hostBuilderContext.Configuration.GetConnectionString("DefaultConnection"),
                sinkOptions: new SinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true },
                columnOptions: new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn> {
                                new SqlColumn {ColumnName = "ApplicationName", DataType = SqlDbType.VarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "MyIdentifier", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestUrl", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestType", DataType = SqlDbType.Int, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestBody", DataType = SqlDbType.NVarChar, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestResponse", DataType = SqlDbType.Text, AllowNull = true},
                                new SqlColumn {ColumnName = "RequestDate", DataType = SqlDbType.DateTime, AllowNull = true}
                        }
                });

            return loggerConfiguration;
        }

        public static IDisposable BeginPropertyScope(this Microsoft.Extensions.Logging.ILogger logger, params ValueTuple<string, object>[] properties)
        {
            var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);
            return logger.BeginScope(dictionary);
        }
    }
}
