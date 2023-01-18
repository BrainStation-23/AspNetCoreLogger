using Newtonsoft.Json;
using System;

namespace WebApp.Logger.Providers.CosmosDbs
{
    public interface IItem
    {
        public string Id { get; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; }
        public string LogType { get; }
    }
}
