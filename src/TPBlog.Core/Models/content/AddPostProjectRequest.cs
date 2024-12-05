using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Models.content
{
    public class AddPostProjectRequest
    {
        public Guid PostId { get; set; }
        public Guid ProjectId { get; set; }
        public int SortOrder { set; get; }

    }
}
