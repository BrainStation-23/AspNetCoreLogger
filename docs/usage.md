# Dotnet logger wrapper usage

## prerequisit
- dotnet 5
- nuget
- visual studio/vs code
- mssql server

## usage
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


