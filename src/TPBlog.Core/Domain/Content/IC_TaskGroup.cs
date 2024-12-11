using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Domain.Content
{
    public class IC_TaskGroup
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
