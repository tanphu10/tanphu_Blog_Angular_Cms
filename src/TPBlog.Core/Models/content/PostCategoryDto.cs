using AutoMapper;
using TPBlog.Core.Domain.Content;

namespace TPBlog.Core.Models.content
{
    public class PostCategoryDto
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
        public string ProjectSlug { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<PostCategory, PostCategoryDto>();
            }
        }
    }
}
