# Dotnet logger wrapper usage

## prerequisit
- dotnet 5
- nuget
- visual studio/vs code
- mssql server

## Run Application
- install prerequisits
- migrate database 
  - changes connection string `WebApp\appsettings.json`
  - basic commands for project db migration
```
-> dotnet tool install --global dotnet-ef
-> dotnet tool update --global dotnet-ef
-> dotnet ef

-> cd WebApp.Sql
-> dotnet add package Microsoft.EntityFrameworkCore.Design

-> cd WebApp
-> dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.2

-> cd <Solution-Directory>
-> dotnet ef migrations add InitialCreate --project WebApp.Sql --startup-project WebApp (optional)
-> dotnet ef database update --project WebApp.Sql --startup-project WebApp
```
- build & run application

## Usages
- All methods need to implement in your `WebApp\Startup.cs` files
- Before that you must have WebApp.Core library or you must add in your new project
- All wrapper methods are calling from `WebApp.Core\Loggers\LoggerExtension.cs`

- Audit Log
```cs
public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    AuditTrailLog();
}
```
- Request Log
```cs
 public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ....
    app.HttpLog();
    ...
}
```
- Error Log
```cs
 public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ....
    app.UseExceptionLog();
    ...
}
```
- DB Queries log
- Notification Log
- Activity Log



