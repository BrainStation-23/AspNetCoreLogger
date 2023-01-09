using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Middlewares;
using WebApp.Logger.Models;

namespace WebApp.Core.Test.Middlewares
{
    [TestClass]
    public class ExceptionMiddlewareTests
    {
        DefaultHttpContext defaultContext;
        Mock<ILogger<ExceptionMiddleware>> mockLogger;
        Mock<IHostEnvironment> hostEnvironment;
        Mock<IExceptionLogRepository> mockExceptionLogRepository;
        Mock<IServiceProvider> serviceProvider;
        Mock<IOptions<LogOption>> logOption;

        [TestInitialize]
        public void Initialize()
        {
            defaultContext = new DefaultHttpContext();
            mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            hostEnvironment = new Mock<IHostEnvironment>();
            mockExceptionLogRepository = new Mock<IExceptionLogRepository>();
            serviceProvider = new Mock<IServiceProvider>();
            logOption = new Mock<IOptions<LogOption>>();
        }

        [TestMethod]
        public async Task ExceptionMiddlewareTests_ReadRquestBody()
        {
            //Arrange
            defaultContext.Response.Body = new MemoryStream();
            var data = new { Name = "My Blog Name", Title = "Hello word title!" };
            var exptected = JsonSerializer.Serialize(data);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(exptected));
            var requestDelegate = new RequestDelegate((httpContext) =>
            {
                httpContext.Request.Body = stream;
                httpContext.Request.ContentLength = stream.Length;
                return Task.CompletedTask;
            });

            //Act
           var middleware = new ExceptionMiddleware(next: requestDelegate, logger: mockLogger.Object, hostEnvironment.Object);
            await middleware.InvokeAsync(defaultContext, serviceProvider.Object);
            var requestBody = await defaultContext.Request.GetRequestBodyAsync();

            //Assert
            Assert.AreEqual(exptected, requestBody);
        }

        [TestMethod]
        public async Task ExceptionMiddlewareTests_ReadResponseBody()
        {
            // Arrange
            defaultContext.Response.Body = new MemoryStream();
            ErrorModel errorModel = new ErrorModel { StatusCode = HttpStatusCode.OK, Message = "Invalid data found" };
            var exptected = JsonSerializer.Serialize(errorModel);
            var requestDelegate = new RequestDelegate((innerHttpContext) =>
            {
                innerHttpContext.Response.WriteAsync(exptected);
                return Task.CompletedTask;
            });

            // Act
            var middleware = new ExceptionMiddleware(next: requestDelegate, logger: mockLogger.Object, hostEnvironment.Object);
            await middleware.InvokeAsync(defaultContext, serviceProvider.Object);
            var responeBody = await defaultContext.Response.GetResponseAsync();

            // Assert
            Assert.AreEqual(exptected, responeBody);
        }

        [TestMethod]
        public async Task HttpRequestMiddlewareTests_ReadMiddlewareDataAgain()
        {
            var request = new { Name = "My Blog Name", Title = "Hello word title!" };
            var requestExptected = JsonSerializer.Serialize(request);
            var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestExptected));
            var response = new ApiResponse { StatusCode = (int)HttpStatusCode.OK, Data = new { Name = "Blog Post 1" } };
            var responseExpected = JsonSerializer.Serialize(response);
            defaultContext.Response.Body = new MemoryStream();

            var requestDelegate = new RequestDelegate((httpContext) =>
            {
                httpContext.Request.Body = requestStream;
                httpContext.Request.ContentLength = requestStream.Length;

                httpContext.Response.WriteAsync(responseExpected);
                return Task.CompletedTask;
            });

            // Act
            var middleware = new ExceptionMiddleware(next: requestDelegate, logger: mockLogger.Object, hostEnvironment.Object);
            await middleware.InvokeAsync(defaultContext, serviceProvider.Object);

            var requestBody = await defaultContext.Request.GetRequestBodyAsync();
            var responseBody = await defaultContext.Response.GetResponseAsync();

            var requestBodyAgain = await defaultContext.Request.GetRequestBodyAsync();
            var responseBodyAgain = await defaultContext.Response.GetResponseAsync();

            // Assert
            Assert.AreEqual(requestExptected, requestBody);
            Assert.AreEqual(responseExpected, responseBody);
            Assert.AreEqual(requestExptected, requestBodyAgain);
            Assert.AreEqual(responseExpected, responseBodyAgain);
        }

        [TestMethod]
        public async Task ExceptionMiddlewareTests_ReadSqlExceptionError()
        {
            // Arrange
            var requestDelegate = new RequestDelegate((innerHttpContext) =>
            {
                string queryString = "EXECUTE NonExistantStoredProcedure";

                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=DotnetCoreLoggers;Trusted_Connection=True;"))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                return Task.CompletedTask;
            });
            defaultContext.Response.Body = new MemoryStream();

            // Act
            var middleware = new ExceptionMiddleware(next: requestDelegate, logger: mockLogger.Object, hostEnvironment.Object);
            await middleware.InvokeAsync(defaultContext, serviceProvider.Object);
            var responeBody = await defaultContext.Response.GetResponseAsync();
            var responseData = JsonSerializer.Deserialize<ErrorModel>(responeBody);

            // Assert
            var errors = responseData.Errors.ToList();
            errors.Should().NotBeEmpty();
            errors.Count.Should().BeGreaterThanOrEqualTo(1);
            errors.FirstOrDefault().Should().ToString().Contains("Microsoft.Data.SqlClient.SqlError");
        }
    }
}