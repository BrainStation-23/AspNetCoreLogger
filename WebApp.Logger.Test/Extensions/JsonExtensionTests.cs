using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class JsonExtensionTests
    {
        [TestMethod]
        public void JsonExtension_ReadJson()
        {
            var path = "./Samples/JsonRemoveKey.json";

            var json = JsonExtension.ReadJson(path);

            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void JsonExtension_ReadJosnProperties()
        {
            var path = "./Samples/JsonRemoveKey.json";
            var json = JsonExtension.ReadJson(path);
            var removeProperties = new string[] { "Id", "Authorization", "createdDateUtc" };

            var skippedJson = JsonExtension.SkipIt(json, removeProperties);


            Assert.IsNotNull(skippedJson);
        }

        [TestMethod]
        public void JsonExtension_MaskJosnProperties()
        {
            var path = "./Samples/JsonRemoveKey1.json";
            var json = JsonExtension.ReadJson(path);
            var maskProperties = new string[] { "Authorization", "createdDateUtc" };

            JsonExtension.MaskIt(json, maskProperties);


            Assert.IsNotNull(json);
        }
    }
}
