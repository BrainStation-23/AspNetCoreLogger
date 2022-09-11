using System.Collections.Generic;

namespace WebApp.Logger.Loggers
{
    public class LogOption
    {
        public const string Name = "LoggerWrapper";

        public List<string> ProviderType { get; set; }
        public List<string> LogType { get; set; }
        public string Mode { get; set; }
        public bool EnableMask { get; set; }
        public bool EnableIgnore { get; set; }
        public string Retention { get; set; }

        public Logs Log { get; set; }
        public Provider Provider { get; set; }
    }

    public class Provider
    {
        public MSSql MSSql { get; set; }
        public File File { get; set; }
        public Mongo Mongo { get; set; }
        public CosmosDb CosmosDb { get; set; }
    }

    public class Audit
    {
        public List<string> IgnoreColumns { get; set; }
        public List<string> MaskColumns { get; set; }
    }

    public class Request
    {
        public List<string> IgnoreColumns { get; set; }
        public List<string> MaskColumns { get; set; }
    }

    public class Sql
    {
        public List<string> IgnoreColumns { get; set; }
        public List<string> MaskColumns { get; set; }
    }

    public class Error
    {
        public List<string> IgnoreColumns { get; set; }
        public List<string> MaskColumns { get; set; }
    }

    public class CosmosDb
    {
        public List<string> LogType { get; set; }
        public string Mode { get; set; }
        public bool Mask { get; set; }
        public string AccountUrl { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string Retention { get; set; }
    }

    public class File
    {
        public List<string> LogType { get; set; }
        public string Mode { get; set; }
        public bool Mask { get; set; }
        public string Retention { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
    }


    public class Mongo
    {
        public List<string> LogType { get; set; }
        public string Version { get; set; }
        public bool Mode { get; set; }
        public string Retention { get; set; }
        public string ConnectionString { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseName1 { get; set; }
        //public string ApiKey { get; set; }
        //public string SecretKey { get; set; }
        //public string Credntial { get; set; }
    }

    public class MSSql
    {
        public List<string> LogType { get; set; }
        public string Mode { get; set; }
        public bool Mask { get; set; }
        public string Retention { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }


    public class Logs
    {
        public Sql Sql { get; set; }
        public Error Error { get; set; }
        public Request Request { get; set; }
        public Audit Audit { get; set; }
    }

}
