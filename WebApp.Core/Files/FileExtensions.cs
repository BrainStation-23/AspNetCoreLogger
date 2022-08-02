using System;
using System.IO;

namespace WebApp.Core.Files
{
    public static class FileExtensions
    {
        public static string ReadFileAsText(this string fileLocation)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileLocation);
           
            using StreamReader streamReader = new StreamReader(filepath);
            string textString = streamReader.ReadToEnd();

            return textString;
        }
    }
}
