using System;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class CommentBulkModel
    {
        public string Comment { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
    }
}
