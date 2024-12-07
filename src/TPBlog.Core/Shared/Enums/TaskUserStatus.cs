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
        InProgress = 1,
        OnHold = 2,
        Completed = 3,
        Cancelled = 4,
    }
}
