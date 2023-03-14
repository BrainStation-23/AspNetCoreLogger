using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using WebApp.Core;
using WebApp.Core.Auths;
using WebApp.Logger.Hostings;
using WebApp.Logger.Loggers;
using WebApp.Logger.Middlewares;
using WebApp.Sql.Configurations;
using WebApp7.Helpers;
using WebApp7.Swaggers;

var builder = WebApplication.CreateBuilder(args);

string WebAppCorsPolicy = "WebAppCorsPolicy";

var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .CreateLogger();

builder.Host.UseSerilog(logger);

IConfiguration Configuration = builder.Configuration;
IWebHostEnvironment webHostEnvironment = builder.Environment;

// Add services to the container.
builder.Services.AddHostedService<ApplicationHostedService>();


builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContextDependencies(Configuration);
builder.Services.AddWebAppDependency(Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddLogDependency(Configuration);
builder.Services.Configure<JwtOption>(Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureModelBindingExceptionHandling();

var origins = Configuration.GetSection("Domain").Get<Domain>();
if (origins.Client2.Any()) { origins?.Client1?.AddRange(origins.Client2); }

builder.Services.AddCors(options =>
{
    options.AddPolicy(WebAppCorsPolicy,
        builder => builder.WithOrigins(origins?.Client1?.ToArray())
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


builder.Services.AddSwaggerExamples();
builder.Services.AddLogConfig(Configuration);
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddJwtTokenAuthentication(Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetCoreLogger", Version = "v1" });
    c.SwaggerGenConfiguration();
});
var app = builder.Build();

if (webHostEnvironment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
app.UseSerilogRequestLogging();
app.UseCors(WebAppCorsPolicy);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionLog();
app.UseHttpLog();
app.UseSession();

IApplicationBuilder applicationBuilder = app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
