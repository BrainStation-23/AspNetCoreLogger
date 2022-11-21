namespace WebApp.Logger.Models
{
    public class SqlModel
    {
        public long? UserId { get; set; }
        public string ApplicationName { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Proctocol { get; set; }
        public string UrlReferrer { get; set; }
        public string Area { get; set; }
	    public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }

        public string Connection { get; set; }
        public string Command { get; set; }
        public string Event { get; set; }
        public string QueryType { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
        public double Duration { get; set; }
        public string Message { get; set; }
    }
}
