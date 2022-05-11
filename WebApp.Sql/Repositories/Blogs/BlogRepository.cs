using WebApp.Core.Sqls;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Sql.Repositories
{
    public class BlogRepository : SqlRepository<BlogEntity>, IBlogRepository
    {
        public BlogRepository(WebAppContext context) : base(context)
        {
        }
    }
}
