using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;
using WebApp.Logger.Test.InitValues;

namespace WebApp.Logger.Test.Extensions
{
    [TestClass]
    public class FileExtensionTests
    {
        [TestMethod]
        public void FileExtensionTests_Paging_Integers()
        {
            //Arrange
            List<int> testList = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            List<int> expectedList = new() { 13, 14, 15, 16 };

            //Act
            var actualList = testList.Paging(3, 4).ToList();

            var enumerable1 = expectedList.AsEnumerable();
            var enumerable2 = actualList.AsEnumerable();

            var isEqual = enumerable1.SequenceEqual(enumerable2);

            //Assert
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void FileExtensionTests_Paging_Objects()
        {
            //Arrange
            object obj1 = new { id = 1, name = "object1" };
            object obj2 = new { id = 2, name = "object2" };
            object obj3 = new { id = 3, name = "object3" };
            object obj4 = new { id = 4, name = "object4" };
            object obj5 = new { id = 5, name = "object5" };
            object obj6 = new { id = 6, name = "object6" };
            object obj7 = new { id = 7, name = "object7" };
            object obj8 = new { id = 8, name = "object8" };

            List<object> testList = new() { obj1, obj2, obj3, obj4, obj5, obj6, obj7, obj8 };
            List<object> expectedList = new() { obj7, obj8 };

            //Act
            List<object> actualList = testList.Paging(3, 2).ToList();

            var enumerable1 = expectedList.AsEnumerable();
            var enumerable2 = actualList.AsEnumerable();

            var isEqual = enumerable1.SequenceEqual(enumerable2);

            //Assert
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void FileExtensionTests_GetLogType()
        {
            //Arrange
            object audit = new AuditModel();
            object sql = new SqlModel();
            object error = new ErrorModel();
            object request = new RequestModel();

            object expectedA = LogType.Audit.ToString().ToLower();
            object expectedS = LogType.Sql.ToString().ToLower();
            object expectedE = LogType.Error.ToString().ToLower();
            object expectedR = LogType.Request.ToString().ToLower();

            //Act
            object actualA = audit.GetLogType();
            object actualS = sql.GetLogType();
            object actualE = error.GetLogType();
            object actualR = request.GetLogType();

            //Assert
            Assert.AreEqual(expectedA, actualA);
            Assert.AreEqual(expectedS, actualS);
            Assert.AreEqual(expectedE, actualE);
            Assert.AreEqual(expectedR, actualR);
        }

        [TestMethod]
        public void FileExtensionTests_ToLogObjects()
        {
            //Arrange
            string SqlJsonString = SqlObjectInit.GetSqlObjectStringWithHeaderAndFooter();
            string AuditJsonString = AuditObjectInit.GetAuditObjectStringWithHeaderAndFooter();
            string ErrorJsonString = ErrorObjectInit.GetErrorObjectStringWithHeaderAndFooter();
            string RequestJsonString = RequestObjectInit.GetRequestObjectStringWithHeaderAndFooter();

            //Act
            List<object> sqlLogs = SqlJsonString.ToLogObjects();
            List<object> auditLogs = AuditJsonString.ToLogObjects();
            List<object> errorLogs = ErrorJsonString.ToLogObjects();
            List<object> requestLogs = RequestJsonString.ToLogObjects();

            //Assert
            Assert.AreEqual(2, sqlLogs.Count);
            Assert.AreEqual(2, auditLogs.Count);
            Assert.AreEqual(2, errorLogs.Count);
            Assert.AreEqual(1, requestLogs.Count);

            Assert.IsInstanceOfType(sqlLogs, typeof(List<object>));
            Assert.IsInstanceOfType(auditLogs, typeof(List<object>));
            Assert.IsInstanceOfType(errorLogs, typeof(List<object>));
            Assert.IsInstanceOfType(requestLogs, typeof(List<object>));
        }

        [TestMethod]
        public void FileExtensionTests_PrepareMessageForJSONFormat()
        {
            //Arrange
            List<SqlModel> sqlModels = SqlObjectInit.GetDemoSqlModels();
            List<AuditModel> auditModels = AuditObjectInit.GetDemoAuditModels();
            List<ErrorModel> errorModels = ErrorObjectInit.GetDemoErrorModels();
            List<RequestModel> requestModels = RequestObjectInit.GetDemoRequestModels();

            //Act
            string sqlString = FileExtension.PrepareMessageForJSONFormat(sqlModels);
            string auditString = FileExtension.PrepareMessageForJSONFormat(auditModels);
            string errorString = FileExtension.PrepareMessageForJSONFormat(errorModels);
            string requestString = FileExtension.PrepareMessageForJSONFormat(requestModels);

            //Assert
            Assert.IsNotNull(sqlString);
            Assert.IsNotNull(auditString);
            Assert.IsNotNull(errorString);
            Assert.IsNotNull(requestString);

            Assert.IsTrue(sqlString.Contains(FileExtension.FooterAppender()));
            Assert.IsTrue(auditString.Contains(FileExtension.FooterAppender()));
            Assert.IsTrue(errorString.Contains(FileExtension.FooterAppender()));
            Assert.IsTrue(requestString.Contains(FileExtension.FooterAppender()));
        }

        [TestMethod]
        public void FileExtensionTests_GetNewFileName()
        {
            //Arrange
            string oldFileName = "request_log_json_20230117_1.txt";
            string expectedNewName = "request_log_json_20230117_2.txt";

            //Act
            string actualNewName = FileExtension.GetNewFileName(oldFileName);

            //Assert
            Assert.AreEqual(expectedNewName, actualNewName);
        }

        [TestMethod]
        public void FileExtensionTests_AddLine()
        {
            //Arrange
            string mainString = "DemoLines";
            string newLines = "NewLines";
            string expectedNewString = "DemoLines\nNewLines";

            //Act
            string actualNewString = FileExtension.AddLine(mainString, newLines);

            //Assert
            Assert.AreEqual(expectedNewString, actualNewString);
        }

        [TestMethod]
        public void FileExtensionTests_ToBytes()
        {
            //Arrange
            string stringSize = "10MB";

            //Act
            int byteSize = FileExtension.ToBytes(stringSize);
            int expectedByteSize = 10 * 1024 * 1024;

            //Assert
            Assert.AreEqual(expectedByteSize, byteSize);
        }
    }
}