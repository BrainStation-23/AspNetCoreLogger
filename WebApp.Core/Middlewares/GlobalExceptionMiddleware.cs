using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace WebApp.Core.Middlewares
{
    public static class GlobalExceptionMiddleware
    {
        /// <summary>
        /// Handling all exception message globaly with builtin middleware
        /// startup.cs -> app.ConfigureExceptionHandler(logger);
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    await contextFeature.Error.ErrorAsync(context, logger);
                });
            });
        }
    }
}



