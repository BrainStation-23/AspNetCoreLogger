using Azure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Test.Samples.NestedObject;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class ObjectExtensionTests
    {
        public MyClass MyClass { get; set; }

        public object NestedObject { get; set; }

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

        //[TestMethod]
        //public void ObjectExtensionTests_Iteterate()
        //{
        //    var obj = MyClass;

        //    var newObj = obj.Iterate();

        //    Assert.IsNotNull(newObj);
        //    //Assert.AreEqual(newObj.Name, obj.Name);
        //}

        [TestMethod]
        public void ObjectExtensionTests_ReadNestedObject_ErrorObject()
        {
            var errorObject = ObjectInitExtension.GetErrorObject();

            var errorData = errorObject.ReadNestedObject();
            var errorAsHash = errorData as Hashtable;

            Assert.IsNotNull(errorAsHash["host"]);
            Assert.AreEqual(errorAsHash["version"], "https");
        }

        [TestMethod]
        public void ObjectExtensionTests_ReadNestedObject_SqlObject()
        {
            var sqlObject = ObjectInitExtension.GetSqlObject();

            var sqlData = sqlObject.ReadNestedObject();
            var sqlAsHash = sqlData as Hashtable;

            Assert.IsNotNull(sqlAsHash["host"]);
            Assert.AreEqual(sqlAsHash["traceId"], "400001b9-0000-fc00-b63f-84710c7967bb");
        }

        [TestMethod]
        public void ObjectExtensionTests_ReadNestedObject_RequestObject()
        {
            var requestObject = ObjectInitExtension.GetRequestObject();

            var requestData = requestObject.ReadNestedObject();
            var request = requestData as Hashtable;

            Assert.IsNotNull(request["response"]);
            Assert.AreEqual(request["ipAddress"], "::1");
        }

        [TestMethod]
        public void ObjectExtensionTests_ReadNestedObject_AuditObject()
        {
            var auditObject = ObjectInitExtension.GetAuditObject();

            var auditData = auditObject.ReadNestedObject();
            var auditAsHash = auditData as Hashtable;

            Assert.IsNotNull(auditAsHash["type"]);
            Assert.AreEqual(auditAsHash["traceId"], "400001b9-0000-fc00-b63f-84710c7967bb");
        }
    }
}
