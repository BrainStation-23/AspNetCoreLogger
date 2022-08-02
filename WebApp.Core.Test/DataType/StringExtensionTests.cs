using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebApp.Common.DataType;

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

        [TestMethod]
        public void StringExtensionTests_ToNullString_True()
        {
            var actual = StringExtension.ToNullString(null);

            Assert.AreEqual(string.Empty, actual);
        }

        [TestMethod]
        public void StringExtensionTests_ToShorten_Valid()
        {
            var str = nameof(ArgumentNullException);

            var actual = StringExtension.ToShorten(str);

            Assert.AreEqual("ANE", actual);
        }
    }
}
