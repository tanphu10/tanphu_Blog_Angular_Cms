using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_TaskNotifications")]
    public class IC_TaskNotifications : EntityBase<Guid>
    {
        public Guid TaskId { get; set; }
        public Guid UserBy { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public string ProjectSlug { get; set; }
    }
}
