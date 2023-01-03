using System.Collections.Generic;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class BlogBulkModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Motto { get; set; }

        public List<PostBulkModel> Posts { get; set; }
    }
}
