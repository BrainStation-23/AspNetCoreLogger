using System;
using WebApp.Sql.Entities.Blogs;

namespace WebApp.Service.Models.Blogs
{
    public class PostModel : MasterModel
    {
        public long BlogId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedFromDateUtc { get; set; }

        public BlogEntity Blog { get; set; }
    }
}
