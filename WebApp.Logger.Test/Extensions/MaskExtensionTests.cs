using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class MaskExtensionTests
    {
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("2", "******")]
        [DataRow("22", "******")]
        [DataRow("333", "3******3")]
        [DataRow("295200211111111111111111111111111", "295******111")]
        [DataRow("295200211111111111111111111111111295200211111111111111111111111111295200211111111111111111111111111", "295******111")]
        [DataRow("700327", "70******27")]
        [DataRow("70032773", "700******773")]
        [DataRow("70032763", "700******763")]
        [DataRow("70032762", "700******762")]
        public void MaskExtensionTests_MaskIt(string maskMe, string expected)
        {
            maskMe = maskMe.Trim();
            var masked = maskMe.MaskMe();

            Assert.IsNotNull(masked);
            Assert.AreEqual(expected, masked);
        }
    }
}
