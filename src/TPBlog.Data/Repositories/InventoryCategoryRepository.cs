using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;
using TPBlog.Core.Helpers;

namespace TPBlog.Data.Repositories
{
    public class InventoryCategoryRepository : RepositoryBase<InventoryCategory, Guid>, IInventoryCategoryRepository
    {
        private readonly IMapper _mapper;
        //private readonly 
        public InventoryCategoryRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PageResult<InventoryCategoryDto>> GetPagingInventoryCategoryAsync(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.InventoryCategories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Slug.Contains(normalizedKeyword) ||
                         x.Name.Contains(normalizedKeyword));
            }
            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageResult<InventoryCategoryDto>
            {
                Results = await _mapper.ProjectTo<InventoryCategoryDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task<bool> HasPost(Guid invtCategoryId)
        {
            return await _context.Inventories.AnyAsync(x => x.InvtCategoryId == invtCategoryId);
        }
        public async Task<InventoryCategoryDto> GetBySlug(string Slug)
        {
            var query = await _context.InventoryCategories.FirstOrDefaultAsync(x => x.Slug == Slug);
            if (query == null) { throw new Exception($" not found{Slug}"); }
            return _mapper.Map<InventoryCategoryDto>(query);
        }
    }
}