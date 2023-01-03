using System;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class CommentModel : MasterModel
    {
        public long PostId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedDateUtc { get; set; }

        public PostModel Post { get; set; }
    }
}
