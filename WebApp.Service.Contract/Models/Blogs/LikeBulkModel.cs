﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service.Contract.Models.Blogs
{
    public class LikeBulkModel : MasterModel
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
    }
}