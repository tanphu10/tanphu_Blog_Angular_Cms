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
    [Table("TaskAttachments")]
    [Index(nameof(Slug), IsUnique = true)]
    public class TaskAttachment : EntityBase<Guid>
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string FilePath { get; set; }
        [MaxLength(250)]
        public required string Slug { get; set; }
        [Column(TypeName = "varchar(250)")]

        public string ProjectSlug { get; set; }

    }
}
