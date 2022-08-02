using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Middlewares;

namespace WebApp.Core.Test.Middlewares
{
    [TestClass]
    public class HttpRequestMiddlewareTests
    {
        DefaultHttpContext defaultContext;
        Mock<ILogger<HttpRequestMiddleware>> mockLogger;
        Mock<IRouteLogRepository> mockRouteLogRepository;

        [TestInitialize]
        public void Initialize()
        {
            defaultContext = new DefaultHttpContext();
            mockLogger = new Mock<ILogger<HttpRequestMiddleware>>();
            mockRouteLogRepository = new Mock<IRouteLogRepository>();

            //mockRouteLogRepository.Setup(r => r.AddAsync(It.IsAny<RequestModel>()));
        }

        [TestMethod]
        public async Task HttpRequestMiddlewareTests_ReadRquestBody()
        {
            // Arrange
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

            // Act
            var middleware = new HttpRequestMiddleware(next: requestDelegate, logger: mockLogger.Object);
            await middleware.InvokeAsync(defaultContext, mockRouteLogRepository.Object);
            var requestBody = await defaultContext.Request.GetRequestBodyAsync();

            // Assert
            Assert.AreEqual(exptected, requestBody);
        }

        [TestMethod]
        public async Task ExceptionMiddlewareTests_ReadResponseBody()
        {
            // Arrange
            defaultContext.Response.Body = new MemoryStream();
            ApiResponse responseModel = new ApiResponse { StatusCode = (int)HttpStatusCode.OK, Data = new { Name = "Blog Post 1" } };
            var exptected = JsonSerializer.Serialize(responseModel);
            var requestDelegate = new RequestDelegate((innerHttpContext) =>
            {
                innerHttpContext.Response.WriteAsync(exptected);
                return Task.CompletedTask;
            });

            // Act
            var middleware = new HttpRequestMiddleware(next: requestDelegate, logger: mockLogger.Object);
            await middleware.InvokeAsync(defaultContext, mockRouteLogRepository.Object);
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
            var middleware = new HttpRequestMiddleware(next: requestDelegate, logger: mockLogger.Object);
            await middleware.InvokeAsync(defaultContext, mockRouteLogRepository.Object);

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
        public async Task HttpRequestMiddlewareTests_ReadMiddlewareData()
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
            //var middlewareInstance = new HttpRequestMiddleware(next: (innerHttpContext) =>
            //{
            //    innerHttpContext.Response.WriteAsync(expectedOutput);
            //    return Task.CompletedTask;
            //}, logger: mockLogger.Object);

            // Act
            var middleware = new HttpRequestMiddleware(next: requestDelegate, logger: mockLogger.Object);
            await middleware.InvokeAsync(defaultContext, mockRouteLogRepository.Object);

            var requestBody = await defaultContext.Request.GetRequestBodyAsync();
            var responseBody = await defaultContext.Response.GetResponseAsync();

            // Assert
            Assert.AreEqual(requestExptected, requestBody);
            Assert.AreEqual(responseExpected, responseBody);

            var responseData = JsonSerializer.Deserialize<ApiResponse>(responseBody);
            responseData.StatusCode.Should().Be((int)HttpStatusCode.OK);

            //// Assert 2: if the cookie is added to the response
            //var setCookieHeaders = defaultContext.Response.GetTypedHeaders().SetCookie;
            //var cookie = setCookieHeaders?.FirstOrDefault(x => x.Name == "DummyCookie");
            //Assert.IsTrue(Guid.TryParse(cookie.Value, out Guid result));
        }
    }
}
