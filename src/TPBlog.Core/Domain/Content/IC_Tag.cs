using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_Tags")]
    public class IC_Tag
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Slug { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }

    }
}