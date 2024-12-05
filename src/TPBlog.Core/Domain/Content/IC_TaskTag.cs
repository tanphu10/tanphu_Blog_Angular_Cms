using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_TaskTags")]
    [PrimaryKey(nameof(TaskId), nameof(TagId))]
    public class IC_TaskTag
    {
        public Guid TaskId { set; get; }
        public Guid TagId { set; get; }
    }
}