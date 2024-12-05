using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_TaskUser")]
    [PrimaryKey(nameof(TaskId), nameof(UserId))]
    public class IC_TaskUser
    {
        public Guid TaskId { set; get; }
        public Guid UserId { set; get; }
    }
}
