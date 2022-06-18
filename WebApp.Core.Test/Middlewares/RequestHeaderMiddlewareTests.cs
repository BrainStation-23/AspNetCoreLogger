using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.Middlewares;

namespace WebApp.Core.Test.Middlewares
{
    [TestClass]
    public class RequestHeaderMiddlewareTests
    {
        DefaultHttpContext defaultContext;
        Mock<ILogger<HttpRequestHeaderMiddleware>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            defaultContext = new DefaultHttpContext();
            mockLogger = new Mock<ILogger<HttpRequestHeaderMiddleware>>();
        }

        [TestMethod]
        public async Task RequestHeaderMiddlewareTests_GetResponseAsync()
        {
            const string expectedOutput = "Request handed over to next request delegate";

            // Arrange
            //DefaultHttpContext defaultContext = new DefaultHttpContext();
            defaultContext.Response.Body = new MemoryStream();
            defaultContext.Request.Path = "/";

            var requestDelegate = new RequestDelegate((innerHttpContext) =>
            {
                innerHttpContext.Response.WriteAsync(expectedOutput);
                return Task.CompletedTask;
            });

            // Act
            var middlewareInstance = new HttpRequestHeaderMiddleware(next: requestDelegate, logger: mockLogger.Object);

            await middlewareInstance.Invoke(defaultContext);

            // Assert
            var responeBody = await defaultContext.Response.GetResponseAsync();
            Assert.AreEqual(expectedOutput, responeBody);
        }
    }
}