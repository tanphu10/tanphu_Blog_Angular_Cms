using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPBlog.Core.Domain.Content
{
    [Table("TaskTags")]
    [PrimaryKey(nameof(TaskId), nameof(TagId))]
    public class TaskTag
    {
        public Guid TaskId { set; get; }
        public Guid TagId { set; get; }
    }
}