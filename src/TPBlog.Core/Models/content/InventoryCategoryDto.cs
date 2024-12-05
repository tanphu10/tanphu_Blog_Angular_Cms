using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class InventoryCategoryDto
    {
        public Guid Id { get; set; }
        public required string Name { set; get; }
        public required string Slug { set; get; }
        public Guid? ParentId { set; get; }
        public bool IsActive { set; get; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public string? SeoDescription { set; get; }
        public int SortOrder { set; get; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_InventoryCategory, InventoryCategoryDto>();
            }
        }
    }
}
