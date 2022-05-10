using WebApp.Core.Sqls;
using WebApp.Sql.Entities.Blogs;

namespace WebApp.Sql.Repositories
{
    public class PostRepository : SqlRepository<PostEntity>, IPostRepository
    {
        public PostRepository(WebAppContext context) : base(context)
        {
        }
    }
}
