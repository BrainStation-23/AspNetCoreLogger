using System;
using System.Collections.Generic;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class PostBulkModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedFromDateUtc { get; set; }

        public List<CommentBulkModel> Comments { get; set; }
        public List<LikeBulkModel> Likes { get; set; }
        public List<TagBulkModel> Tags { get; set; }
    }
}
