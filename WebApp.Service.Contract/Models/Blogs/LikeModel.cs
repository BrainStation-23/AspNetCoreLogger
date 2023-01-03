using WebApp.Service.Contract.Models.Users;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class LikeModel : MasterModel
    {
        public long PostId { get; set; }
        public long UserId { get; set; }

        public PostModel Post { get; set; }
        public UserModel User { get; set; }
    }
}
