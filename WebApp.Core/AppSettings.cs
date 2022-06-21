using System.Collections.Generic;

namespace WebApp.Core
{
    public class AppSettings
    {
        public Domain Domain { get; set; }
        public JwtOption JwtOption { get; set; }
        public CacheConfig CacheConfig { get; set; }
        public Cms Cms { get; set; }
        public RabbitMqConnection RabbitMq { get; set; }
        public EmailSetting EmailSetting { get; set; }
        public SmsSetting SmsSetting { get; set; }
        public ClientApp ClientApp { get; set; }
    }

    public class Domain
    {
        public List<string> Client1 { get; set; }
        public List<string> Client2 { get; set; }
        public List<string> All { get; set; }
        public string DomainName { get; set; }
    }

    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
        string AuditCollectinName { get; set; }
    }


    public class RabbitMqConnection
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QName { get; set; }
        public int PrefetchCount { get; set; }
    }

    public class JwtOption
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public string Alg { get; set; }
        public uint ValidForInMinitues { get; set; }
        public uint RefreshTokenValidForInDays { get; set; }
        public uint LockUserJwtInMinitues { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; }
        public string AuthenticationScheme { get; set; }
        public string TokenType { get; set; }
    }

    public class CacheConfig
    {
        public uint BaseControllerCacheDuration { get; set; }
        public CacheKeyDuration HoldingQueuePaged { get; set; }
        public uint LifeLineSyncDurationInMinute { get; set; }
    }

    public class Cms
    {
        public string Api { get; set; }
    }

    public class CacheKeyDuration
    {
        public string CacheKey { get; set; }
        public uint Duration { get; set; }
    }

    public class EmailSetting
    {
        public string SenderEmail { get; set; }
        public string HostServer { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsSSL { get; set; }
        public bool IsAsync { get; set; }
        public int TryLimit { get; set; }
    }

    public class SmsSetting
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TryLimit { get; set; }
    }

    public class ClientApp
    {
        public string BaseUrl { get; set; }
    }
}
