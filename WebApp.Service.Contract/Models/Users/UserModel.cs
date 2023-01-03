using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Service.Contract.Models.Users
{
    public class UserModel
    {
        [Key]
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<LikeModel> Posts { get; set; }
    }
}
