using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Domain.Entity;

namespace TPBlog.Core.Domain.Content
{
    [Table("Series")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Series : EntityBase<Guid>
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
        public Guid ProjectId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }


    }
}