using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebApp.Common.Serialize;
using WebApp.Core.Files;

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
    }
}
