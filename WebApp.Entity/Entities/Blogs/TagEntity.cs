using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Common.Sqls;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class TagEntity:BaseEntity
    {
        public string Name { get; set; }

        public IList<PostTagEntity> Posts { get; set; }
    }
}
