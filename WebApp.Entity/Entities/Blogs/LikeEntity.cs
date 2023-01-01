using WebApp.Common.Sqls;
using WebApp.Core.Models;
using WebApp.Entity.Entities.Blogs;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class LikeEntity:BaseEntity
    {
        public long PostId { get; set; }       
        public long UserId { get; set; }

        public PostEntity Post { get; set; }
        public UserLogin User { get; set; }
    }
}
