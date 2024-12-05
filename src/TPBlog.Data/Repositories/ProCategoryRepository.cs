using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Helpers;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class ProCategoryRepository : RepositoryBase<IC_ProductCategory, Guid>, IProCategoryRepository
    {
        private readonly IMapper _mapper;

        public ProCategoryRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<ProductCategoryDto> GetBySlug(string Slug)
        {
            var query = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Slug == Slug);
            if (query == null) { throw new Exception($" not found {Slug}"); }
            return _mapper.Map<ProductCategoryDto>(query);

        }

        public async Task<PageResult<ProductCategoryDto>> GetPagingProductCategoryAsync(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.ProductCategories.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                var normalizedKeyword = TextNormalizedName.ToTextNormalizedString(keyword);
                query = query.Where(x => x.Slug.Contains(normalizedKeyword) ||
                         x.Name.Contains(normalizedKeyword));
            }
            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageResult<ProductCategoryDto>
            {
                Results = await _mapper.ProjectTo<ProductCategoryDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<bool> HasProduct(Guid categoryId)
        {
            return await _context.Products.AnyAsync(x => x.ProCategoryId == categoryId);
        }
    }
}
