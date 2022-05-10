using System.Collections.Generic;

namespace WebApp.Sql.Entities.Blogs
{
    public class BlogEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Motto { get; set; }

        public IList<PostEntity> Posts { get; set; }
    }
}
