using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class CommentBulkModel : MasterModel
    {
        public long PostId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
