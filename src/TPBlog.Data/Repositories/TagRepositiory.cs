using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class TagRepositiory : RepositoryBase<Tag, Guid>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepositiory(TPBlogContext context, IMapper mapper) : base
            (context)
        {
            _mapper = mapper;

        }

        public async Task<List<string>> GetAllTags()
        {
            var data = _context.Tags.Select(x => x.Name);
            return await data.ToListAsync();
        }

        public async Task<TagDto?> GetBySlug(string slug)
        {
            var data= await _context.Tags.FirstOrDefaultAsync(x => x.Slug == slug);
            if (data == null) return null;
            return _mapper.Map<TagDto>(data);

        }

    }
}
