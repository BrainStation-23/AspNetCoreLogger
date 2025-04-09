using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers
{
    internal class TraceHelper
    {
        public async Task CollectTraces(HttpContext context)
        {
            StackTrace st = new StackTrace(true);
            List<TraceModel> models = new();

            int sequence = 0;
            for (int i = 0; i < st.FrameCount; i++)
            {

                StackFrame sf = st.GetFrame(i);
                var method = sf.GetMethod();
                var methodName = method.DeclaringType.Name;
                var assemblyName = method.DeclaringType.Assembly.GetName().Name;
                var namespaceName = method.DeclaringType.Namespace;
                var filename = sf.GetFileName();
                var line = sf.GetFileLineNumber();

                if (assemblyName == typeof(TraceHelper).Assembly.GetName().Name)
                    continue;

                if (line == 0)
                    continue;

                sequence++;
                var model = new TraceModel
                {
                    UserId = context.GetUserId(),
                    IpAddress = context.GetIpAddress(),
                    Url = context.GetUrl(),
                    Trace = $"{sequence} {filename} -> {line} -> {assemblyName}:{namespaceName}.{methodName}",
                    TraceId = context.TraceIdentifier,
                    Sequence = sequence
                };
                models.Add(model);
            }

            await BatchLoggingContext.PublishAsync(models, LogType.Trace.ToString());
        }
    }
}
