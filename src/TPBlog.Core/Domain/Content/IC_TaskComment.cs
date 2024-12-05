using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_TaskComments")]
    [Index(nameof(Slug), IsUnique = true)]
    public class IC_TaskComment : EntityBase<Guid>
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public required string CommentText { get; set; }
        [MaxLength(250)]
        public required string Slug { get; set; }
        [Column(TypeName = "varchar(250)")]

        public string ProjectSlug { get; set; }

    }
}
