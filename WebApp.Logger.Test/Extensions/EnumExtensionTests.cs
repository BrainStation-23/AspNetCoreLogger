using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class EnumExtensionTests
    {
        [TestMethod]
        public void EnumExtension_ToProviderTypeEnums()
        {
            var enums = new List<string> { "fiyyle", "MSuuSql", "cosiimos" };

            var providerTypes = enums.ToProviderTypeEnums();

            Assert.IsNotNull(providerTypes);
        }
    }
}
