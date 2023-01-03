using WebApp.Common.Sqls;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class LikeEntity : BaseEntity
    {
        public long PostId { get; set; }
        public PostEntity Post { get; set; }
    }
}
