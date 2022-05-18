using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace DotnetCoreApplicationBoilerplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));      // If the console is not available
            //                                                                    // Serilog.Debugging.SelfLog.Enable(Console.Error);
            //SerilogExtension.SelfLog();
            //Log.Logger = SerilogExtension.CreateBootstrap();

            try
            {
                //Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                //SerilogExtension.Clear();
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
