using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Shared.Enums
{
    public enum TaskUserStatus
    {
        Pending = 0,
        WaitingForApproval = 1,
        Rejected = 2,
        Done = 3,
    }
}
