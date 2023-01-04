using GreenPipes.Pipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Extensions
{
    public static class FileExtension
    {

        /// <summary>
        /// Read a directory if exists or create a new directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Read a file from a given path and filename if exists or create a new file named the given filename.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FileInfo ReadOrCreateFile(string path, string filename)
        {
            path = $"{path}\\{filename}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                System.IO.File.Create(path).Dispose();
                fileInfo = new FileInfo(path);
            }

            return fileInfo;
        }

        /// <summary>
        /// Returns the last created file of a path if any exists or returns null.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static FileInfo GetLastCreatedFile(this DirectoryInfo directory, string namingFormate)
        {
            var lastFile = directory.GetFiles().Where(f => f.Name.ToLower().Contains(namingFormate)).OrderByDescending(file => file.LastWriteTime).FirstOrDefault();
            if (lastFile is null)
                return null;
            return lastFile;
        }

        /// <summary>
        /// Prepares file for log in the directory according to the configurations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileConfig"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static FileInfo PrepareLogFile<T>(Loggers.File fileConfig, T model) where T : class
        {
            string path = fileConfig.Path;
            string maxFileSize = fileConfig.FileSize;
            string fileFormate = fileConfig.FileFormate;

            string dateStamp = DateTime.Now.ToString("yyyyMMdd");
            var logName = model.GetType().Name;

            logName = logName.Remove(logName.Length - 5);

            var fileNamingFormate = $"{logName}_Log_{fileFormate}_{dateStamp}";
            var defaultFileName = fileNamingFormate + "_1.txt";

            var dir = ReadOrCreateDirectory($"{path}/{dateStamp}");

            var lastCreatedFile = dir.GetLastCreatedFile(fileNamingFormate);

            var fileName = defaultFileName;

            if (lastCreatedFile != null)
            {
                if (lastCreatedFile.IsFileSizeExceed(maxFileSize))
                    fileName = GetNewFileName(lastCreatedFile.Name);
                else
                    fileName = lastCreatedFile.Name;
            }

            var file = ReadOrCreateFile(dir.FullName, fileName.ToLower());

            return file;
        }

        /// <summary>
        /// Check whether a file size is exceeded more than maxSize or not.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="maxSizeInMb"></param>
        /// <returns></returns>
        public static bool IsFileSizeExceed(this FileInfo fileInfo, string maxSizeInMb)
        {
            if (fileInfo.Length > maxSizeInMb.ToBytes())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts megabytes to bytes. Filesize formate should be "%MB"
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int ToBytes(this string filesize)
        {
            filesize = filesize.Remove(filesize.Length - 2);
            var mbInNumber = int.Parse(filesize);
            return mbInNumber * 1024 * 1024;
        }

        /// <summary>
        /// Generate log files according to the configurations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileConfig"></param>
        /// <param name="model"></param>
        public static void LogWrite<T>(Loggers.File fileConfig, T model) where T : class
        {
            var file = PrepareLogFile(fileConfig, model);

            if (file.Exists)
            {
                using (StreamWriter writer = System.IO.File.AppendText(file.FullName))
                {
                    var message = "";
                    if (fileConfig.FileFormate == "JSON")
                        message = PrepareMessageForJSONFormate(model);
                    else
                        message = PrepareMessageForTextFormate(model);

                    writer.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Generate log files according to the configurations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileConfig"></param>
        /// <param name="model"></param>
        public static void LogWrite<T>(Loggers.File fileConfig, List<T> models) where T : class
        {
            foreach (var model in models)
            {
                LogWrite(fileConfig, model);
            }
        }

        /// <summary>
        /// Return a header string for file.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HeaderAppender(string statusCode, string url)
        {
            var message = $"[Request: {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}] {statusCode} {url}\n";

            return message;
        }
        public static string HeaderAppender(string logType)
        {
            var message = $"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss zz")} [{logType}] ";

            return message;
        }
        /// <summary>
        /// Return a footer string to file.
        /// </summary>
        /// <returns></returns>
        public static string FooterAppender()
        {
            var message = "\n----------------------------------------------------------------------------------";

            return message;
        }

        /// <summary>
        /// Increment the file number and return a new file name according to the last file name
        /// </summary>
        /// <param name="lastFileName"></param>
        /// <returns></returns>
        public static string GetNewFileName(string lastFileName)
        {
            var extension = lastFileName.Split('.')[1];
            var lastFileNameWithoutExtension = lastFileName.Split('.')[0];

            var splitedStrings = lastFileNameWithoutExtension.Split('_');
            var numberString = splitedStrings[4];
            var number = int.Parse(numberString);
            number += 1;
            numberString = number.ToString();
            splitedStrings[4] = numberString;

            var newFileName = string.Join("_", splitedStrings) + '.' + extension;

            return newFileName;
        }
        public static string AddLine(this string mainString, string toAdd)
        {
            mainString = mainString + "\n" + toAdd;

            return mainString;
        }
        public static string PrepareMessageForTextFormate<T>(T model) where T : class
        {

            var properites = model.GetProperties();
            var statusCode = "";
            var url = "";
            string message = "";

            properites.ForEach(prop =>
            {
                if (prop.Name == "StatusCode")
                    statusCode = prop.GetValue(model).ToString();

                if (prop.Name == "Url")
                    url = prop.GetValue(model).ToString();
            });


            message = message.AddLine(HeaderAppender(statusCode, url));

            foreach (var prop in properites)
            {
                message = message.AddLine($"{prop.Name}: {prop.GetValue(model)}");
            }

            message = message.AddLine(FooterAppender());

            return message;

        }
        public static string PrepareMessageForJSONFormate<T>(T model) where T : class
        {
            var jsonText = model.ToPrettyJson();
            var logType = model.GetType().Name;

            logType = logType.Remove(logType.Length - 5).ToLower();

            jsonText = HeaderAppender(logType) + jsonText + FooterAppender();

            return jsonText;
        }

        public static List<string> GetFileNamesBySearchKey(Loggers.File fileConfig, string searchKey)
        {
            string path = fileConfig.Path;

            var directory = ReadOrCreateDirectory(path);

            var fileNameList = new List<string>();

            directory.GetDirectories().ToList().ForEach(fileFolder =>
            {
                fileNameList =fileNameList.Concat(fileFolder.GetFiles().Select(folder=>folder.Name).Where(f => f.ToLower().Contains(searchKey.ToLower())).ToList()).ToList();
            });

            return fileNameList;
        }

        public static Dictionary<string, List<string>> GetAllLogFileDirectory(this Loggers.File fileConfig)
        {
            string path = fileConfig.Path;

            var directory = ReadOrCreateDirectory(path);
            var fileTree = new Dictionary<string, List<string>>();

            directory.GetDirectories().ToList().ForEach(fileFolder =>
            {
                var files = fileFolder.GetFiles().Select(f => f.Name).ToList();

                fileTree.Add(fileFolder.Name, files);
            });

            return fileTree;

        }

        public static List<object> GetLogsFromSpecificFiles(this Loggers.File fileConfig, string fileName)
        {
            string path = fileConfig.Path;

            var directory = ReadOrCreateDirectory(path);
            var fileTree = new Dictionary<string, List<string>>();

            List<object> logObjects = null;

            directory.GetDirectories().ToList().ForEach(fileFolder =>
            {
                fileFolder.GetFiles().ToList().ForEach(f =>
                {
                    if (f.Name.ToLower() == fileName.ToLower())
                    {
                        logObjects = File.ReadAllText(f.FullName).ToLogObjects();
                        return;
                    }
                });
            });

            return logObjects;

        }

        public static List<object> ToLogObjects(this string logsInStringForm)
        {
            var logObjects = new List<object>();

            logsInStringForm.Split(FooterAppender()).ToList().ForEach(logsInStringForm =>
            {
                var ind = logsInStringForm.IndexOf('{');
                if (ind >= 0) {
                    var obj = logsInStringForm.Substring(ind);
                    logObjects.Add(obj.ToModel<object>());
                }
                
            });

            return logObjects;
        }
    }
}
