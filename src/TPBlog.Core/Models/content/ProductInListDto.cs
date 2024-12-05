using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class ProductInListDto
    {
        public Guid Id { get; set; }

        public string No { get; set; }

        public string Name { get; set; }
        public required string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? CatalogPdf { get; set; }
        public string?[] Image { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public string ProCategorySlug { get; set; }
        public string ProCategoryName { get; set; }
        public Guid ProCategoryId { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_Product, ProductInListDto>();
            }
        }
    }
}
