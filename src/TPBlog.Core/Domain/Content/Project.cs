using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace TPBlog.Core.Domain.Content
{
    [Table("Projects")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Project : EntityBase<Guid>
    {
        [MaxLength(250)]
        [Required]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [Column(TypeName = "varchar(250)")]
        public required string Slug { get; set; }

        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        [MaxLength(250)]
        public string? SeoDescription { get; set; }

        [MaxLength(250)]
        public string? Thumbnail { set; get; }

        public string? Content { get; set; }
        public Guid AuthorUserId { get; set; }

        [EmailAddress]
        public string? EmailProAddress { get; set; }

    }
}
