using System;
using System.IO;

namespace WebApp.Core.Files
{
    public static class FileExtensions
    {
        public static string ReadFileAsText(this string fileLocation)
        {
            string textString = string.Empty;
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileLocation);
           
            using StreamReader streamReader = new StreamReader(filepath);
            textString = streamReader.ReadToEnd();

            return textString;
        }
    }
}
