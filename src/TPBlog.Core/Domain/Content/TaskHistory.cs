using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{

    [Table("TaskHistories")]
    [Index(nameof(Slug), IsUnique = true)]
    public class TaskHistory : EntityBase<Guid>
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [MaxLength(250)]
        public required string Slug { get; set; }
        public TaskUserStatus ChangeTaskStatus { get; set; }
        public PriorityStatus ChangePrority { get; set; }
        public string oldContent { get; set; }
        public string newContent { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
    }
   
}
