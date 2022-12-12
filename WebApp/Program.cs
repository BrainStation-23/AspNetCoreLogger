using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using WebApp.Logger.Loggers.Serilogs;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine("msg"));
            Log.Logger = SerilogExtension.CreateBootstrap();

            try
            {
                Log.Information("Starting Web Application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("```````````````````````````````````````````````````");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("```````````````````````````````````````````````````");
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                SerilogExtension.Clear();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(),
                        writeToProviders: true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
