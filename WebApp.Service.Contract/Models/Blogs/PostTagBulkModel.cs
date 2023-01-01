using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class PostTagBulkModel : MasterModel
    {
        public long PostId { get; set; }
        public long TagId { get; set; }
    }
}
