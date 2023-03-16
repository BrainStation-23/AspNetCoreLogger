using System;

namespace WebApp.Logger.Models
{
    public class TraceModel
    {
        public long? UserId { get; set; }
        public string ApplicationName { get; set; }
        public string TraceId { get; set; }
        public string IpAddress { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Trace { get; set; }
        public float Duration { get; set; }
        public DateTime? CreatedDateUtc { get; set; } = DateTime.UtcNow;
    }
}
