using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class TagBulkModel : MasterModel
    {
        public string Name { get; set; }

        public DateTime? CreatedDateUtc { get; set; }
    }
}
