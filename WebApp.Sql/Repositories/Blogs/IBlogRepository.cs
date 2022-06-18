using WebApp.Core.Sqls;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Sql.Repositories
{
    public interface IBlogRepository: ISqlRepository<BlogEntity>
    {
    }
}
