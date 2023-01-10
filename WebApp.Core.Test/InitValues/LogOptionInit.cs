using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Core.Test.InitValues
{
    public static class LogOptionInit
    {
        public static LogOption GetValue()
        {
            var logOption = new LogOption
            {
                LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                Provider = new Provider
                {
                    MSSql = new MSSql
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "3d",
                        Server = "localhost",
                        Username = "sdfsdf",
                        Password = "dsfdsf",
                        Port = 2650
                    },
                    Mongo = new Mongo
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "3d",
                        Server = "localhost",
                        Port = 27017,
                        Username = "admin",
                        Password = "admin",
                        DatabaseName = "Logger",
                        DatabaseName1 = "Logger",
                        ConnectionString = "mongodb://admin:admin@localhost:27017"
                    },
                    File = new File
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "3d",
                        FileSize = "5MB",
                        Path = "MyLogs/Logs",
                        Filename = "dsfdsf"
                    },
                    CosmosDb = new CosmosDb
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "3d",
                        AccountUrl = "https://localhost:8081",
                        Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        DatabaseName = "LoggerWrapper"
                    }
                },
                Log = new Logs()
                {
                    Request = new Request
                    {
                        HttpVerbs = new List<string> { "POST", "GET", "PUT", "PATCH", "DELETE" },
                        IgnoreRequests = new List<string> { "password", "token", "config", "/log" }
                    }
                }
            };
            return logOption;
        }
    }
}
