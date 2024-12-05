using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Shared.Enums
{
    public enum PostStatus
    {
        Draft = 0,
        WaitingForApproval = 1,
        Rejected = 2,
        Published = 3,
        //AppovedConsult=4
    }
}
