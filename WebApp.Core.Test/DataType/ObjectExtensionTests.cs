using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using WebApp.Common.DataType;
using WebApp.Core.Models;

namespace WebApp.Core.Test
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void ObjectExtensionTests_GetPropValue_Valid()
        {
            var user = new UserModel
            {
                Firstname = "My Firstname"
            };

            var actual = ObjectExtension.GetPropValue(user, "Firstname");

            actual.Should().NotBeNull();
            actual.Should().Be("My Firstname");

           
        }

        public void ObjectExtensionTests_ReadStream_Valid()
        {
            string test = "Testing 1-2-3";

            byte[] byteArray = Encoding.ASCII.GetBytes(test);
            MemoryStream stream = new MemoryStream(byteArray);

            var actual = stream.ReadStream();

            actual.Should().NotBeNull();
            actual.Should().Be("Testing 1-2-3");
        }
        }
}
