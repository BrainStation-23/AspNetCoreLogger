using System.Collections.Generic;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class BlogModel : MasterModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Motto { get; set; }

        public IList<PostModel> Posts { get; set; }
    }
}
