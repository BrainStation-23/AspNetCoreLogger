using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebApp.Logger.Extensions;
using WebApp.Logger.Test.Samples.NestedObject;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class ObjectExtensionTests
    {
        public MyClass MyClass { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            MyClass = new MyClass
            {
                Id = 10,
                Name = "Jamal",
                Phone = "010101818181",
                Address = new MyAddress
                {
                    HouseNo = "130/A",
                    RoadNo = "17",
                    Latitude = 10.36589M,
                    Longitude = 12.52689M,
                    City = new MyCity
                    {
                        Id = 1,
                        CityName = "Dhaka"
                    }
                },
                Profile = new MyProfile
                {
                    Id = 56,
                    AboutMe = "A good boy",
                    Fullname = "Jamal Uddin",
                    BirthDate = new DateTime(1982, 6, 12)
                }
            };
        }

        [TestMethod]
        public void ObjectExtensionTests_Iteterate()
        {
            var obj = MyClass;

            var newObj = obj.Iterate();

            Assert.IsNotNull(newObj);
            //Assert.AreEqual(newObj.Name, obj.Name);
        }
    }
}
