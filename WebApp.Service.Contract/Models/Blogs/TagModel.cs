using System;
using System.Collections.Generic;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class TagModel : MasterModel
    {
        public string Name { get; set; }
        public DateTime? CreatedDateUtc { get; set; }

        public IList<PostTagModel> Posts { get; set; }
    }
}
