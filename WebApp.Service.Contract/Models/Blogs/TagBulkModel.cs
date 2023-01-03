using System;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class TagBulkModel
    {
        public string Name { get; set; }

        public DateTimeOffset CreatedDateUtc { get; set; }
    }
}
