using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;

namespace AspCoreLog.Loggers.Serilogs.Enrichers
{
    public class SerilogEnricher
    {

    }

    public class SerilogContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.RemovePropertyIfPresent("RequestId");
        }
    }

    public class RemovePropertiesEnricher : ILogEventEnricher
    {
        /// <summary>
        /// usage:  .Enrich.With(new RemovePropertiesEnricher())
        ///         .Enrich.With<RemovePropertiesEnricher>()
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="logEventPropertyFactory"></param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory logEventPropertyFactory)
        {
            logEvent.RemovePropertyIfPresent("SourceContext");
            logEvent.RemovePropertyIfPresent("RequestId");
            logEvent.RemovePropertyIfPresent("RequestPath");

            logEvent.RemovePropertyIfPresent("ActionId");
            logEvent.RemovePropertyIfPresent("ActionName");
            logEvent.RemovePropertyIfPresent("CorrelationId");
        }
    }

    /// <summary>
    /// usage: .Enrich.With(new ThreadIdEnricher())
    /// </summary>
    public class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }

    public class UtcTimestampEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
        }
    }

    public static class SerilogEnrichExtensions
    {
        public static LoggerConfiguration WithUtcTimestamp(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<UtcTimestampEnricher>();
        }
    }
}



