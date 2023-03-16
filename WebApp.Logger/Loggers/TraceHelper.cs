using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
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
                var method = sf.GetMethod().DeclaringType.Name;
                var filename = sf.GetFileName();
                var line = sf.GetFileLineNumber();

                if (line == 0)
                    continue;

                sequence++;
                var model = new TraceModel
                {
                    UserId = context.GetUserId(),
                    IpAddress = context.GetIpAddress(),
                    Url = context.GetUrl(),
                    Trace = $"{sequence} {filename} -> {line} -> {method}",
                    TraceId = context.TraceIdentifier
                };
                models.Add(model);
            }

            await BatchLoggingContext.PublishAsync(models, LogType.Trace.ToString());
        }
    }
}
