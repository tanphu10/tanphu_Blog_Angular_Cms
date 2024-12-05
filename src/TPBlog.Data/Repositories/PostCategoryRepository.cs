using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using TPBlog.Core.Repositories;
using AutoMapper;
using TPBlog.Core.Helpers;

namespace TPBlog.Data.Repositories
{
    public class PostCategoryRepository : RepositoryBase<IC_PostCategory, Guid>, IPostCategoryRepository
    {
        private readonly IMapper _mapper;
        //private readonly 
        public PostCategoryRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PageResult<PostCategoryDto>> GetPagingPostCategoryAsync(string? keyword, Guid? projectId, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.PostCategories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Slug.Contains(normalizedKeyword) ||
                         x.Name.Contains(normalizedKeyword));
            }

            if (projectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == projectId.Value);
            }

            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageResult<PostCategoryDto>
            {
                Results = await _mapper.ProjectTo<PostCategoryDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task<bool> HasPost(Guid categoryId)
        {
            return await _context.Posts.AnyAsync(x => x.CategoryId == categoryId);
        }
        public async Task<PostCategoryDto> GetBySlug(string Slug)
        {
            var query = await _context.PostCategories.FirstOrDefaultAsync(x => x.Slug == Slug);
            if (query == null) { throw new Exception($" not found{Slug}"); }
            return _mapper.Map<PostCategoryDto>(query);
        }
    }
}
