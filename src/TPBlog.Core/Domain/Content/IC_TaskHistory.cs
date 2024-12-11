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

    [Table("IC_TaskHistories")]
    //[Index(nameof(Slug), IsUnique = true)]
    public class IC_TaskHistory : EntityBase<Guid>
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [MaxLength(250)]
        public required string TaskSlug { get; set; }
        public TaskUserStatus ChangeTaskStatus { get; set; }
        public PriorityStatus ChangePrority { get; set; }
        public string OldContent { get; set; }
        public string NewContent { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }

    }

}
