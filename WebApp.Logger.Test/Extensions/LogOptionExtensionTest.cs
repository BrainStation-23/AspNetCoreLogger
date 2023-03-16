using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class LogOptionExtensionTest
    {
        public LogOption logOption;
        public LogOption invalidLogOption;
        public Mock<HttpContext> mockHttpContext;
        public Mock<HttpRequest> mockHttpRequest;


        [TestInitialize]
        public void Inititalize()
        {
            mockHttpContext = new Mock<HttpContext>();
            mockHttpRequest = new Mock<HttpRequest>();
            logOption = new LogOption
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
                        HttpVerbs=new List<string> { "POST", "GET", "PUT", "PATCH", "DELETE" },
                        IgnoreRequests = new List<string> { "password", "token", "config", "/log" }
                    }
                }
            };
            invalidLogOption = new LogOption
            {
                LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                Provider = new Provider
                {
                    MSSql = new MSSql
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "",
                        Server = "",
                        Username = "",
                        Password = "",
                        Port = 0
                    },
                    Mongo = new Mongo
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "",
                        Server = "",
                        Port = 0,
                        Username = "",
                        Password = "",
                        DatabaseName = "",
                        DatabaseName1 = "",
                        ConnectionString = ""
                    },
                    File = new File
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "",
                        FileSize = "5MB",
                        Path = "MyLogs/Logs",
                        Filename = "dsfdsf"
                    },
                    CosmosDb = new CosmosDb
                    {
                        LogType = new List<string> { "Sql", "Error", "Request", "Audit" },
                        Retention = "",
                        AccountUrl = "",
                        Key = "",
                        DatabaseName = ""
                    }
                }
            };

        }

        [TestMethod]
        public void LogOptionExtentionTest_MustContain_ContainSame()
        {
            var source = new List<string> { "A", "B", "C" };
            var contain = new List<string> { "A", "B", "C" };

            var containSame = source.MustContain(contain);

            Assert.IsTrue(containSame);
        }

        [TestMethod]
        public void LogOptionExtentionTest_MustContain_NotContainAll()
        {
            var source = new List<string> { "A", "B", "C" };
            var contain = new List<string> { "A", "B", "C", "D" };

            var notContainAll = source.MustContain(contain);

            Assert.IsFalse(notContainAll);
        }

        [TestMethod]
        public void LogOptionExtentionTest_MustContain_ContainAll()
        {
            var source = new List<string> { "A", "B", "C" };
            var contain = new List<string> { "A", "B" };


            var containAll = source.MustContain(contain);

            Assert.IsTrue(containAll);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_MSSqlProvider_IsTrue()
        {
            logOption.ProviderType = "MSSql";

            var isMSSqlProvider = logOption.IsProviderConfigValid();

            Assert.IsTrue(isMSSqlProvider);

        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_MSSqlProvider_IsFalse()
        {
            invalidLogOption.ProviderType = "MSSql";

            var isMSSqlProvider = invalidLogOption.IsProviderConfigValid();

            Assert.IsFalse(isMSSqlProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_FileProvider_IsTrue()
        {
            logOption.ProviderType = "File";

            var isFileProvider = logOption.IsProviderConfigValid();

            Assert.IsTrue(isFileProvider);

        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_FileProvider_IsFalse()
        {
            invalidLogOption.ProviderType = "File";

            var isFileProvider = invalidLogOption.IsProviderConfigValid();

            Assert.IsFalse(isFileProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_CosmosDBProvider_IsTrue()
        {
            logOption.ProviderType = "MSSql";

            var isCosmosDbProvider = logOption.IsProviderConfigValid();

            Assert.IsTrue(isCosmosDbProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_CosmosDbProvider_IsFalse()
        {

            invalidLogOption.ProviderType = "CosmosDb";

            var isCosmosDbProvider = invalidLogOption.IsProviderConfigValid();

            Assert.IsFalse(isCosmosDbProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_MongoProvider_IsTrue()
        {

            logOption.ProviderType = "Mongo";

            var isMongoProvider = logOption.IsProviderConfigValid();

            Assert.IsTrue(isMongoProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_IsProviderConfigValid_MongoProvider_IsFalse()
        {
            invalidLogOption.ProviderType = "Mongo";

            var isMongoDbProvider = invalidLogOption.IsProviderConfigValid();

            Assert.IsFalse(isMongoDbProvider);
        }

        [TestMethod]
        public void LogOptionExtentionTest_SkipRequest_Valid()
        {
            mockHttpRequest.Setup(hr => hr.Method).Returns("GET");
            mockHttpRequest.Setup(hr => hr.Scheme).Returns("http");
            mockHttpRequest.Setup(hr => hr.Host).Returns(HostString.FromUriComponent("http://localhost:8089"));
            mockHttpRequest.Setup(hr => hr.PathBase).Returns(PathString.FromUriComponent("/api/log"));
            mockHttpContext.Setup(hc => hc.Request).Returns(mockHttpRequest.Object);

            var skipRequest = LogOptionExtension.SkipRequest(mockHttpContext.Object, logOption);

            Assert.IsTrue(skipRequest);
        }

        [TestMethod]
        public void LogOptionExtentionTest_SkipRequest_NotValid()
        {
            mockHttpRequest.Setup(hr => hr.Method).Returns("GET");
            mockHttpRequest.Setup(hr => hr.Scheme).Returns("http");
            mockHttpRequest.Setup(hr => hr.Host).Returns(HostString.FromUriComponent("http://localhost:8089"));
            mockHttpRequest.Setup(hr => hr.PathBase).Returns(PathString.FromUriComponent("/api/blog"));
            mockHttpContext.Setup(hc => hc.Request).Returns(mockHttpRequest.Object);

            var skipRequest = LogOptionExtension.SkipRequest(mockHttpContext.Object, logOption);

            Assert.IsFalse(skipRequest);
        }

    }
}
