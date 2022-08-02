using System.Collections.Generic;
using WebApp.Common.Sqls;

namespace WebApp.Entity.Entities.Blogs
{
    public class BlogEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Motto { get; set; }

        public IList<PostEntity> Posts { get; set; }
    }
}
