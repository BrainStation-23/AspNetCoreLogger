using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class PostBulkModel : MasterModel
    {
        public long BlogId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedFromDateUtc { get; set; }

        public List<CommentBulkModel> Comments { get; set; }
        public List<LikeBulkModel> Users { get; set; }
        public List<TagBulkModel> Tags { get; set; }
    }
}
