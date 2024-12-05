using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public abstract class CreateOrUpdateProductDto
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters.")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters.")]
        public string Summary { get; set; }

        public string Description { get; set; }
        public string Slug { get; set; }

        public decimal Price { get; set; }
        public string? CatalogPdf { get; set; }
        public string?[] Image { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsActive { get; set; }

        public Guid ProCategoryId { get; set; }

    }
}
