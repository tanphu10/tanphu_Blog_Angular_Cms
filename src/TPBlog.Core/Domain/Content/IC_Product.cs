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
    [Table("IC_Products")]
    [Index(nameof(Slug), IsUnique = true)]
    public class IC_Product : EntityBase<Guid>
    {
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string No { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string Slug { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Summary { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }
        public string?[] Image { get; set; }
        public string? catalogPdf { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Guid ProCategoryId { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string ProCategorySlug { get; set; }
        [MaxLength(250)]
        [Required]
        public required string ProCategoryName { get; set; }
        public string? BaseUnit { get; set; }

    }
}
