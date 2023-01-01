using System;
using System.Collections.Generic;
using WebApp.Common.Sqls;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Entity.Entities.Blogs
{
    public class PostEntity : BaseEntity
    {
        public long BlogId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedFromDateUtc { get; set; }

        public BlogEntity Blog { get; set; }
        public IList<CommentEntity> Comments { get; set; }
        public IList<TagEntity> Tags { get; set; }
        public IList<PostTagEntity> PostTags { get; set; }
        public IList<LikeEntity> Users { get; set; }
    }
}
