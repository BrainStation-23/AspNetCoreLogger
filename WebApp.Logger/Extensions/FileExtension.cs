using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApp.Logger.Models;

namespace WebApp.Logger.Extensions
{
    public static class FileExtension
    {
        public static DirectoryInfo ReadOrCreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{path}");

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
                return directoryInfo;

            return Directory.CreateDirectory(path);
        }

        public static FileInfo ReadOrCreateFile(string path, string filename)
        {
            path = $"{path}\\{filename}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                File.Create(path).Dispose();
                fileInfo = new FileInfo(path);
                if (fileInfo.Length > 1024)
                {

                }
            }

            return fileInfo;
        }

        //public static async Task FileCrop(string path, string line)
        //{
        //    if (File.ReadAllBytes().Length >= 100 * 1024 * 1024) // (100mB) File to big? Create new
        //    {
        //        string filenamebase = "myLogFile"; //Insert the base form of the log file, the same as the 1st filename without .log at the end
        //        if (filename.contains("-")) //Check if older log contained -x
        //        {
        //            int lognumber = Int32.Parse(filename.substring(filename.lastIndexOf("-") + 1, filename.Length - 4); //Get old number, Can cause exception if the last digits aren't numbers
        //            lognumber++; //Increment lognumber by 1
        //            filename = filenamebase + "-" + lognumber + ".log"; //Override filename
        //        }
        //        else
        //        {
        //            filename = filenamebase + "-1.log"; //Override filename
        //        }
        //        fs = File.Create(filename);
        //        fs.Close();
        //    }
        //}

        public static void LogWrite(string path, string filename, RequestModel model)
        {
            filename = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            var dir = ReadOrCreateDirectory(path + "/Route_Logs");
            var file = ReadOrCreateFile(dir.FullName, filename);
            if (file.Exists)
            {
                //var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter stream = File.AppendText(file.FullName))
                    LogWriteMessage(model, stream);
            }
        }
        public static void LogWrite(string path, string filename, ErrorModel model)
        {
            filename = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            var dir = ReadOrCreateDirectory(path + "/Error_Logs");
            var file = ReadOrCreateFile(dir.FullName, filename);
            if (file.Exists)
            {
                //var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter stream = File.AppendText(file.FullName))
                    LogWriteMessage(model, stream);
            }
        }
        public static void LogWrite(string path, string filename, SqlModel model)
        {
            filename = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            var dir = ReadOrCreateDirectory(path + "/Query_Logs");
            var file = ReadOrCreateFile(dir.FullName, filename);
            if (file.Exists)
            {
                //var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter stream = File.AppendText(file.FullName))
                    LogWriteMessage(model, stream);
            }
        }
        public static void LogWrite(string path, string filename, List<AuditModel> models)
        {
            filename = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            var dir = ReadOrCreateDirectory(path + "/Audit_Logs");
            var file = ReadOrCreateFile(dir.FullName, filename);
            if (file.Exists)
            {
                //var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter stream = File.AppendText(file.FullName))
                {
                    foreach (var model in models) { LogWriteMessage(model, stream); }

                }
            }
        }
        public static void LogWrite(string path, string filename, AuditModel model)
        {
            filename = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

            var dir = ReadOrCreateDirectory(path + "/Audit_Logs");
            var file = ReadOrCreateFile(dir.FullName, filename);
            if (file.Exists)
            {
                //var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter stream = File.AppendText(file.FullName))
                    LogWriteMessage(model, stream);
            }
        }

        public static void LogWriteMessage(RequestModel model, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
                txtWriter.WriteLine("\r\n[Request: {0} {1}] {2} {3}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), model.StatusCode, model.Url);
                txtWriter.WriteLine("TraceId: {0}", model.TraceId);
                txtWriter.WriteLine("Body: {0}", model.Body);
                txtWriter.WriteLine("Response: {0}", model.Response);
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
        public static void LogWriteMessage(ErrorModel model, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
                txtWriter.WriteLine("\r\n[Request: {0} {1}] {2} {3}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), model.StatusCode, model.Url);
                txtWriter.WriteLine("TraceId: {0}", model.TraceId);
                txtWriter.WriteLine("Body: {0}", model.Body);
                txtWriter.WriteLine("Message: {0}", model.Message);
                txtWriter.WriteLine("Response: {0}", model.Response);
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
        public static void LogWriteMessage(SqlModel model, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
                txtWriter.WriteLine("\r\n[Request: {0} {1}] {2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), model.Url);
                txtWriter.WriteLine("TraceId: {0}", model.TraceId);
                txtWriter.WriteLine("Message: {0}", model.Message);
                txtWriter.WriteLine("Query: {0}", model.Query);
                txtWriter.WriteLine("Response: {0}", model.Response);
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
        public static void LogWriteMessage(AuditModel model, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
                txtWriter.WriteLine("\r\n[Request: {0} {1}]", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("TraceId: {0}", model.TraceId);
                txtWriter.WriteLine("Table Name: {0}", model.TableName);
                txtWriter.WriteLine("Old values: {0}", model.OldValues);
                txtWriter.WriteLine("New Values: {0}", model.NewValues);
                txtWriter.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
