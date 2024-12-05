using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_PostInProject")]
    [PrimaryKey(nameof(PostId), nameof(ProjectId))]
    public class IC_PostInProject
    {
        public Guid PostId { get; set; }
        public Guid ProjectId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
