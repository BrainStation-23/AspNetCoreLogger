using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Core.DataType;

namespace WebApp.Core.Test
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void StringExtensionTests_IsNotNullOrEmpty_True()
        {
            var nonEmptyString = "string";

            var actual = StringExtension.IsNotNullOrEmpty(nonEmptyString);

            Assert.AreEqual(true, actual);
        }
    }
}
