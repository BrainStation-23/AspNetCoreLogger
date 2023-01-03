namespace WebApp.Service.Contract.Models.Blogs
{
    public class PostTagModel : MasterModel
    {
        public long PostId { get; set; }
        public long TagId { get; set; }

        public PostModel Post { get; set; }
        public TagModel Tag { get; set; }
    }
}
