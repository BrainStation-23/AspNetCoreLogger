using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.Middlewares;

namespace WebApp.Core.Test.Extensions
{
    [TestClass]
    public class HttpContextExtensionTests
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Executes once before the test run. (Optional)
        }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Executes once for the test class. (Optional)
        }

        [TestInitialize]
        public void Setup()
        {
            // Runs before each test. (Optional)
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Executes once after the test run. (Optional)
        }
        [ClassCleanup]

        public static void TestFixtureTearDown()
        {
            // Runs once after all tests in this class are executed. (Optional)
            // Not guaranteed that it executes instantly after all tests from the class.
        }
        [TestCleanup]

        public void TearDown()
        {
            // Runs after each test. (Optional)
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(0)]
        public void TestMethod_1(int value)
        {
            Assert.AreEqual(value, value);
        }

        [Ignore]
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public async Task HttpContextExtensionTests_GetBodyAsync()
        {
            //Arrange
            var data = "Hello World!!";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            // Act
            var requestBody = await httpContext.Request.GetRequestBodyAsync();

            // Assert
            Assert.AreEqual(data, requestBody);
        }

        [TestMethod]
        public async Task HttpContextExtensionTests_GetRequestBodyAsync_ReadAgain()
        {
            var data = "Hello World!!";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            var requestBody = await httpContext.Request.GetRequestBodyAsync();
            var requestBodyAgain = await httpContext.Request.GetRequestBodyAsync();


            Assert.AreEqual(data, requestBody);
            Assert.AreEqual(data, requestBodyAgain);
        }

        [TestMethod]
        public async Task HttpContextExtensionTests_GetReponseBodyAsync_ReadAgain()
        {
            var data = "Hello World!!";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data));

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = stream;
            httpContext.Response.ContentLength = stream.Length;

            var responseBody = await httpContext.Response.GetResponseAsync();
            var responseBodyAgain = await httpContext.Response.GetResponseAsync();


            Assert.AreEqual(data, responseBody);
            Assert.AreEqual(data, responseBodyAgain);
        }
    }
}
