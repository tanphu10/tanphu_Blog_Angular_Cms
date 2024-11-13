using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class ProjectInListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Slug { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string? SeoKeywords { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Project, ProjectInListDto>();
            }
        }
    }
}
