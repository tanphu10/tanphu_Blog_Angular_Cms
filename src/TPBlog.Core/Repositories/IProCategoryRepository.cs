using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IProCategoryRepository : IRepository<IC_ProductCategory, Guid>
    {
        Task<PageResult<ProductCategoryDto>> GetPagingProductCategoryAsync(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasProduct(Guid categoryId);
        Task<ProductCategoryDto> GetBySlug(string Slug);
    }
}
