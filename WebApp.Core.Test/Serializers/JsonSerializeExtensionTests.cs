using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using WebApp.Common.Files;
using WebApp.Logger.Extensions;

namespace WebApp.Core.Test.Serializers
{
    [TestClass]
    public class JsonSerializeExtensionTests
    {

        [TestMethod]
        public void JsonSerializeExtensionTests_1()
        {
            var escapeJsonPath = "./Jsons/escape-json.json";
            var escapeJsonString = escapeJsonPath.ReadFileAsText();

            var unescapeFilePath = "./Jsons/unescape-json.txt";
            var unescapeFileString = unescapeFilePath.ReadFileAsText();


            var actual = escapeJsonString.JsonUnescaping();

            Assert.AreEqual(actual, unescapeFileString);
        }

        [TestMethod]
        public void JsonSerializeExtensionTests_AuditJson()
        {
            var escapeJsonPath = "./Jsons/audit-escape-json.json";
            var escapeJsonString = escapeJsonPath.ReadFileAsText();

            var unescapeFilePath = "./Jsons/audit-unescape-json.txt";
            var unescapeFileString = unescapeFilePath.ReadFileAsText();


            var actual = escapeJsonString.JsonUnescaping();
            var parsed = JArray.Parse(actual);

            Assert.AreEqual(actual, unescapeFileString);
        }

        [TestMethod]
        public void JsonSerializeExtensionTests_RouteJson()
        {
            var escapeJsonPath = "./Jsons/route-escape-json.json";
            var escapeJsonString = escapeJsonPath.ReadFileAsText();

            var unescapeFilePath = "./Jsons/route-unescape-json.txt";
            var unescapeFileString = unescapeFilePath.ReadFileAsText();


            var actual = escapeJsonString.JsonUnescaping();
            var parsed = JArray.Parse(actual);

            Assert.AreEqual(actual, unescapeFileString);
        }

        [TestMethod]
        public void JsonSerializeExtensionTests_ErrorJson()
        {
            var escapeJsonPath = "./Jsons/error-escape-json.json";
            var escapeJsonString = escapeJsonPath.ReadFileAsText();

            var unescapeFilePath = "./Jsons/error-unescape-json.txt";
            var unescapeFileString = unescapeFilePath.ReadFileAsText();

            var actual = escapeJsonString.JsonUnescaping();
            var parsed = JArray.Parse(actual);

            Assert.AreEqual(actual, unescapeFileString);
        }
    }
}
