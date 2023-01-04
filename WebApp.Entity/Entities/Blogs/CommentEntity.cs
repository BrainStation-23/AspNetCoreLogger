using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Common.Sqls;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class CommentEntity:BaseEntity
    {
        public long PostId { get; set; }
        public string Comment { get; set; }
        public PostEntity Post { get; set; }
    }
}
