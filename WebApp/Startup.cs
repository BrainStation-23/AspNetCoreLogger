using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Linq;
using System.Reflection;
using WebApp.Core;
using WebApp.Core.Auths;
using WebApp.Helpers;
using WebApp.Logger.Hostings;
using WebApp.Logger.Loggers;
using WebApp.Logger.Middlewares;
using WebApp.Sql.Configurations;
using WebApp.Swaggers;

namespace WebApp
{
    public class Startup
    {
        readonly string WebAppCorsPolicy = "WebAppCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ApplicationHostedService>();

            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddControllers(
                //options =>
                //{
                //    options.Filters.Add<RouteFilterAttribute>();
                //}
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add<RouteFilterAttribute>();
            //});
            //services.AddScoped<RouteFilterAttribute>();
            services.AddDbContextDependencies(Configuration);
            services.AddWebAppDependency(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDapper();
            //services.AddHealthChecks();
            services.Configure<JwtOption>(Configuration.GetSection("Jwt"));
            services.AddHttpContextAccessor();
            services.ConfigureModelBindingExceptionHandling();
            //services.AddMongoDb(Configuration);
            //services.AddDatabaseDeveloperPageExceptionFilter();

            var origins = Configuration.GetSection("Domain").Get<Domain>();
            if (origins.Client2.Any()) { origins?.Client1?.AddRange(origins.Client2); }

            services.AddCors(options =>
            {
                options.AddPolicy(WebAppCorsPolicy,
                    builder => builder.WithOrigins(origins?.Client1?.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSwaggerExamples();
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            services.AddJwtTokenAuthentication(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetCoreLogger", Version = "v1" });
                c.SwaggerGenConfiguration();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetCoreLogger v1");
                });
            }
            else
            {
                //app.UseHsts();
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath}";
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });
            //app.UseResponseCaching();
            app.UseCors(WebAppCorsPolicy);
            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            //app.ExceptionLog();
            app.UseAuthorization();
            //app.UseSession();
            app.ExceptionLog();
            app.HttpLog();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}