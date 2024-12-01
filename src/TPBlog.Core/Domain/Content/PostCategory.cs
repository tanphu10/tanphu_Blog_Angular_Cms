using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Entity;

namespace TPBlog.Core.Domain.Content
{
    [Table("PostCategories")]
    [Index(nameof(Slug), IsUnique = true)]
    public class PostCategory : EntityBase<Guid>
    {

        [MaxLength(250)]
        public required string Name { set; get; }

        [Column(TypeName = "varchar(250)")]
        public required string Slug { set; get; }
        public Guid? ParentId { set; get; }
        public bool IsActive { set; get; }

        [MaxLength(160)]
        public string? SeoDescription { set; get; }
        public int SortOrder { set; get; }
        public Guid ProjectId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }


    }
}