using GreenPipes.Pipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using WebApp.Logger.Loggers;
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
        public static DirectoryInfo ReadOrCreateDirectory(string path, string root = "wwwroot")
        {
            if (string.IsNullOrEmpty(path))
                return null;

            path = Path.Combine(Directory.GetCurrentDirectory(), $"{root}\\{path}");

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
                return directoryInfo;

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo ReadDirectory(string path, string root = "wwwroot")
        {
            if (string.IsNullOrEmpty(path))
                return null;

            path = Path.Combine(Directory.GetCurrentDirectory(), $"{root}\\{path}");

            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            return directoryInfo;
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
        public static FileInfo GetLastCreatedFile(this DirectoryInfo directory, string namingFormat)
        {
            var lastFile = directory.GetFiles().Where(f => f.Name.ToLower().Contains(namingFormat)).OrderByDescending(file => file.LastWriteTime).FirstOrDefault();
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
            string fileFormat = fileConfig.FileFormate;

            string dateStamp = DateTime.Now.ToString("yyyyMMdd");
            var logName = model.GetLogType();

            var fileNamingFormat = $"{logName}_Log_{fileFormat}_{dateStamp}";
            var defaultFileName = fileNamingFormat + "_1.txt";

            var dir = ReadOrCreateDirectory($"{path}/{logName}/{dateStamp}");

            var lastCreatedFile = dir.GetLastCreatedFile(fileNamingFormat.ToLower());

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
        /// Converts megabytes to bytes. Filesize format should be "%MB"
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
                        message = PrepareMessageForJSONFormat(model);
                    else
                        message = PrepareMessageForTextFormat(model);

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
            if (models.Any() is false)
                return;
            var file = PrepareLogFile(fileConfig, models[0]);

            if (file.Exists)
            {
                using (StreamWriter writer = System.IO.File.AppendText(file.FullName))
                {
                    var message = "";
                    if (fileConfig.FileFormate == "JSON")
                        message = PrepareMessageForJSONFormat(models);
                    else
                        message = PrepareMessageForTextFormat(models);

                    writer.WriteLine(message);
                }
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
        public static string PrepareMessageForTextFormat<T>(T model) where T : class
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
        public static string PrepareMessageForJSONFormat<T>(T model) where T : class
        {
            var jsonText = model.ToPrettyJson();

            var logType = model.GetLogType();

            jsonText = HeaderAppender(logType) + jsonText + FooterAppender();

            return jsonText;
        }

        public static string PrepareMessageForJSONFormat<T>(List<T> models) where T : class
        {
            var JsonReturnText = "";

            foreach (var model in models)
            {
                var jsonText = model.ToPrettyJson();
                var logType = model.GetLogType();

                JsonReturnText = JsonReturnText + HeaderAppender(logType) + jsonText + FooterAppender() + '\n';
            }

            return JsonReturnText;
        }

        public static Dictionary<string, object> GetDirectories(string path, bool withSubdirectories = false)
        {
            var directory = ReadDirectory(path);

            if (directory is null)
                return null;

            return GetDirectories(directory, withSubdirectories);
        }

        public static Dictionary<string, object> GetDirectories(DirectoryInfo directory, bool withSubdirectories = false)
        {
            if (directory is null)
                return null;

            var fileTree = new Dictionary<string, object>();

            var subDirectories = new List<object>();

            if (withSubdirectories is true)
            {
                directory.GetDirectories().ToList().ForEach(subDirectory =>
                {
                    subDirectories.Add(GetDirectories(subDirectory, true));
                });
            }

            directory.GetFiles().ToList().ForEach(file =>
            {
                subDirectories.Add(file.Name);
            });

            fileTree.Add(directory.Name, subDirectories);

            return fileTree;

        }

        public static FileInfo SearchFileWithDirectory(this DirectoryInfo directory, string fileName, bool searchInSubdirectory = false)
        {
            if (directory is null)
                return null;

            FileInfo file = null;

            directory.GetFiles().ToList().ForEach(fi =>
            {
                if (fi.Name.ToLower() == fileName.ToLower())
                {
                    file = fi;

                    return;
                }
            });

            if (file is null && searchInSubdirectory is true)
            {
                directory.GetDirectories().ToList().ForEach(subDirectory =>
                {

                    var logObjectsFromSubDirectory = SearchFileWithDirectory(subDirectory, fileName, searchInSubdirectory);

                    if (logObjectsFromSubDirectory is not null)
                    {
                        file = logObjectsFromSubDirectory;

                        return;
                    }

                });
            }

            return file;

        }

        public static List<string> SearchFiles(this DirectoryInfo directory, string searchkey)
        {
            if (directory is null)
                return null;

            var files = new List<string>();

            directory.GetDirectories().ToList().ForEach(subDirectory =>
            {
                subDirectory.GetFiles().ToList().ForEach(fi =>
                {
                    if (fi.Name.ToLower().Contains(searchkey.ToLower()))
                    {
                        files.Add(fi.Name);
                    }
                });

                var filesFromSubDirectory = SearchFiles(subDirectory, searchkey);

                if (filesFromSubDirectory is not null)
                {
                    files = files.Concat(filesFromSubDirectory).ToList();

                    return;
                }

            });

            return files;

        }

        public static List<string> GetFilenames(this string path, string searchkey)
        {
            var directory = ReadDirectory(path);

            if (directory is null)
                return null;

            return SearchFiles(directory, searchkey);
        }

        public static FileInfo SearchFileWithPath(this string path, string fileName)
        {
            var directory = ReadDirectory(path);

            if (directory is null)
                return null;

            var file = directory.SearchFileWithDirectory(fileName, true);

            return file;
        }

        public static List<object> GetLogObjects(this string path, string fileName)
        {
            var file = path.SearchFileWithPath(fileName);

            if (file is null)
                return null;

            var fileContent = System.IO.File.ReadAllText(file.FullName);

            return fileContent.ToLogObjects();
        }

        public static List<object> ToLogObjects(this string jsonString)
        {
            var logObjects = new List<object>();

            jsonString.Split(FooterAppender()).ToList().ForEach(j =>
            {
                var ind = j.IndexOf('{');

                if (ind >= 0)
                {
                    var obj = j.Substring(ind).ToModel<object>();
                    logObjects.Add(obj);
                }

            });

            return logObjects;
        }

        public static string GetLogType<T>(this T model)
        {
            var logName = model.GetType().Name.ToLower();

            if (logName.Contains(LogType.Audit.ToString().ToLower()))
            {
                return LogType.Audit.ToString().ToLower();
            }
            else if (logName.Contains(LogType.Sql.ToString().ToLower()))
            {
                return LogType.Sql.ToString().ToLower();
            }
            else if (logName.Contains(LogType.Request.ToString().ToLower()))
            {
                return LogType.Request.ToString().ToLower();
            }
            else if (logName.Contains(LogType.Error.ToString().ToLower()))
            {
                return LogType.Error.ToString().ToLower();
            }
            else
                return "UnknownLog";
        }

        public static DirectoryInfo SearchDirectory(this DirectoryInfo currentDirectory, string directoryName)
        {
            if (currentDirectory is null)
                return null;

            if (currentDirectory.Exists is false)
                return null;

            DirectoryInfo directoryInfo = null;

            currentDirectory.GetDirectories().ToList().ForEach(d =>
            {
                if (d.Name.ToLower() == directoryName.ToLower())
                {
                    directoryInfo = d;
                    return;
                }
                else
                {
                    var dir = d.SearchDirectory(directoryName);
                    if (dir is not null)
                    {
                        directoryInfo = dir;
                        return;
                    }
                }

            });

            return directoryInfo;
        }

        public static void RetentionFileLogs(this DateTime datetime, string path, string logtype)
        {
            string date = datetime.ToString("yyyyMMdd");

            logtype = logtype.ToLower();

            var mainDirectory = ReadDirectory(path);

            if (mainDirectory is null)
            {
                return;
            }

            var logDirectory = mainDirectory.SearchDirectory(logtype);

            if (logDirectory is not null)
            {
                logDirectory.GetDirectories().AsQueryable().Where(d => d.Name.ToString().CompareTo(date) <= 0).Select(d => d.FullName).ToList().ForEach(directoryFullname =>
                {
                    Directory.Delete(directoryFullname, true);
                });
            }
        }
    }
}
